using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvanSimulator.logic.gameObjects
{
    internal class Player : PhysicsObject
    {
        float speed = 10;
        float jumpPower = 40;

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
            0.9f,//drag
            0f//bounce
        )
        {
            size.X = 100f;
            size.Y = 100f;
        }

        public override void OnKeyDown(string key)
        {
            if(key == "left")
            {
                left = true;
            }

            if (key == "right")
            {
                right = true;
            }

            if(key == "jump" && grounded)
            {
                velocity.Y = -jumpPower;
            }

            if (key == "crouch")
            {
                if (!crouched)
                {
                    crouched = true;
                    size.Y /= 2;
                    position.Y += size.Y;
                    speed /= 2;
                }
            }

            if (key == "shoot")
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

        public override void OnKeyUp(string key)
        {
            if (key == "left")
            {
                left = false;
            }

            if (key == "right")
            {
                right = false;
            }

            if (key == "crouch")
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
