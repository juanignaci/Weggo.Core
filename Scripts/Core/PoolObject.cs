using UnityEngine;

namespace Weggo.Core
{
    public class PoolObject : Entity
    {
        public System.Action OnReset;

        public virtual void Reset() { if (OnReset != null) OnReset(); }
    } 
}
