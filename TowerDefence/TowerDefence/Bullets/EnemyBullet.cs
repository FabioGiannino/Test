using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace TowerDefence
{
    class EnemyBullet : Bullet
    {
        public EnemyBullet(int textOffsetX = 0, int textOffsetY = 0, int spriteW = 0, int spriteH = 0) : base("playerBullet", textOffsetX, textOffsetY, spriteW, spriteH)
        {
            Type = BulletType.EnemyBullet;
            RigidBody.Collider = ColliderFactory.CreateCircleFor(this);
            RigidBody.Type = RigidBodyType.EnemyBullet;
            RigidBody.AddCollisionType((uint)RigidBodyType.Player | (uint)RigidBodyType.PlayerBullet | (uint)RigidBodyType.Tile);

            maxSpeed = 8.0f;
            RigidBody.Velocity.X = maxSpeed;

            sprite.SetAdditiveTint(200.0f, 0.0f, 0.0f, 0.0f);
        }

        public override void OnCollide(Collision collisionInfo)
        {
            if (collisionInfo.Collider is Player)
            {
                ((Player)collisionInfo.Collider).AddDamage(damage);
            }

            BulletMngr.RestoreBullet(this);
        }

    }
}
