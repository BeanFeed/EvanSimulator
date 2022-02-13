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
        float jumpPower = 30;

        int timeSinceLastJump = 0;

        public bool crouched = false;

        public Player(Form game, PointF position) : base(
            game,
            new Dictionary<string, string>()
            {
                { "default", "Sprites/player/player.png" },
                { "left", "Sprites/player/playerLeft.png" }
            },
            position,
            100,//mass
            1.001f,//drag, 1 is no drag, higher is more 
            0f//bounce
        )
        {
            size.X = 40f;
            size.Y = 100f;
        }

        public override void OnKeyDown(string key)
        {
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
                    ("bean-" + Util.RandomString(game, 69)),
                    new Bullet(
                        game,
                        new PointF(
                            position.X + (size.X * 0.5f),
                            position.Y + (size.Y * 0.7f)
                        ),
                        (spriteToUse == "left" ? "left" : "right"),
                        velocity
                        
                    )
                );
            }
        }

        public override void OnKeyUp(string key)
        {
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
            if (game.inputKeys["left"].pressed && game.inputKeys["right"].pressed)
            {

            }
            else if (game.inputKeys["left"].pressed)
            {
                velocity.X = -speed;
                spriteToUse = "left";
            }
            else if (game.inputKeys["right"].pressed)
            {
                velocity.X = speed;
                spriteToUse = "default";
            }

            if (game.inputKeys["jump"].pressed && grounded && timeSinceLastJump > 5)
            {
                velocity.Y = -(crouched ? (jumpPower / 2) : jumpPower);
                timeSinceLastJump = 0;
            }
        }
        public override void Render()
        {
            timeSinceLastJump++;
            getInput();

            base.Render();
        }
    }
}
