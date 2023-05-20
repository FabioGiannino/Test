using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace TowerDefence
{
    class Scene1 : Scene
    {
        protected Map Map;

        private bool clickedL = false;
        private bool clickedR = false;
        private bool clickedC = false;

        public Scene1() : base()
        {

        }

        public override void Start()
        {
            LoadAssets();
            base.Start();
            Map = new Map(Game.HorizontalCells, Game.VerticalCells);
        }

        protected override void LoadAssets()
        {
            GFXMngr.AddTexture("road", "Assets/Road.png");
            GFXMngr.AddTexture("block", "Assets/Block.png");
            GFXMngr.AddTexture("water", "Assets/Sea.png");
            GFXMngr.AddTexture("forest", "Assets/Forest.png");
        }

        public override void Input()
        {
            
            // Toggle Map Forest (ON/OFF)
            if (Game.Window.MouseRight)
            {
                if (!clickedR)
                {
                    clickedR = true;
                    Map.ToggleForest((int)Game.Window.MouseX, (int)Game.Window.MouseY);
                    //List<Node> path = map.GetPath(agent.X, agent.Y, (int)mousePos.X, (int)mousePos.Y);
                    //agent.SetPath(path);
                }
            }
            else if (clickedR)
            {
                clickedR = false;
            }

            // Toggle Map Block (ON/OFF)
            /*if (Game.Window.MouseLeft)
            {
                if (!clickedL)
                {
                    clickedL = true;
                    Map.ToggleBlock((int)Game.Window.MouseX, (int)Game.Window.MouseY);
                    //List<Node> path = map.GetPath(agent.X, agent.Y, (int)mousePos.X, (int)mousePos.Y);
                    //agent.SetPath(path);
                }
            }
            else if (clickedL)
            {
                clickedL = false;
            }*/

            // Toggle Map Sea (ON/OFF)
            if (Game.Window.GetKey(KeyCode.C))
            {
                if (!clickedC)
                {
                    clickedC = true;
                    Map.ToggleSea((int)Game.Window.MouseX, (int)Game.Window.MouseY);
                    //List<Node> path = map.GetPath(agent.X, agent.Y, (int)mousePos.X, (int)mousePos.Y);
                    //agent.SetPath(path);
                }
            }
            else if (clickedC)
            {
                clickedC = false;
            }
        }

        public override void Update()
        {
            base.Update();
            Map.WaveFunctionCollapse();
        }


        public override void Draw()
        {
            Map.Draw();
        }
    }
}
