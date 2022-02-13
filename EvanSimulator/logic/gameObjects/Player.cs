using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvanSimulator.logic.gameObjects
{
    internal class Player : Person
    {

        int shootCooldown = 0;

        public Player(Form game, PointF position) : base(
            game,
            new Dictionary<string, string>()
            {
                { "default", "Sprites/player/player.png" },
                { "left", "Sprites/player/playerLeft.png" },
                { "shootCooldownBar", "sprites/gui/shootCooldown.png" },
                { "barBackground", "sprites/gui/barBackground.png" }
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
            /*
            if (key == "crouch")
            {
                Crouch();
            }
            */

            if (key == "shoot")
            {
                Shoot();
            }
        }
        /*
        public override void OnKeyUp(string key)
        {
            
            if (key == "crouch")
            {
                UnCrouch();
            }
        }
        */

        void Shoot()
        {
            if(shootCooldown > 0)
            {
                return;
            }

            shootCooldown = 15;

            PointF shootFrom = GetShootFrom();
            PointF startingVel = Util.SubtractPositions(game.mousePos, shootFrom);
            startingVel = Util.NormalizeVector(startingVel);
            startingVel = Util.ScaleVector(startingVel, 50f);

            game.Spawn(
                ("bean-" + Util.RandomString(game, 69)),
                new Bullet(
                    game,
                    shootFrom,
                    Util.AddPositions(velocity, startingVel)
                )
            );
        }
        void getInput()
        {
            left = game.inputKeys["left"].pressed;
            right = game.inputKeys["right"].pressed;
            jump = game.inputKeys["jump"].pressed;
            crouch = game.inputKeys["crouch"].pressed;
        }

        public override void Render()
        {
            if (shootCooldown > 0)
            {
                shootCooldown--;
            }
            getInput();

            base.Render();
        }

        /*
        bool isBelow()
        {
            PointF checkPoint1 = new PointF(position.X, position.Y * -2);
            PointF checkPoint2 = new PointF(position.X + size.X, position.Y * -2);
            foreach (string go in game.gameObjects.Keys.ToList())
            {
                if (go != ID && game.gameObjects[go].hasCollision)
                {
                    GameObject obj = game.gameObjects[go];
                    if ((obj.position.X < checkPoint1.X && checkPoint1.X < obj.position.X + obj.size.X) && (obj.position.Y < checkPoint1.Y && checkPoint1.Y < obj.position.Y + obj.size.Y))
                    {
                        if ((obj.position.X < checkPoint2.X && checkPoint2.X < obj.position.X + obj.size.X) && (obj.position.Y < checkPoint2.Y && checkPoint2.Y < obj.position.Y + obj.size.Y))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        */
        
        public override void GuiRender()
        {
            game.graphics.DrawImage(
                sprites["barBackground"],
                10,
                game.height - 20,
                30,
                10
            );

            game.graphics.DrawImage(
                sprites["shootCooldownBar"],
                10,
                game.height - 20,
                30 - (shootCooldown * 2),
                10
            );

            base.GuiRender();
        }
    }
}
