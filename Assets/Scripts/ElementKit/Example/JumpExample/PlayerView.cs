using UnityEngine;

namespace JumpExample
{
	public class PlayerView : Codeplay.ElementBehavior<Player>
	{
		protected override void OnInit()
		{
			_transform = transform;
			_moveModel = Context.GetModel<PlayerMoveModel>();
		}

		private void Update()
		{
			_transform.position = _moveModel.Position.Value;
		}

		private PlayerMoveModel _moveModel;
		private Transform _transform;
	}
}
