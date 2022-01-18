using UnityEngine;
using Codeplay;

namespace JumpExample
{
	public class PlayerMove : ElementBehavior<Player>
	{
		protected override void OnInit()
		{
			_moveModel = _context.GetModel<PlayerMoveModel>();
			_inputModel = _context.GetModel<PlayerInputModel>();
			_inputModel.Jump.BindAction(Jump);

			_controller = GetComponent<CharacterController>();
			_initJumpVelocity = -_gravity * Mathf.Sqrt(2 * _jumpHeight / -_gravity);

			_transform = transform;
			_moveModel.Position.Exec(_transform.position);
		}

		private void Jump()
		{
			_velocity.y = _initJumpVelocity;
		}

		private void FixedUpdate()
		{
			_transform.position = _moveModel.Position.Value;
			_initJumpVelocity = -_gravity * Mathf.Sqrt(2 * _jumpHeight / -_gravity);
			Vector2 velocity = _velocity;
			if (_controller.isGrounded)
			{
				if (velocity.y < 0)
				{
					velocity.y = -0.5f;
				}
				velocity.x = _inputModel.Input.Value * _horizontalSpeed;
			}
			else
			{
				velocity.y += _gravity * Time.fixedDeltaTime;
			}

			_controller.Move(velocity * Time.fixedDeltaTime);
			_velocity = velocity;
			_moveModel.Position.Exec(_transform.position);
		}

		private PlayerMoveModel _moveModel;
		private PlayerInputModel _inputModel;
		private CharacterController _controller;
		private bool _needJump;

		[Range(-100, 0)]
		[SerializeField]
		private float _gravity = -10;

		[SerializeField]
		private float _jumpHeight = 5;

		[SerializeField]
		private float _horizontalSpeed = 10;

		private float _initJumpVelocity;
		private Vector3 _velocity;
		private Transform _transform;
	}
}
