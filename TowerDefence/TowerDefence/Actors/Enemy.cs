using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace TowerDefence
{

    enum EnemyType
    {
        Base, LAST
    }

    class Enemy : Actor
    {
        public EnemyType Type { get; protected set; }
        public int Points { get; protected set; }

        // FSM Values
        private float visionRadius;
        public float walkSpeed;
        public float followSpeed;
        private float shootDistance;

        private StateMachine fsm;

        public Enemy() : base("enemy", 0, 0, Game.PixelsToUnits(58), Game.PixelsToUnits(58))
        {
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            RigidBody.Type = RigidBodyType.Enemy;
            bulletType = BulletType.EnemyBullet;

            Position = new Vector2(2, 8);

            // FSM
            visionRadius = 5.0f;
            walkSpeed = 1.5f;
            followSpeed = walkSpeed * 2.0f;
            shootDistance = 3.0f;

            frameW = texture.Height;

            fsm = new StateMachine();
            fsm.GoTo(StateEnum.WALK);

            UpdateMngr.AddItem(this);
            DrawMngr.AddItem(this);

            Points = 10;

            IsActive = true;
        }

        public bool CanAttackPlayer()
        {
            Player player = ((PlayScene)Game.CurrentScene).Player;
            Vector2 dist = player.Position - Position;

            if(dist.LengthSquared < shootDistance * shootDistance)
            {
                return true;
            }

            return false;
        }

        public bool CanSeePlayer()
        {
            Player player = ((PlayScene)Game.CurrentScene).Player;
            Vector2 dist = player.Position - Position;

            return dist.LengthSquared < visionRadius * visionRadius;
        }

        public void HeadToPlayer()
        {
            Player player = ((PlayScene)Game.CurrentScene).Player;
            Vector2 dist = player.Position - Position;

            RigidBody.Velocity.X = followSpeed * Math.Sign(dist.X);
        }

        public void ShootPlayer()
        {
            Player player = ((PlayScene)Game.CurrentScene).Player;
            Vector2 dist = player.Position - Position;

            Shoot(dist.Normalized());
        }

        public void ChangeSpriteColor(Vector3 newCol)
        {
            sprite.SetAdditiveTint(new Vector4(newCol, 0.0f));
        }

        public override void Update()
        {
            fsm.Update();
        }

        public override void OnDie()
        {
            IsActive = false;
        }
    }
}
