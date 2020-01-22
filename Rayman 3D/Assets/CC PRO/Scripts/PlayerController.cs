using UnityEngine;

namespace PlayerController
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        private bool _isMovingForward = false, _isMovingBack = false, _isMovingLeft = false, _isMovingRight = false, _isJumping = false;
        private const int _minSliderValue = 0, _maxSliderValue = 100;
        private readonly float _error = 0.01f, _maxMovementSpeed = 30, _maxJumpHeight = 500;
        private Transform _cameraTransform;
        private Rigidbody _playerRigidbody;
        private float _movementSpeed, _jumpHeight;

        [Range(_minSliderValue, _maxSliderValue)]
        public int MovementSpeed = 50;
        [Range(_minSliderValue, _maxSliderValue)]
        public int JumpHeight = 50;
        // Control keys
        public KeyCode ForwardKey = KeyCode.W, LeftKey = KeyCode.A, BackKey = KeyCode.S, RightKey = KeyCode.D, JumpKey = KeyCode.Space;
        // Physics or kinematic
        public bool IsKinematic = false;
        // States of movement

        void Start()
        {
            // Main camera object with tag "MainCamera"
            _cameraTransform = Camera.main.transform;
            // Definition of player Rigidbody component for physical control
            _playerRigidbody = GetComponent<Rigidbody>();
        }
        
        // Fixed framerate update for Rigidbody component of player
        void FixedUpdate()
        {
            if (!IsKinematic && Mathf.Abs(_playerRigidbody.velocity.y) <= _error)
            {
                _movementSpeed = _maxMovementSpeed * MovementSpeed / _maxSliderValue;

                if (_isJumping)
                    Jump();

                if (_isMovingForward)
                    MoveStraight();
                else
                if (_isMovingBack)
                    MoveStraight(-1);

                if (_isMovingRight)
                    MoveToSide();
                else
                if (_isMovingLeft)
                    MoveToSide(-1);
            }
        }

        // Check of movement direction according to pressed keys
        void Update()
        {
            if (Input.GetKeyDown(ForwardKey))
                _isMovingForward = true;

            if (Input.GetKeyUp(ForwardKey))
                _isMovingForward = false;

            if (Input.GetKeyDown(BackKey))
                _isMovingBack = true;

            if (Input.GetKeyUp(BackKey))
                _isMovingBack = false;

            if (Input.GetKeyDown(LeftKey))
                _isMovingLeft = true;

            if (Input.GetKeyUp(LeftKey))
                _isMovingLeft = false;

            if (Input.GetKeyDown(RightKey))
                _isMovingRight = true;

            if (Input.GetKeyUp(RightKey))
                _isMovingRight = false;

            if (Input.GetKeyDown(JumpKey))
                _isJumping = true;

            if (Input.GetKeyUp(JumpKey))
                _isJumping = false;

            UpdateKinematics();

            if (IsKinematic)
            {
                _movementSpeed = _maxMovementSpeed * MovementSpeed / _maxSliderValue;

                if (_isMovingForward)
                    MoveStraight();
                else
                if (_isMovingBack)
                    MoveStraight(-1);

                if (_isMovingRight)
                    MoveToSide();
                else
                if (_isMovingLeft)
                    MoveToSide(-1);
            }
        }

        // Back and Forward movement according to direction parameter - sign: {1, -1}
        void MoveStraight(int sign = 1)
        {
            var forwardDirection = _cameraTransform.forward.normalized;
            forwardDirection.y = 0;

            if (IsKinematic)
                transform.position += forwardDirection * Time.fixedDeltaTime * _movementSpeed * sign;
            else
                _playerRigidbody.MovePosition(_playerRigidbody.position + forwardDirection * Time.fixedDeltaTime * _movementSpeed * sign);
        }

        // Left and Right movement accoring to direction parameter - sign: {1, -1}
        void MoveToSide(int sign = 1)
        {
            var rightDirection = _cameraTransform.right.normalized;
            rightDirection.y = 0;

            if (IsKinematic)
                transform.position += rightDirection * Time.fixedDeltaTime * _movementSpeed * sign;
            else
                _playerRigidbody.MovePosition(_playerRigidbody.position + rightDirection * Time.fixedDeltaTime * _movementSpeed * sign);
        }
        
        // Jumping if physics is enabled
        void Jump()
        {
            _jumpHeight = _maxJumpHeight * JumpHeight / _maxSliderValue;

            _playerRigidbody.AddForce(Vector3.up * _jumpHeight);
            _isJumping = false;
        }
        
        // Set of the player kinematics according to his settings
        void UpdateKinematics()
        {
            if (IsKinematic)
                _playerRigidbody.isKinematic = true;
            else
                _playerRigidbody.isKinematic = false;
        }
    }
}