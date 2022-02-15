using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvanSimulator.logic.gameObjects
{
    internal class Enemy : Person
    {
        public Enemy(Form game, PointF position) : base(
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

            speed *= 0.7f;
            jumpPower *= 0.7f;
        }

        int shootCooldown = 1;
        GameObject bean;
        public override void Shoot()
        {
            PointF shootFrom = GetShootFrom();
            PointF player = game.gameObjects["player"].GetCenter();
            //PointF targetTra = new PointF(targetPos.X + velocity.X, targetPos.Y + velocity.Y);
            PointF startingVel = Util.SubtractPositions(player, shootFrom);
            startingVel = Util.ScaleVector(startingVel, 0.1f);

            bean = game.Spawn(
                    ("bean-" + Util.RandomString(game, 69)),
                    new Bullet(
                        game,
                        shootFrom,
                        startingVel
                    )
                );
            bean.hasCollision = false;
        }

        bool isClose()
        {
            PointF self = GetCenter();
            PointF player = game.gameObjects["player"].GetCenter();
            float distance = MathF.Sqrt(
                MathF.Pow(player.X - self.X, 2) + 
                MathF.Pow(player.Y - self.Y, 2)
            );

            return distance < 50000;
        }

        void followTarget()
        {
            PointF self = GetCenter();
            PointF target = game.gameObjects["player"].GetCenter();


            left = false;
            right = false;
            jump = false;

            if (Util.DstForm(self,target) > 200)
            {
                if (target.X < self.X)
                {
                    left = true;
                }
                else
                {
                    right = true;
                }
            }

            if(target.Y < position.Y - 50)
            {
                jump = true;
            }
        }

        void Attack()
        {
            shootCooldown--;
            if (isClose() && shootCooldown <= 0)
            {
                shootCooldown = 60 * 1;
                Shoot();
            }
        }
        public override void Render()
        {
            followTarget();
            
            Attack();
            base.Render();
        }

    }
}
