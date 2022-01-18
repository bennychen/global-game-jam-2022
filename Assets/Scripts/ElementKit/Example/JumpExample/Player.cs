using Codeplay;
using UnityEngine;

namespace JumpExample
{
	public class PlayerMoveModel : ElementModel
	{
		public Vector3ChangeCommand Position { get; private set; }

		public PlayerMoveModel()
		{
			Position = new Vector3ChangeCommand(this, "Position");
		}
	}

	public class PlayerInputModel : ElementModel
	{
		public FloatChangeCommand Input { get; private set; }
		public Command Jump { get; private set; }

		public PlayerInputModel()
		{
			Input = new FloatChangeCommand(this, "Input");
			Jump = new Command(this, "Jump");
		}
	}

	public class Player : SceneElement<Player>
	{
		protected override void OnInit(object data)
		{
			Models.Add(new PlayerInputModel());
			Models.Add(new PlayerMoveModel());

			Controllers.Add(_controllerNode.GetComponent<PlayerMove>());
			Controllers.Add(_controllerNode.GetComponent<PlayerKeyboardControl>());

			Views.Add(_viewsNode.gameObject.GetComponent<PlayerView>());
		}

		[SerializeField]
		private GameObject _controllerNode;
	}
}
