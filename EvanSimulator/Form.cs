using EvanSimulator.logic;
using EvanSimulator.logic.gameObjects;
using System.Diagnostics;

namespace EvanSimulator
{
    public partial class Form : System.Windows.Forms.Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int key);

        public bool running = true;
        public Stopwatch stopWatch = new Stopwatch();
        public Stopwatch frameTimer = new Stopwatch();

        public Thread gameThread;

        //public string assetsFolder = "C:\\Users/Austin/source/repos/EvanSimulator/EvanSimulator/assets/";
        public string assetsFolder = "C:\\Users/Billy George/source/repos/EvanSimulator/EvanSimulator/assets/";

        public Bitmap bmp;
        public Graphics graphics;

        public int width;
        public int height;

        public Random random = new Random(69);


        //use https://keycode.info/ to get keycodes
        public Dictionary<string, InputKey> inputKeys = new Dictionary<string, InputKey>()
        {
            { "jump", new InputKey(new int[] { 38, 87 }) },
            { "crouch", new InputKey(new int[] { 16, 40, 83 }) },
            { "shoot", new InputKey(new int[] { 32 }) },
            { "left", new InputKey(new int[] { 37, 65 }) },
            { "right", new InputKey(new int[] { 39, 68 }) },
        };


        private Dictionary<string, GameObject> gameObjects = new Dictionary<string, GameObject>();

        private Queue<Action> toDoInGameThread = new Queue<Action>();

        public Form()
        {
            //assetsFolder = Assembly.GetExecutingAssembly().Location;
            //assetsFolder = Directory.GetCurrentDirectory();
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            width = pictureBox1.Width;
            height = pictureBox1.Height;

            bmp = new Bitmap(width, height);
            pictureBox1.Image = bmp;

            graphics = Graphics.FromImage(bmp);

            Spawn("wayeyBoi1", new WaveyBoi(this, new PointF(5.0F,5.0F)));
            Spawn("player", new Player(this, new PointF(100.0F,100.0F)));

            //gameObjects["player"].size = new PointF(50f, 50f);

            stopWatch.Start();
            gameThread = new Thread(Render);
            gameThread.Start();
        }

        public void Spawn(string id, GameObject toSpawn)
        {
            toSpawn.ID = id;
            gameObjects.Add(id, toSpawn);
        }

        public void Despawn(string id)
        {
            gameObjects.Remove(id);
        }

        public void Render()
        {
            while (running)
            {
                frameTimer.Restart();

                foreach (KeyValuePair<string, InputKey> inputKey in inputKeys)
                {
                    bool newPressed = false;
                    foreach (int keyCode in inputKey.Value.KeyCodes)
                    {
                        if (GetAsyncKeyState(keyCode) != 0x0)
                        {
                            newPressed = true;
                        }
                    }

                    if (newPressed && !inputKey.Value.pressed)
                    {
                        KeyDownEvent(inputKey.Key);
                    }

                    if (!newPressed && inputKey.Value.pressed)
                    {
                        KeyUpEvent(inputKey.Key);
                    }

                    inputKey.Value.pressed = newPressed;
                }

                graphics.Clear(Color.Green);

                while (toDoInGameThread.Count > 0)
                {
                    toDoInGameThread.Dequeue()();
                }

                //--- render start ---

                foreach (string go in gameObjects.Keys.ToList())
                {
                    gameObjects[go].Render();
                }

                //--- render end ---

                //pictureBox1.Image = bmp;
                //pictureBox1.Dispose();
                if (running)
                {
                    try
                    {
                        pictureBox1.Invoke(new Action(() =>
                        {
                            if (running)
                            {
                                try
                                {
                                    pictureBox1.Invalidate();
                                }
                                catch (ObjectDisposedException)
                                {
                                }
                            }
                        }));
                    }
                    catch (ObjectDisposedException)
                    {
                    }
                }

                frameTimer.Stop();

                long timeToSleep = (1000 / 60) - frameTimer.ElapsedMilliseconds;
                if(timeToSleep > 0)
                {
                    Thread.Sleep((int)timeToSleep);
                }
            }
        }
        void KeyDownEvent(string key)
        {
            foreach (string go in gameObjects.Keys.ToList())
            {
                gameObjects[go].OnKeyDown(key);
            }
        }

        void KeyUpEvent(string key)
        {
            foreach (string go in gameObjects.Keys.ToList())
            {
                gameObjects[go].OnKeyUp(key);
            }
        }

        private void Form_Closing(object sender, EventArgs e)
        {
            running = false;
            //gameThread.Join();
        }

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}