using UnityEngine;
using Codeplay;

namespace JumpExample
{
	public class PlayerKeyboardControl : ElementBehavior<Player>
	{
		protected override void OnInit()
		{
			_model = _context.GetModel<PlayerInputModel>();
		}

		private void FixedUpdate()
		{
			float input = 0;
			if (Input.GetKey(KeyCode.LeftArrow))
			{
				input = -1;
			}
			else if (Input.GetKey(KeyCode.RightArrow))
			{
				input = 1;
			}
			_model.Input.Exec(input);

			if (Input.GetKeyDown(KeyCode.Space))
			{
				_model.Jump.Exec();
			}
		}

		private PlayerInputModel _model;
	}
}
