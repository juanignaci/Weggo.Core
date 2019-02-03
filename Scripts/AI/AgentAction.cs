namespace Weggo.AI
{
    public abstract class AgentAction
    {
        private static T Create<T>(AgentBehaviour agent) where T : AgentAction, new() { T action = new T(); action.Init(agent); return action; }

        AgentBehaviour agent;
        public bool isFinished = false;

        public virtual void Update() { }
        public virtual void Tick() { }

        public void Init(AgentBehaviour agent) { this.agent = agent; isFinished = false; }

        protected void Do<T>() where T : AgentAction, new()
        {
            agent.actionQueue.Clear();
            agent.actionQueue.Add(Create<T>(agent));
        }

        protected void DoNext<T>() where T : AgentAction, new() { agent.actionQueue.Insert(1, Create<T>(agent)); }
        protected void DoFirst<T>() where T : AgentAction, new() { agent.actionQueue.Add(Create<T>(agent)); }
        protected void DoNow<T>() where T : AgentAction, new() { agent.actionQueue.Add(Create<T>(agent)); }

    } 
}