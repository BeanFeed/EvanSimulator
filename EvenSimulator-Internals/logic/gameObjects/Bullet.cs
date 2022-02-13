using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvanSimulator.logic.gameObjects
{
    internal class Bullet : PhysicsObject
    {
        private float startSpeed = 15;
        public Bullet(Form game, PointF position, string dir, PointF pVel) : base(
            game,
            new Dictionary<string, string>()
            {
                { "default", "Sprites/beans/baked/bean1.png" },
                { "var1", "Sprites/beans/baked/bean2.png" },
                { "var2", "Sprites/beans/baked/bean3.png" },
                { "var3", "Sprites/beans/baked/bean4.png" }

            },
            position,
            10,//mass
            0.99f,//drag
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

            spriteToUse = sprites.ElementAt(game.random.Next(0, sprites.Count)).Key;


            size = new PointF(10f, 10f);
        }

        public override void Render()
        {
            if(MathF.Abs(velocity.X) < 1f && MathF.Abs(velocity.Y) < 1f)
            {
                Despawn();
            }

            base.Render();
        }

    }
}
