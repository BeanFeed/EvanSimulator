using EvanSimulator.logic;
using EvanSimulator.logic.gameObjects;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Input;

namespace EvanSimulator
{
    public partial class Form : System.Windows.Forms.Form
    {
        public bool running = true;
        public Stopwatch stopWatch = new Stopwatch();

        public Thread gameThread;

        public string assetsFolder = "C:\\Users/Austin/source/repos/EvanSimulator/EvanSimulator/assets/";

        public Bitmap bmp;
        public Graphics graphics;

        public int width;
        public int height;

        public Random random = new Random(69);


        private Dictionary<string, GameObject> gameObjects = new Dictionary<string, GameObject>();

        private Queue<Action> toDoInGameThread = new Queue<Action>();

        public Form()
        {
            //assetsFolder = Assembly.GetExecutingAssembly().Location;
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
                graphics.Clear(Color.Green);

                while(toDoInGameThread.Count > 0)
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
                    pictureBox1.Invoke(new Action(() => { if (running) { pictureBox1.Invalidate(); } }));
                }
                Thread.Sleep(1);
            }
        }

        private void Form_Closing(object sender, EventArgs e)
        {
            running = false;
            gameThread.Join();
        }

        void OnKeyDown(object sender, KeyEventArgs e)
        {
            toDoInGameThread.Enqueue(() => {
                foreach (string go in gameObjects.Keys.ToList())
                {
                    gameObjects[go].OnKeyDown(e.KeyCode);
                }
            });
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        void OnKeyUp(object sender, KeyEventArgs e)
        {
            toDoInGameThread.Enqueue(() => {
                foreach (string go in gameObjects.Keys.ToList())
                {
                    gameObjects[go].OnKeyUp(e.KeyCode);
                }
            });
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}