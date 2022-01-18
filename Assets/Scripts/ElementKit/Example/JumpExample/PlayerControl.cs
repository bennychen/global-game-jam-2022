using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _initJumpVelocity = -_gravity * Mathf.Sqrt(2 * _jumpHeight / -_gravity);
    }

    private void FixedUpdate()
    {
        _initJumpVelocity = -_gravity * Mathf.Sqrt(2 * _jumpHeight / -_gravity);
        if (_controller.isGrounded)
        {
            _currentVelocity.y = -0.5f;

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _currentVelocity.x = _horizontalSpeed * -1;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                _currentVelocity.x = _horizontalSpeed;
            }
            else
            {
                _currentVelocity.x = 0;
            }


            if (Input.GetKeyDown(KeyCode.Space))
            {
                _currentVelocity.y = _initJumpVelocity;
            }
        }
        else
        {
            _currentVelocity.y += _gravity * Time.fixedDeltaTime;
        }

        _controller.Move(_currentVelocity * Time.fixedDeltaTime);
    }

    private CharacterController _controller;

    [Range(-100, 0)]
    [SerializeField]
    private float _gravity = -10;

    [SerializeField]
    private float _jumpHeight = 5;

    [SerializeField]
    private float _horizontalSpeed = 10;

    private float _initJumpVelocity;
    private Vector2 _currentVelocity;
}
