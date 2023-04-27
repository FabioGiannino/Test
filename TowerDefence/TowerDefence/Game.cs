using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;


namespace TowerDefence
{
    static class Game
    {

        private static int windowWidth = 800;
        private static int windowHeight = 800;

        public static Window Window;
        public static Scene CurrentScene { get; private set; }
        public static float DeltaTime { get { return Window.DeltaTime; } }


        public static void Init()
        {
            Window = new Window(windowWidth, windowHeight, "Tower Defence");
            Window.SetDefaultViewportOrthographicSize(10);
            Scene1 playScene = new Scene1();
            playScene.NextScene = playScene;

            CurrentScene = playScene;
        }

        public static void Play()
        {
            CurrentScene.Start();
            while (Window.IsOpened)
            {
                //Window.SetTitle($"FPS: {1f / Window.DeltaTime}");

                if (Window.GetKey(KeyCode.Esc))
                {
                    break;
                }
                if (!CurrentScene.IsPlaying)
                {
                    Scene nextScene = CurrentScene.NextScene;
                    if (nextScene != null)
                    {
                        CurrentScene = nextScene;
                        CurrentScene.Start();
                    }
                    else
                    {
                        return;
                    }
                }

                CurrentScene.Input();
               // CurrentScene.Update();
                CurrentScene.Draw();

                Window.Update();
            }
        }
    }
}
