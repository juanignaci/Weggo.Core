﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Weggo.Core;
using System.Linq;

namespace Weggo.AI
{
    public enum Actions
    {
        NoAction,
        Action1,
        Action2
    }

    public class AgentBehaviour : CharacterBehaviour
    {
        public Agent agent;
        public bool isMoving = false;
        [SerializeField]
        protected Actions[] possibleActions;
        public List<AgentAction> actionQueue = new List<AgentAction>();
        NavMeshPath path;

        private void Awake()
        {
            path = new NavMeshPath();
        }

        public override void OnInit()
        {
            agent = (Agent)c;
            RegisterPossibleActions();

            if (possibleActions == default || possibleActions.Length == 0)
                Debug.LogError("There are not possible actions for this agent!");
        }

        protected virtual void RegisterPossibleActions() { }

        public override void UpdateBehaviour()
        {
            if (isMoving && path.corners.Length > 0)
                agent.Move(path);
            else if (path == null || path.corners.Length == 0) {
                Debug.LogWarning("Path is invalid yet isMoving is set to true!");
                isMoving = false;
            }

            if (actionQueue.Count == 0)
                return;// actionQueue.Add(AgentAction.Create(this, possibleActions[0]));
            
            if (actionQueue[0].isFinished)
                actionQueue.RemoveAt(0);
            else
                actionQueue[0].Update();
        }

        public virtual void OnBehaviourTick()
        {
            if (actionQueue.Count == 0)
                return;

            if (actionQueue[0].isFinished)
                actionQueue.RemoveAt(0);
            else
                actionQueue[0].Tick();
        }

        public void SetPath(NavMeshPath path) { this.path = path; }

        public NavMeshPath SetPathTo(Vector3 position)
        {
            if (NavMesh.CalculatePath(c.transform.position, position, NavMesh.AllAreas, path))
                return path;
            else
            {
                Debug.LogError("Couldn't find valid path to target position! returning null...");
                return null;
            }
        }

        public bool CanDo(Actions action)
        {
            return possibleActions.Contains(action);
        }
    }
}