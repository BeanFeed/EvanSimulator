using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvanSimulator.logic.gameObjects
{
    internal class Player : PhysicsObject
    {
        float speed = 5;

        bool left;
        bool right;

        bool crouched = false;

        public Player(Form game, PointF position) : base(
            game,
            new Dictionary<string, string>()
            {
                { "default", "Sprites/player/player.png" },
                { "left", "Sprites/player/playerLeft.png" }
            },
            position,
            100,//mass
            0.99f,//drag
            0f//bounce
        )
        {
            size.X = 100f;
            size.Y = 100f;
        }

        public override void OnKeyDown(Keys key)
        {
            if(key == Keys.A || key == Keys.Left)
            {
                left = true;
            }

            if (key == Keys.D || key == Keys.Right)
            {
                right = true;
            }

            if(key == Keys.W && grounded)
            {
                velocity.Y = -10;
            }

            if (key == Keys.S || key == Keys.Down)
            {
                if (!crouched)
                {
                    crouched = true;
                    size.Y /= 2;
                    position.Y += size.Y;
                    speed /= 2;
                }
            }

            if (key == Keys.Space)
            {
                game.Spawn(
                    ("bean-" + game.RandomString(69)),
                    new Bullet(
                        game,
                        new PointF(
                            position.X + size.X / 2,
                            position.Y + size.Y / 2
                        ),
                        (spriteToUse == "left" ? "left" : "right"),
                        velocity
                        
                    )
                );
            }
        }

        public override void OnKeyUp(Keys key)
        {
            if (key == Keys.A || key == Keys.Left)
            {
                left = false;
            }

            if (key == Keys.D || key == Keys.Right)
            {
                right = false;
            }

            if (key == Keys.S || key == Keys.Down)
            {
                if (crouched)
                {
                    crouched = false;
                    position.Y -= size.Y;
                    size.Y *= 2;
                    speed *= 2;
                }
            }
        }

        void getInput()
        {
            if (left && right)
            {

            }
            else if (left)
            {
                velocity.X = -speed;
                spriteToUse = "left";
            }
            else if (right)
            {
                velocity.X = speed;
                spriteToUse = "default";
            }
        }
        public override void Render()
        {
            getInput();

            base.Render();
        }
    }
}
