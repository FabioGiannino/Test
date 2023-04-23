using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace TowerDefence
{
    class Background : I_Updatable, I_Drawable
    {
        private Texture texture;
        private Sprite sprite;

        public DrawLayer Layer { get; protected set; }

        public Background()
        {
            Layer = DrawLayer.Background;

            texture = GfxMngr.GetTexture("bg");
            sprite = new Sprite(texture.Width, texture.Height);

            float[] positionsY = new float[] { 0, 3.8f };

            UpdateMngr.AddItem(this);
            DrawMngr.AddItem(this);
        }

        public void Update()
        {
            
        }

        public void Draw()
        {
            sprite.DrawTexture(texture);

        }
    }
}
