using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvanSimulator.logic.gameObjects
{
    internal class WaveyBoi : GameObject
    {
        int counter = 0;

        public WaveyBoi(Form game, PointF position) : base(game, "Sprites/waveyBoi.png", position)
        {
            size.X = 100f;
            size.Y = 100f;
        }

        public override void Render()
        {
            counter++;
            position.X = MathF.Sin((float)(counter / Math.PI)) * 10f;
            
            base.Render();
        }
    }
}
