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

        public string assetsFolder = "C:\\Users/Austin/source/repos/EvanSimulator/EvanSimulator/assets/";
        //public string assetsFolder = "C:\\Users/Billy George/source/repos/EvanSimulator/EvanSimulator/assets/";

        public Bitmap bmp = new(1, 1);
        public Graphics graphics;

        public int width;
        public int height;

        public Random random = new(69);

        public PointF mousePos = new PointF();

        public Color backgroundColor = Color.Green;


        //use https://keycode.info/ to get keycodes
        public Dictionary<string, InputKey> inputKeys = new Dictionary<string, InputKey>()
        {
            { "jump", new InputKey(new int[] { 38, 32 }) },
            { "crouch", new InputKey(new int[] { 16, 40 }) },
            { "shoot", new InputKey(new int[] { 1 }) },
            { "left", new InputKey(new int[] { 37, 65 }) },
            { "right", new InputKey(new int[] { 39, 68 }) },
            { "test", new InputKey(new int[] { 70 }) },
        };


        public Dictionary<string, GameObject> gameObjects = new Dictionary<string, GameObject>();

        public Form()
        {
            //assetsFolder = Assembly.GetExecutingAssembly().Location;
            //assetsFolder = Directory.GetCurrentDirectory();
            gameThread = new Thread(Render);
            graphics = Graphics.FromImage(bmp);

            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            width = pictureBox1.Width;
            height = pictureBox1.Height;

            bmp = new Bitmap(width, height);
            pictureBox1.Image = bmp;

            graphics = Graphics.FromImage(bmp);

            for (int i = 0; i < 10; i++)
            {
                Spawn("cloud-" + i.ToString(), new Cloud(this, new PointF(0f, random.Next(5, 300))),1);
            }

            GameObject player = Spawn("player", new Player(this, new PointF(100.0F,100.0F)),1);
            //GameObject enemy1 = Spawn("enemy", new Enemy(this, new PointF(300.0F, 100.0F)));
            //enemy1.hasCollision = false;
            GameObject plat = new GameObject(this, "sprites/world/platforms/platform1.png", new PointF(400.0F, 325.0F));
            plat.size = new PointF(100, 50);
            GameObject platform1 = Spawn("platform-qw49e567sadkjfhj", plat,1);
            platform1.collisionGroup = 1;
            
            //gameObjects["player"].size = new PointF(50f, 50f);

            stopWatch.Start();
            gameThread.Start();
        }

        public GameObject Spawn(string id, GameObject toSpawn, int collisionGroup)
        {
            toSpawn.ID = id;
            gameObjects.Add(id, toSpawn);
            gameObjects[id].collisionGroup = collisionGroup;
            return gameObjects[id];

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

                mousePos.X = Cursor.Position.X - pictureBox1.Location.X - Location.X;
                mousePos.Y = Cursor.Position.Y - pictureBox1.Location.Y - Location.Y;

                mousePos = Util.ScaleVector(mousePos, 0.935f);
                
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

                graphics.Clear(backgroundColor);

                //--- render start ---

                foreach (string go in gameObjects.Keys.ToList())
                {
                    gameObjects[go].Render();
                }

                foreach (string go in gameObjects.Keys.ToList())
                {
                    gameObjects[go].GuiRender();
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

                //Thread.Sleep(100);
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
    }
}