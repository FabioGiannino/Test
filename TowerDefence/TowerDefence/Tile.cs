using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefence
{
    class Tile : GameObject
    {
        public Tile() : base("tile")
        {
            RigidBody = new RigidBody(this);
            RigidBody.Type = RigidBodyType.Tile;
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            IsActive = true;

            DrawMngr.AddItem(this);
        }
    }
}
