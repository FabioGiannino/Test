using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefence
{
    abstract class State
    {
        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public abstract void Update();

        protected StateMachine fsm;

        public void SetStateMachine(StateMachine sm)
        {
            fsm = sm;
        }
    }
}
