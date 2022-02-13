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

        void UpdateVelocity()
        {
            velocity.Y += mass * 0.02f;

            

            velocity.X /= (grounded ? (drag * groundedDragMultiplier) : drag);
            velocity.Y /= drag;
        }

        void ApplyVelocity()
        {
            position.X += velocity.X;
            position.Y += velocity.Y;
        }

        void EdgeCollision()
        {
            if(
                position.X <= 0 && velocity.X < 0 ||
                position.X + size.X >= game.width && velocity.X > 0
            )
            {
                if(position.X <= 0)
                {
                    position.X = 0;

                    if (velocity.X < 0)
                    {
                        velocity.X *= -bounce;
                    }
                }

                if (position.X + size.X >= game.width)
                {
                    position.X = game.width - size.X;

                    if (velocity.X > 0)
                    {
                        velocity.X *= -bounce;
                    }
                }

                velocity.X *= -bounce;
                OnCollide(null);
            }

            if (
                position.Y <= 0 && velocity.Y < 0 ||
                position.Y + size.Y >= game.height && velocity.Y > 0
            )
            {
                if (position.Y <= 0)
                {
                    position.Y = 0;

                    if (velocity.Y < 0)
                    {
                        velocity.Y *= -bounce;
                    }
                }

                if (position.Y + size.Y >= game.height) {
                    grounded = true;
                }

                if (position.Y + size.Y >= game.height)
                {
                    position.Y = game.height - size.Y;

                    if(velocity.Y > 0)
                    {
                        velocity.Y *= -bounce;
                    }
                }

                OnCollide(null);
            }
        }

        public virtual void OnCollide(GameObject? against)
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

        public bool OverlapsObject(GameObject go)
        {
            float thisBottom = position.Y + size.Y;
            float thisRight = position.X + size.X;

            float otherBottom = go.position.Y + go.size.Y;
            float otherRight = go.position.X + go.size.X;

            bool leftOverlapX = position.X >= go.position.X && position.X <= otherRight;//left side of this is in other
            bool rightOverlapX = thisRight >= go.position.X && thisRight <= otherRight;//right side of this is in other

            bool otherLeftOverlapX = go.position.X >= position.X && go.position.X <= thisRight;//left side of other is in this
            bool otherRightOverlapX = otherRight >= position.X && otherRight <= thisRight;//right side of other is in this

            bool overlapX = leftOverlapX || rightOverlapX || otherLeftOverlapX || otherRightOverlapX;

            bool topOverlapY = position.Y >= go.position.Y && position.Y <= otherBottom;//top side of this is in other
            bool bottomOverlapY = thisBottom >= go.position.Y && thisBottom <= otherBottom;//bottom side of this is in other

            bool otherTopOverlapY = go.position.Y >= position.Y && go.position.Y <= thisBottom;//left side of other is in this
            bool otherBottomOverlapY = otherBottom >= position.Y && otherBottom <= thisBottom;//right side of other is in this

            bool overlapY = topOverlapY || bottomOverlapY || otherTopOverlapY || otherBottomOverlapY;

            return overlapX && overlapY;
        }

        void ObjectCollision(GameObject go)
        {
            if (OverlapsObject(go))
            {
                OnCollide(go);

                if (MathF.Abs(position.X - (go.position.X + go.size.X)) < (size.X / 2))
                {
                    //left side touching - is right of other

                    if (velocity.X < 0)
                    {
                        velocity.X *= -bounce;
                    }
                    
                    position.X = go.position.X + go.size.X;
                }

                else if (MathF.Abs((position.X + size.X) - go.position.X) < (size.X / 2))
                {
                    //right side touching - is left of other

                    if (velocity.X > 0)
                    {
                        velocity.X *= -bounce;
                    }

                    position.X = go.position.X - size.X;
                }

                else if (MathF.Abs(position.Y - (go.position.Y + go.size.Y)) < (size.Y / 4))
                {
                    //top side touching - is below other

                    if (velocity.Y < 0)
                    {
                        velocity.Y *= -bounce;
                    }

                    position.Y = go.position.Y + go.size.Y;
                }

                else if (MathF.Abs((position.Y + size.Y) - go.position.Y) < (size.Y / 4))
                {
                    //bottom side touching - is above other

                    if (velocity.Y > 0)
                    {
                        velocity.Y *= -bounce;
                    }

                    position.Y = go.position.Y - size.Y;

                    grounded = true;
                }

                if (go is PhysicsObject)
                {
                    velocity = Util.AddPositions(velocity, ((PhysicsObject)go).velocity);
                }
            }
        }

        void Physics()
        {
            UpdateVelocity();

            grounded = false;
            //one of the following collision can set grounded to true
            EdgeCollision();
            ObjectsCollision();

            ApplyVelocity();
        }

        public override void Render()
        {
            Physics();
            base.Render();
        }
    }
}
