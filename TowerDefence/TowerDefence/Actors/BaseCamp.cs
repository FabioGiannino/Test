using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace TowerDefence
{
    class BaseCamp : I_Drawable
    {
        private Sprite sprite;
        public BaseCamp(int x, int y)
        {
            sprite = new Sprite(1, 1);
            sprite.position = new Vector2(x, y);
        }

        public void Draw()
        {
            sprite.DrawTexture(GFXMngr.GetTexture("baseCamp"));
        }
    }
}
