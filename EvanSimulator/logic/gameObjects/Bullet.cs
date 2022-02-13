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
        public int bounceCount = 0;

        static Dictionary<string, string> spritesCouldUse = new Dictionary<string, string>()
        {
            { "default", "Sprites/beans/baked/bean1.png" },
            { "var1", "Sprites/beans/baked/bean2.png" },
            { "var2", "Sprites/beans/baked/bean3.png" },
            { "var3", "Sprites/beans/baked/bean4.png" }

        };

        public Bullet(Form game, PointF position, PointF startingVelocity) : base(
            game,
            Util.RandomDictItem(game, spritesCouldUse).Value,
            position,
            10,//mass
            //0,//mass
            1.001f,//drag, 1 is no drag, higher is more
            //1f,//drag, 1 is no drag, higher is more
            0.9f//bounce
            //0f//bounce
        )
        {
            velocity = startingVelocity;
            size = new PointF(10f, 10f);
        }

        public override void OnCollide(GameObject against)
        {
            bounceCount++;
            if(bounceCount > 1)
            {
                Despawn();
            }
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
