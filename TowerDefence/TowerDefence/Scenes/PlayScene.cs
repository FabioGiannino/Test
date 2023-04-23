using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace TowerDefence
{
    class PlayScene : Scene
    {
        protected Background Bg;
        public Player Player { get; protected set; }
        public Enemy Enemy { get; protected set; }

        public PlayScene() : base()
        {

        }

        public override void Start()
        {
            LoadAssets();

            Bg = new Background();

            Player = new Player(Game.GetController(0), 0);
            Player.Position = new Vector2(8, 8);

            base.Start();
        }

        protected override void LoadAssets()
        {
            //BG
            GfxMngr.AddTexture("bg", "Assets/bg_0.png");

            //images
            GfxMngr.AddTexture("player", "Assets/Player.png");
            GfxMngr.AddTexture("enemy", "Assets/Enemy.png");

            //GUI
            GfxMngr.AddTexture("barFrame", "Assets/loadingBar_frame.png");
            GfxMngr.AddTexture("blueBar", "Assets/loadingBar_bar.png");

            //fonts
            FontMngr.AddFont("stdFont", "Assets/textSheet.png", 15, 32, 20, 20);
            FontMngr.AddFont("comics", "Assets/comics.png", 10, 32, 61, 65);
        }

        public override void Input()
        {
            Player.Input();
        }

        public override void Update()
        {
            if (!Player.IsAlive)
                IsPlaying = false;

            PhysicsMngr.Update();
            UpdateMngr.Update();
            Bg.Update();

            PhysicsMngr.CheckCollisions();
        }

        public override Scene OnExit()
        {
            Player = null;
            Bg = null;

            UpdateMngr.ClearAll();
            PhysicsMngr.ClearAll();
            DrawMngr.ClearAll();
            GfxMngr.ClearAll();
            FontMngr.ClearAll();

            return base.OnExit();
        }

        public override void Draw()
        {
            Bg.Draw();
            DrawMngr.Draw();
        }
    }
}
