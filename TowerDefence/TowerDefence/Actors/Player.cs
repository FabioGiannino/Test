using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace TowerDefence
{
    class Player : Actor
    {
        protected ProgressBar nrgBar;
        protected TextObject playerName;
        protected int playerId;

        protected TextObject scoreText;
        protected int score;

        protected Controller controller;

        public override int Energy { get => base.Energy; set { base.Energy = value; nrgBar.Scale((float)value / (float)maxEnergy); } }

        public Player(Controller ctrl, int id = 0) : base("player")
        {
            IsActive = true;
            maxSpeed = 12.0f;

            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            RigidBody.Type = RigidBodyType.Player;
            RigidBody.AddCollisionType(RigidBodyType.Enemy | RigidBodyType.Tile);
            bulletType = BulletType.PlayerBullet;

            nrgBar = new ProgressBar("barFrame", "blueBar", new Vector2(0.037f, 0.037f));
            nrgBar.Position = new Vector2(0.5f, 1f);

            Vector2 playerNamePos = nrgBar.Position;
            playerNamePos.Y -= 0.5f;
            playerName = new TextObject(playerNamePos, $"Player {playerId + 1}", FontMngr.GetFont(), 0);
            playerName.IsActive = true;

            Vector2 scorePos = nrgBar.Position + new Vector2(0, 0.5f);
            scoreText = new TextObject(scorePos, "", FontMngr.GetFont(), 0);
            scoreText.IsActive = true;
            UpdateScore();

            Reset();

            shootVel = new Vector2(1000.0f, 0.0f);
            tripleShootAngle = MathHelper.DegreesToRadians(15.0f);

            controller = ctrl;

            UpdateMngr.AddItem(this);
            DrawMngr.AddItem(this);
        }

        protected void UpdateScore()
        {
            scoreText.Text = score.ToString("00000000");
        }

        public void AddScore(int points)
        {
            score += points;
            UpdateScore();
        }

        public void Input()
        {
            RigidBody.Velocity.X = controller.GetHorizontal() * maxSpeed;
            RigidBody.Velocity.Y = controller.GetVertical() * maxSpeed;
        }

        public override void OnCollide(Collision collisionInfo)
        {
            //((Enemy)collisionInfo.Collider).OnDie();
            //AddDamage(30);

            OnTileCollide(collisionInfo);
        }

        private void OnTileCollide(Collision collisionInfo)
        {
            if(collisionInfo.Delta.X < collisionInfo.Delta.Y)
            {
                // Horizontal Collision
                if(X < collisionInfo.Collider.X)
                {
                    // Left to Right
                    collisionInfo.Delta.X = -collisionInfo.Delta.X;
                }

                // Apply DeltaX
                X += collisionInfo.Delta.X;
                RigidBody.Velocity.X = 0.0f;
            }
            else
            {
                // Vertical Collision
                if (Y < collisionInfo.Collider.Y)
                {
                    // Top to Bottom
                    collisionInfo.Delta.Y = -collisionInfo.Delta.Y;
                    RigidBody.Velocity.Y = 0.0f;
                }
                else
                {
                    // Bottom to Top
                    RigidBody.Velocity.Y = -RigidBody.Velocity.Y * 0.8f;
                }

                // Apply DeltaY
                Y += collisionInfo.Delta.Y;

            }
        }

        public override void OnDie()
        {
            Console.WriteLine("Player is dead");
        }
    }
}
