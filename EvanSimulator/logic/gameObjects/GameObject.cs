using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvanSimulator.logic
{
    public class GameObject
    {
        public string ID;

        internal Form game;

        public bool hasCollision = true;
        public int collisionGroup =  0;
        public Dictionary<string, Image> sprites = new Dictionary<string, Image>();
        public string spriteToUse = "default";

        public PointF position = new PointF(100.0F, 100.0F);
        public PointF size = new PointF(1F, 1F);


        public GameObject(Form game, string spriteFile, PointF position)
        {
            setup(
                game,
                new Dictionary<string, string>()
                {
                    { "default", spriteFile }
                },
                position
            );
        }

        public GameObject(Form game, Dictionary<string, string> spriteFiles, PointF position)
        {
            setup(
                game,
                spriteFiles,
                position
            );
        }

        private void setup(Form game, Dictionary<string, string> spriteFiles, PointF position) {
            this.game = game;

            if (!spriteFiles.ContainsKey("default"))
            {
                throw new Exception("All sprites must contain a 'default' frame");
            }

            foreach (KeyValuePair<string, string> sprite in spriteFiles)
            {
                sprites.Add(sprite.Key, Image.FromFile(game.assetsFolder + sprite.Value));
            }

            size.X = sprites["default"].Width;
            size.Y = sprites["default"].Height;

            this.position = position;
        }

        public PointF GetCenter()
        {
            return Util.AddPositions(position, Util.ScaleVector(size, 0.5f));
        }

        public virtual void Render()
        {
            game.graphics.DrawImage(sprites[spriteToUse], position.X, position.Y, size.X, size.Y);
        }

        public virtual void GuiRender() { }

        public virtual void OnKeyDown(string key)
        {

        }

        public virtual void OnKeyUp(string key)
        {

        }

        public void Despawn()
        {
            game.Despawn(ID);
        }
    }
}