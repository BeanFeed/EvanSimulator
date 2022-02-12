using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvanSimulator.logic.gameObjects
{
    internal class WaveyBoi : GameObject
    {
        public WaveyBoi(Form game, PointF position) : base(game, "Sprites/waveyBoi.png", position)
        {
            size.X = 100f;
            size.Y = 100f;
        }

        public override void Render()
        {
            position.X = MathF.Sin(game.stopWatch.ElapsedMilliseconds / 100) * 10f;
            
            base.Render();
        }
    }
}
