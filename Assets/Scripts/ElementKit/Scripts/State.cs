namespace Codeplay
{
	public abstract class State<T>
	{
		internal void Init(StateMachine<T> stateMachine, T context)
		{
			_stateMachine = stateMachine;
			_context = context;
			OnInit();
		}

		public virtual void OnInit() { }
		public virtual void OnEnter() { }
		public virtual void OnUpdate(float deltaTime) { }
		public virtual void OnExit() { }

		protected StateMachine<T> _stateMachine;
		protected T _context;
	}
}