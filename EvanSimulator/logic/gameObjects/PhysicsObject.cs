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
        public float groundedDragMultiplier = 1.3f;

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
            velocity.Y += mass * 0.02f;

            position.X += velocity.X;
            position.Y += velocity.Y;

            velocity.X /= (grounded ? (drag * groundedDragMultiplier) : drag);
            velocity.Y /= drag;
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
                OnCollide(null);
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
                OnCollide(null);
            }
        }

        public virtual void OnCollide(GameObject against)
        {
            
        }

        void ObjectsCollision()
        {
            foreach (string go in game.gameObjects.Keys.ToList())
            {
                if (go != ID && game.gameObjects[go].hasCollision)
                {
                    ObjectCollision(game.gameObjects[go]);
                }
            }
        }

        void ObjectCollision(GameObject go)
        {
            float thisBottom = position.Y + size.Y;
            float thisRight = position.X + size.X;

            float otherBottom = go.position.Y + go.size.Y;
            float otherRight = go.position.X + go.size.X;

            bool leftOverlapX = position.X >= go.position.X && position.X <= otherRight;//left side of this is in other
            bool rightOverlapX = thisRight >= go.position.X && thisRight <= otherRight;//right side of this is in other

            float xOverlapAmt = Math.Max(
                Math.Max(
                    position.X - go.position.X,
                    otherRight - position.X
                ),
                Math.Max(
                    thisRight - go.position.X,
                    otherRight - thisRight
                )
            );

            bool overlapX = leftOverlapX || rightOverlapX;

            bool topOverlapY = position.Y >= go.position.Y && position.Y <= otherBottom;//top side of this is in other
            bool bottomOverlapY = thisBottom >= go.position.Y && thisBottom <= otherBottom;//bottom side of this is in other

            float yOverlapAmt = Math.Max(
                Math.Max(
                    position.Y - go.position.Y,
                    otherBottom - position.Y
                ),
                Math.Max(
                    thisBottom - go.position.Y,
                    otherBottom - thisBottom
                )
            );

            bool overlapY = topOverlapY || bottomOverlapY;

            if (overlapX && overlapY)
            {
                OnCollide(go);

                if (overlapX && xOverlapAmt > yOverlapAmt)
                {
                    if ((leftOverlapX && velocity.X < 0) || (rightOverlapX && velocity.X > 0))
                    {
                        velocity.X *= -bounce;
                    }

                    if (leftOverlapX)
                    {
                        position.X = otherRight + 1;
                    }

                    if (rightOverlapX)
                    {
                        position.X = (go.position.X - size.X) - 1;
                    }
                }


                if (overlapY && xOverlapAmt < yOverlapAmt)
                {
                    if ((topOverlapY && velocity.Y < 0) || (bottomOverlapY && velocity.Y > 0))
                    {
                        velocity.Y *= -bounce;
                    }

                    if (topOverlapY)
                    {
                        position.Y = otherBottom + 1;
                    }

                    if (bottomOverlapY)
                    {
                        grounded = true;
                        position.Y = (go.position.Y - size.Y) - 1;
                    }
                }

                if(go is PhysicsObject)
                {
                    velocity = Util.AddPositions(velocity, ((PhysicsObject)go).velocity);
                }
            }
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
