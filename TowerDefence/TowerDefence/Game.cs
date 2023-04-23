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
        // Variables
        public static Window Window;

        private static KeyboardController keyboardCtrl;
        private static List<Controller> controllers;

        public static float OptimalScreenHeight;
        public static float UnitSize { get; private set; }
        public static float OptimalUnitSize { get; private set; }



        // Properties
        public static Scene CurrentScene { get; private set; }
        public static float DeltaTime { get { return Window.DeltaTime; } }
        public static float ScreenCenterX { get { return Window.Width * 0.5f; } }
        public static float ScreenCenterY { get { return Window.Height * 0.5f; } }

        public static void Init()
        {
            Window = new Window(1280, 720, "TowerDefence", false);
            Window.SetVSync(false);
            Window.SetDefaultViewportOrthographicSize(10);

            OptimalScreenHeight = 1080;//best resolution
            UnitSize = Window.Height / Window.OrthoHeight;//72
            OptimalUnitSize = OptimalScreenHeight / Window.OrthoHeight;//108

            // SCENES
            PlayScene playScene = new PlayScene();
            playScene.NextScene = null;

            CurrentScene = playScene;

            // CONTROLLERS
            // Always create a keyboard controller (init at 0 cause we only have 1 keyboard)
            keyboardCtrl = new KeyboardController(0);

            // Check how many joypads are currently connected
            string[] joysticks = Window.Joysticks;
            controllers = new List<Controller>();

            for (int i = 0; i < joysticks.Length; i++)
            {
                Console.WriteLine(Window.JoystickDebug(i));
                Console.WriteLine(joysticks[i]);

                if (joysticks[i] != null) // && joysticks[i] != "Unmapped Controller")
                {
                    controllers.Add(new JoypadController(i));
                }
            }
        }

        public static float PixelsToUnits(float pixelsSize)
        {
            return pixelsSize / OptimalUnitSize;
        }

        public static Controller GetController(int index)
        {
            Controller ctrl = keyboardCtrl;

            if (index < controllers.Count)
            {
                ctrl = controllers[index];
            }

            return ctrl;
        }

        public static void Play()
        {
            CurrentScene.Start();

            while (Window.IsOpened)
            {
                // Show FPS on Window Title Bar
                //Window.SetTitle($"FPS: {1f / Window.DeltaTime}");

                // Exit when ESC is pressed
                if (Window.GetKey(KeyCode.Esc))
                {
                    break;
                }

                if (!CurrentScene.IsPlaying)
                {
                    Scene nextScene = CurrentScene.OnExit();

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

                // INPUT
                CurrentScene.Input();

                // UPDATE
                CurrentScene.Update();

                // DRAW
                CurrentScene.Draw();



                Window.Update();
            }
        }
    }
}
