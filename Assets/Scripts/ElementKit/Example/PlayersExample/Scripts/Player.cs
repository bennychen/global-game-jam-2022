using UnityEngine;

namespace PlayersExample
{
	public class PlayerRunState : Codeplay.State<Player> { }
	public class PlayerDeadState : Codeplay.State<Player> { }

	public class Player : Codeplay.SceneElement<Player>
	{
		public Vehicle Vehicle { get { return _vehicle; } }

		protected override void OnInit(object data)
		{
			AddState(new PlayerRunState());
			AddState(new PlayerDeadState());
		}

		[SerializeField]
		private Vehicle _vehicle;
	}
}