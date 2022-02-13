using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvanSimulator.logic.gameObjects
{
    internal class Bullet : PhysicsObject
    {
        private float startSpeed = 30f;

        static Dictionary<string, string> spritesCouldUse = new Dictionary<string, string>()
        {
            { "default", "Sprites/beans/baked/bean1.png" },
            { "var1", "Sprites/beans/baked/bean2.png" },
            { "var2", "Sprites/beans/baked/bean3.png" },
            { "var3", "Sprites/beans/baked/bean4.png" }

        };

        public Bullet(Form game, PointF position, string dir, PointF pVel) : base(
            game,
            Util.RandomDictItem(game, spritesCouldUse).Value,
            position,
            10,//mass
            1.001f,//drag, 1 is no drag, higher is more
            0.9f//bounce
        )
        {
            if (dir == "left")
            {
                velocity.X = pVel.X - startSpeed;
            }
            
            if (dir == "right")
            {
                velocity.X = pVel.X + startSpeed;
            }


            size = new PointF(10f, 10f);
        }

        public override void Render()
        {
            if(MathF.Abs(velocity.X) < 0.2f && MathF.Abs(velocity.Y) < 0.2f)
            {
                Despawn();
            }

            base.Render();
        }

    }
}
