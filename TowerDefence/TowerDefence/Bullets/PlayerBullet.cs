using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace TowerDefence
{
    class PlayerBullet : Bullet
    {
        
        public PlayerBullet():base("playerBullet")
        {
            RigidBody.Type = RigidBodyType.PlayerBullet;
            RigidBody.Collider = ColliderFactory.CreateCircleFor(this);
            RigidBody.AddCollisionType((uint)RigidBodyType.Enemy | (uint)RigidBodyType.EnemyBullet | (uint)RigidBodyType.Tile);

            maxSpeed = 8f;
            RigidBody.Velocity.X = maxSpeed;

            Type = BulletType.PlayerBullet;
        }

        public override void OnCollide(Collision collisionInfo)
        {
            if(collisionInfo.Collider is Enemy)
            {
                Enemy enemy = ((Enemy)collisionInfo.Collider);
                enemy.AddDamage(damage);

                if (!enemy.IsAlive)
                {
                    ((PlayScene)Game.CurrentScene).Player.AddScore(enemy.Points);
                }
            }

            BulletMngr.RestoreBullet(this);
        }
    }
}
