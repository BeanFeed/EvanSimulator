using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvanSimulator.logic.gameObjects
{
    internal class Person : PhysicsObject
    {
        internal float speed = 3;
        internal float hSpeedCap = 10;
        internal float jumpPower = 30;

        internal int timeSinceLastJump = 0;


        //controls
        public bool left;
        public bool right;
        public bool jump;

        public bool crouched = false;
        
        public Person(Form game, Dictionary<string, string> spriteFiles, PointF position, float mass, float drag, float bounce) : base(game, spriteFiles, position, mass, drag, bounce)
        {
        }

        public void Crouch()
        {
            if (!crouched)
            {
                crouched = true;
                size.Y /= 2;
                position.Y += size.Y;
                speed /= 2;
            }
        }

        public void UnCrouch()
        {
            if (crouched)
            {
                crouched = false;
                position.Y -= size.Y;
                size.Y *= 2;
                speed *= 2;
            }
        }

        public PointF GetShootFrom()
        {
            return new PointF(
                position.X + (size.X * 0.5f),
                position.Y + (size.Y * 0.7f)
            );
        }

        public virtual void Shoot()
        {
            
        }

        public void MoveLeft()
        {
            if(velocity.X > -hSpeedCap)
            {
                velocity.X -= speed;
            }
            spriteToUse = "left";
        }

        public void MoveRight()
        {
            if (velocity.X < hSpeedCap)
            {
                velocity.X += speed;
            }
            spriteToUse = "default";
        }

        public void Jump()
        {
            velocity.Y = -(crouched ? (jumpPower / 2) : jumpPower);
            timeSinceLastJump = 0;
        }

        public void HandleControlValues()
        {
            if (left && right)
            {

            }
            else if (left)
            {
                MoveLeft();
            }
            else if (right)
            {
                MoveRight();
            }

            if (jump && grounded && timeSinceLastJump > 5)
            {
                Jump();
            }
        }

        public override void Render()
        {
            timeSinceLastJump++;
            HandleControlValues();

            base.Render();
        }

    }
}
