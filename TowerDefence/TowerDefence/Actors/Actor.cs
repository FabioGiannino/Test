using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefence
{
    enum WeaponType
    {
        Default,
        TripleShoot
    }

    abstract class Actor : GameObject
    {
        // Variables
        protected Vector2 shootOffset;
        protected BulletType bulletType;

        protected int energy;
        protected int maxEnergy;

        //protected AudioSource soundEmitter;
        //protected AudioClip shootSound;

        protected WeaponType weaponType;
        protected float tripleShootAngle = 0.0f;
        protected Vector2 shootVel;

        public bool IsAlive { get { return energy > 0; } }
        public virtual int Energy { get => energy; set { energy = MathHelper.Clamp(value, 0, maxEnergy); } }

        public bool IsGrounded { get { return RigidBody.Velocity.Y == 0; } }
        public bool IsJumping { get { return RigidBody.Velocity.Y < 0; } }
        public bool IsFalling { get { return RigidBody.Velocity.Y > 0; } }

        public Actor(string texturePath, int textOffsetX = 0, int textOffsetY = 0, float spriteWidth = 0, float spriteHeight = 0) : base(texturePath, textOffsetX:textOffsetX, textOffsetY:textOffsetY, spriteWidth: spriteWidth, spriteHeight: spriteHeight)
        {
            RigidBody = new RigidBody(this);
            maxEnergy = 100;
        }

        public void ChangeWeapon(WeaponType weapon)
        {
            weaponType = weapon;
        }

        protected virtual void Shoot(Vector2 direction)
        {
            Bullet b;

            switch (weaponType)
            {
                case WeaponType.Default:
                    b = BulletMngr.GetBullet(bulletType);

                    if (b != null)
                    {
                        b.Shoot(sprite.position + shootOffset, direction.Normalized());
                    }
                    break;
            }
        }

        public virtual void AddDamage(int dmg)
        {
            Energy -= dmg;

            if (Energy <= 0)
            {
                OnDie();
            }
        }

        public virtual void OnDie()
        {
            
        }
             

        public virtual void Reset()
        {
            Energy = maxEnergy;
        }

        public override void Update()
        {

        }
    }
}
