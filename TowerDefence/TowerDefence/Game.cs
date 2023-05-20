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

        #region ParametersMap        
        private static int horizontalCells = 20;
        private static int verticalCells = 10;
        private static int sizeCell = 80;
        public static int SizeCell { get { return sizeCell; } }
        public static int HorizontalCells { get { return horizontalCells; } }
        public static int VerticalCells { get { return verticalCells; } }
        #endregion

        public static Window Window;
        public static Scene CurrentScene { get; private set; }
        public static float DeltaTime { get { return Window.DeltaTime; } }
        private static int windowWidth;
        private static int windowHeight;

        public static void Init()
        {
            windowHeight = sizeCell * horizontalCells;
            windowWidth = sizeCell * verticalCells;
            Window = new Window(windowHeight,windowWidth, "Tower Defence");
            Window.SetDefaultViewportOrthographicSize(verticalCells);
            Scene1 playScene = new Scene1();
            playScene.NextScene = playScene;

            CurrentScene = playScene;
        }

        public static void Play()
        {
            CurrentScene.Start();
            while (Window.IsOpened)
            {
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
                CurrentScene.Update();
                CurrentScene.Draw();


                Window.Update();
            }
        }
    }
}
