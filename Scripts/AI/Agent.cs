using UnityEngine;
using UnityEngine.AI;
using Weggo.Core3D;

namespace Weggo.AI
{
    public class Agent : Character
    {
        new public AgentBehaviour behaviour;
        [SerializeField]
        int ticksPerSecond = 15;
        float _tickTime { get { return 1.0f / ticksPerSecond; } }
        float lastTick = -1;

        protected override void Reset()
        {
            base.Reset();
            base.behaviour = null;
            behaviour = GetComponent<AgentBehaviour>();
        }

        protected override void Update()
        {
            movement.Update();
            rBody.velocity = movement.GetVelocity();

            behaviour.UpdateBehaviour();

            if (Time.realtimeSinceStartup < lastTick + _tickTime)
                return;

            OnTick();

            lastTick = Time.realtimeSinceStartup;
        }

        protected virtual void OnTick()
        {
            behaviour.OnBehaviourTick();
        }

        public void Move(NavMeshPath path)
        {
            movement.Accelerate((path.corners[1] - path.corners[0]).normalized, Mathf.Min((path.corners[path.corners.Length - 1] - transform.position).magnitude, 1));
        }
    }
}