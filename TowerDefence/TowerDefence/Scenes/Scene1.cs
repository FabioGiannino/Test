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
        private bool clickedSpace = false;

        public Scene1() : base()
        {

        }



        public override void Start()
        {
            //M.C.D tra 1280 e 720 è 80, che è il numero di celle
            base.Start();
            Map = new Map(10, 10, 100);
        }




        protected override void LoadAssets()
        {
            throw new NotImplementedException();
        }



        public override void Input()
        {
            
            /*
            if (Game.Window.MouseLeft)
            {
                if (!clickedL)
                {
                    clickedL = true;
                    mousePos = new Vector2(Game.Window.MouseX, Game.Window.MouseY);
                    List<Node> path = map.GetPath(agent.X, agent.Y, (int)mousePos.X, (int)mousePos.Y);
                    //agent.SetPath(path);
                }
            }
            else if (clickedL)
            {
                clickedL = false;
            }*/



            // Toggle Map Node (ON/OFF)
            if (Game.Window.MouseRight)
            {
                if (!clickedR)
                {
                    clickedR = true;
                    Map.ToggleNode((int)Game.Window.MouseX, (int)Game.Window.MouseY);
                    //List<Node> path = map.GetPath(agent.X, agent.Y, (int)mousePos.X, (int)mousePos.Y);
                    //agent.SetPath(path);
                }
            }
            else if (clickedR)
            {
                clickedR = false;
            }


            // Toggle Map Obstacle (ON/OFF)
            if (Game.Window.GetKey(KeyCode.C))
            {
                if (!clickedSpace)
                {
                    clickedSpace = true;
                    Map.ToggleObstacle((int)Game.Window.MouseX, (int)Game.Window.MouseY);
                    //List<Node> path = map.GetPath(agent.X, agent.Y, (int)mousePos.X, (int)mousePos.Y);
                    //agent.SetPath(path);
                }
            }
            else if (clickedSpace)
            {
                clickedSpace = false;
            }
        }




        public override void Update()
        {
            base.Update();
        }


        public override void Draw()
        {
            Map.Draw();
        }
    }
}
