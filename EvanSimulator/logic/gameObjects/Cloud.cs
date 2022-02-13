using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvanSimulator.logic.gameObjects
{
    internal class Cloud : GameObject
    {
        int counter = 0;

        static Dictionary<string, string> spritesCouldUse = new Dictionary<string, string>()
        {
            { "default", "sprites/world/clouds/cloud1.png" },
            { "var1", "sprites/world/clouds/cloud2.png" },
            { "var2", "sprites/world/clouds/cloud3.png" }
        };

        public Cloud(Form game, PointF position) : base(game, Util.RandomDictItem(game, spritesCouldUse).Value, position)
        {
            size.X = 100f;
            size.Y = 50f;
            counter = game.random.Next(0, 10000);
        }

        public override void Render()
        {
            counter++;
            position.X = (MathF.Sin(counter / 1000f) * game.width / 2f) + (game.width / 2f);
            
            base.Render();
        }
    }
}
