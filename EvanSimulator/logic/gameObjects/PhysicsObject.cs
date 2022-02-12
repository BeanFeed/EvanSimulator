using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvanSimulator.logic.gameObjects
{
    internal class PhysicsObject : GameObject
    {
        public float mass;
        public float drag;
        public float bounce;//should be 0 and 1
        public PointF velocity = new PointF();
        public bool grounded = false;

        public PhysicsObject(Form game, string spriteFile, PointF position, float mass, float drag, float bounce) : base(game, spriteFile, position)
        {
            this.mass = mass;
            this.drag = drag;
            this.bounce = bounce;
        }

        public PhysicsObject(Form game, Dictionary<string, string> spriteFiles, PointF position, float mass, float drag, float bounce) : base(game, spriteFiles, position)
        {
            this.mass = mass;
            this.drag = drag;
            this.bounce = bounce;
        }

        void Velocity()
        {
            velocity.Y += mass * 0.001f;

            position.X += velocity.X;
            position.Y += velocity.Y;

            velocity.X *= drag;
            velocity.Y *= drag;
        }

        void EdgeCollision()
        {
            if(
                position.X < 0 && velocity.X < 0 ||
                position.X + size.X > game.width && velocity.X > 0
            )
            {
                if(position.X < 0)
                {
                    position.X = 0;
                }

                if (position.X + size.X > game.width)
                {
                    position.X = game.width - size.X;
                }

                velocity.X *= -bounce;
            }

            if (
                position.Y < 0 && velocity.Y < 0 ||
                position.Y + size.Y > game.height && velocity.Y > 0
            )
            {
                if (position.Y < 0)
                {
                    position.Y = 0;
                }

                if (position.Y + size.Y >= game.height) {
                    grounded = true;
                }

                if (position.Y + size.Y > game.height)
                {
                    position.Y = game.height - size.Y;
                }

                velocity.Y *= -bounce;
            }
        }

        void ObjectsCollision()
        {
            //TODO
        }

        void Physics()
        {
            Velocity();

            grounded = false;
            //one of the following collision can set grounded to true
            EdgeCollision();
            ObjectsCollision();
        }

        public override void Render()
        {
            Physics();
            base.Render();
        }
    }
}
