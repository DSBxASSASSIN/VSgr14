using UnityEngine;
using States;

namespace CameraController
{
    [RequireComponent(typeof(Camera))]
    /// <summary>
    /// Class that execute all functions of camera
    /// </summary>
    [ExecuteInEditMode]
    public class CameraController : MonoBehaviour
    {
        /// <summary>
        /// Enums for states definition of smoothing, control and mouse movement
        /// </summary>

        #region Private Fields       
        // Current and previous states of control, smoothing (of control) and mouse/touchpad
        #region States
        public ControlState _currentControlState = ControlState.Rest, _previousControlState = ControlState.Rest;
        private MouseState _currentMouseState = MouseState.Rest;
        public SmoothingState _currentSmoothingState = SmoothingState.Rest, _previousSmoothingState = SmoothingState.Rest;
        #endregion

        // Private dublicates of public fields (states)
        #region Camera
        // Clone of camera for player rotation
        private GameObject _cameraClone, _target;
        private Transform _camera;
        private readonly string _targetName = "_cameraTarget", _cameraCloneName = "_cameraClone";
        private float _distance, _rotation, _height;
        #endregion

        // Private dublicates of public fields (target and camera)
        #region Mouse

        #region State
        private float _currentDistance, _currentScroll;
        private Vector3 _currentMousePosition, _previousDragPosition = Vector3.zero;
        #endregion

        #region Speed
        private float _distancingSpeed, _rotationSpeed, _liftingSpeed;
        #endregion

        // Minimum values of dragging to start moving, maximum values of dragging not to normalize
        #region Thresholds
        private readonly int _minThreshold = 0;
        private readonly float _maxDistancingThreshold = 0.03f, _maxRotationThreshold = 180;
        private float _maxLiftingThreshold;
        #endregion

        #region Smoothness
        private float _distancingSmoothness, _rotationSmoothness, _liftingSmoothness;
        #endregion
        #endregion

        // Private dublicates of public fields (mouse)
        #region Following
        private float _followingSmoothness;
        #endregion

        // Private dublicates of public fields (border)
        #region Border
        private float _borderWidthPercent, _borderHeightPercent, _borderRotationSpeed, _borderLiftingSpeed,
                      _borderRotationSmoothness, _borderLiftingSmoothness;
        #endregion
        // Valid limits of different values
        #region Limits
        private float _minDistance, _maxDistance, _minHeight, _maxHeight;
        private const float _minSliderValue = 0, _maxSliderValue = 100;
        private readonly float _specialMinSliderValue = 1, _specialMaxSliderValue = 10,

                            _minLowerDistance = 0, _minUpperDistance = 20, _maxLowerDistance = 30, _maxUpperDistance = 50,
                            _maxRotation = 359,
                            _minLowerHeight = -10, _minUpperHeight = 30, _maxLowerHeight = 50, _maxUpperHeight = 90,

                            _maxScrollingSpeed = 800, _maxDraggingSpeed = 10,
                            _minBorderRotationSpeed = 1.8f, _maxBorderRotationSpeed = 4, _minBorderLiftingSpeed = 0.3f, _maxBorderLiftingSpeed = 2,
                            _maxBorderSizePercent = 15,
                            //_minScrollingNormalization = 0.03f, _maxScrollingNormalization = 0.005f,
                            //_minDraggingNormalization = 180, _maxDraggingNormalization = 10,

                            _minSmoothness = 10, _maxSmoothness = 2;
                            //_minBorderSmoothness = 10, _maxBorderSmoothness = 2;
        #endregion
        // Valid errors of control and camera following
        #region Errors
        private readonly float _distancingError = 0.05f, _rotationError = 1.0f, _liftingError = 1.0f,
                            _distancingSmoothnessError = 0.1f, _rotationSmoothnessError = 1.0f, _liftingSmoothnessError = 1.0f,
                            _positionFollowingError = 0.1f, _distanceFollowingError = 0.1f, _rotationFollowingError = 1.0f, _heightFollowingError = 1.0f;
        #endregion
        // Enable physics or Rigidbody is Kinematic
        private Rigidbody _playerRigidbody;
        #endregion

        #region Public Fields
        // Settings of target object (player) and camera position
        #region Position
        // Target object
        public Transform Player;
        // Center of player to follow
        public Vector3 PositionOffset = new Vector3(0, 1.5f, 0);
        // 3 components of camera position relative to the player
        public int Distance, Rotation, Height;
        #endregion
        
        // Settings of camera control by scrolling and dragging
        #region Control
        // Key to rotate and lift camera
        public KeyCode DragKey = KeyCode.Mouse1;
        // Mode to lock simultaneous rotation and lifting 
        public bool SeparateDrag = false;
        // Player rotates simultaneously with the camera
        public bool PlayerRotation = false;
        // Speed of camera movement in 3 directions
        public int DistancingSpeed, RotationSpeed, LiftingSpeed;
        // Smoothness of camera movement in 3 directions
        public int DistancingSmoothness, RotationSmoothness, LiftingSmoothness;
        // Smoothness of camera following (behind the player)
        public int FollowingSmoothness;
        #endregion
        
        // Screen border control speed and size
        #region Border
        public int BorderWidth, BorderHeight, BorderRotationSpeed, BorderLiftingSpeed,
                   BorderRotationSmoothness, BorderLiftingSmoothness;
        #endregion

        #region Limits
        public int MinDistance, MaxDistance, MinHeight, MaxHeight;
        #endregion
        #endregion

        #region Methods
        #region Main
        // Definition of major variables once
        void Start()
        {
            _camera = Camera.main.transform;
            _target = GameObject.Find(_targetName);
            _cameraClone = GameObject.Find(_cameraCloneName);

            if (_cameraClone == null)
                _cameraClone = new GameObject(_cameraCloneName);

            if (_target == null)
                _target = new GameObject(_targetName);

            if (Player == null)
                Player = GameObject.FindGameObjectWithTag("Player").transform;

            _playerRigidbody = Player.gameObject.GetComponent<Rigidbody>();
        }
        
        // Updates that called every frame and keep all variables in actual state
        // Fixed framerate update for Rigidbody component of player
        void FixedUpdate()
        {
            if (PlayerRotation)
                RotatePlayer(_playerRigidbody.isKinematic);
        }
        
        // Сamera movement according to pressed key
        void Update()
        {
            ResetCameraState();
            UpdateTarget();
            UpdateCurrentMouseState();
            UpdateCurrentDistance();
            UpdateFloatFields();

            if (PlayerRotation)
                RotatePlayer(_playerRigidbody.isKinematic);

            switch ((int)_currentMouseState)
            {
                case 1:
                    if (IsValuePositive(DistancingSpeed))
                        Distance_();
                    break;
                case 2:
                    RotateOrLift();
                    break;
                default:
                    RotateOrLift();
                    break;
            }
        }
        
        // Camera following according to player position in previous frame
        void LateUpdate()
        {
            if (_currentControlState == ControlState.Rest)
                Smooth();

            FollowTarget();

            UpdateIntFields();
            UpdatePreviousDragPosition();
            UpdatePreviousCameraState();
        }
        #endregion
        
        // Movement of camera in 3 directions: back-forward, up-down, around
        #region Control
        // Camera movement back and forward relative to the player
        void Distance_()
        {
            float deltaScroll = -_currentScroll * Time.deltaTime;

            if (Mathf.Abs(deltaScroll) > _minThreshold)
            {
                if (_currentSmoothingState != SmoothingState.Rest)
                    _distance = _currentDistance;

                float previousDistance = _distance;
                _distance += NormalizeValue(deltaScroll, _maxDistancingThreshold) * _distancingSpeed;

                CorrectValue(ref _distance, _minDistance, _maxDistance);

                if (Mathf.Abs(_distance - previousDistance) > _distancingError)
                {
                    _currentControlState = ControlState.Distancing;

                    if (_currentSmoothingState != SmoothingState.Rest)
                        InterruptSmoothing();

                    _camera.position -= _camera.rotation * Vector3.forward * Mathf.Lerp(0, _distance - _currentDistance, _distancingSmoothness * Time.deltaTime);

                    UpdateCurrentDistance();
                }
            }
        }
        
        // Switch between rotation and lifting
        void RotateOrLift() // TODO ??
        {

            float horizontalDrag = (_currentMousePosition.x - (_previousDragPosition == Vector3.zero ? _currentMousePosition.x : _previousDragPosition.x)) * Time.deltaTime,
                  verticalDrag = (_currentMousePosition.y - (_previousDragPosition == Vector3.zero ? _currentMousePosition.y : _previousDragPosition.y)) * Time.deltaTime;

            bool isBorderRotation = false, isBorderLifting = false;

            if (_currentMouseState == MouseState.Dragging)
            {
                if (IsValuePositive(BorderRotationSpeed))
                {
                    float borderWidth = _borderWidthPercent * Screen.width / 100;

                    if (Mathf.Abs(_currentMousePosition.x - Screen.width) < borderWidth)
                    {
                        isBorderRotation = true;
                        horizontalDrag = 1;
                    }
                    else if (Mathf.Abs(_currentMousePosition.x) < borderWidth)
                    {
                        isBorderRotation = true;
                        horizontalDrag = -1;
                    }
                }

                if (IsValuePositive(BorderLiftingSpeed))
                {
                    float borderHeight = _borderHeightPercent * Screen.height / 100;

                    if (Mathf.Abs(_currentMousePosition.y - Screen.height) < borderHeight)
                    {
                        isBorderLifting = true;
                        verticalDrag = 1;
                    }
                    else if (Mathf.Abs(_currentMousePosition.y) < borderHeight)
                    {
                        isBorderLifting = true;
                        verticalDrag = -1;
                    }
                }
            }

            if (isBorderRotation)
                Rotate(horizontalDrag * _borderRotationSpeed, isBorderRotation);
            else
            if (Mathf.Abs(horizontalDrag) > _minThreshold &&
                ((Mathf.Abs(ConvertByAspectRatio(horizontalDrag)) > Mathf.Abs(verticalDrag) || !IsValuePositive(LiftingSpeed)) && SeparateDrag || !SeparateDrag))
                Rotate(horizontalDrag * _rotationSpeed);

            if (isBorderLifting)
                Lift(verticalDrag * _borderLiftingSpeed, isBorderLifting);
            else
            if (Mathf.Abs(verticalDrag) > _minThreshold && (_currentControlState == ControlState.Rest && SeparateDrag || !SeparateDrag))
                Lift(verticalDrag * _liftingSpeed);
        }
        
        // Camera movement up and down relative to the player
        void Lift(float verticalDrag, bool isBorder = false) // TODO?
        {
            if (_currentSmoothingState != SmoothingState.Rest)
                _height = _camera.eulerAngles.x;

            float previousHeight = _height;

            _height -= isBorder ? verticalDrag : NormalizeValue(verticalDrag, _maxLiftingThreshold);

            CorrectValue(ref _height, _minHeight, _maxHeight);

            if (Mathf.Abs(_height - previousHeight) > _liftingError && (_previousControlState != ControlState.Rotation && SeparateDrag || !SeparateDrag || isBorder))
            {
                _currentControlState = isBorder ? ControlState.BorderLifting : ControlState.Lifting;

                if (_currentSmoothingState != SmoothingState.Rest)
                    InterruptSmoothing();

                _camera.RotateAround(_target.transform.position, _camera.right, Mathf.LerpAngle(0, _height - _camera.eulerAngles.x, Time.deltaTime * _liftingSmoothness));
            }
        }
        
        // Camera rotation around the player
        void Rotate(float horizontalDrag, bool isBorder = false) // TODO ?
        {
            if (_currentSmoothingState != SmoothingState.Rest)
                _rotation = _camera.eulerAngles.y;

            float previousRotation = _rotation;

            _rotation += isBorder ? horizontalDrag : NormalizeValue(horizontalDrag, _maxRotationThreshold);
            _rotation = _rotation % _maxRotation + (_rotation < 0 ? _maxRotation : 0);

            if ((Mathf.Abs(_rotation - previousRotation) > _rotationError) && (_previousControlState != ControlState.Lifting && SeparateDrag || !SeparateDrag))
            {
                _currentControlState = isBorder ? ControlState.BorderRotation : ControlState.Rotation;              

                if (_currentSmoothingState != SmoothingState.Rest)
                    InterruptSmoothing();

                _camera.RotateAround(_target.transform.position, Vector3.up, Mathf.LerpAngle(0, _rotation - _camera.eulerAngles.y, Time.deltaTime * (isBorder ? _borderRotationSmoothness : _rotationSmoothness)));
            }
        }
        
        // Player rotation with the camera
        void RotatePlayer(bool isKinematic)
        {
            _cameraClone.transform.SetPositionAndRotation(_camera.position, Quaternion.identity);
            _cameraClone.transform.LookAt(Player.transform);

            float playerAngleY, cameraAngleY = _cameraClone.transform.eulerAngles.y;

            if (cameraAngleY >= 0 && cameraAngleY < 180)
                playerAngleY = cameraAngleY + 180;
            else
                playerAngleY = cameraAngleY - 180;

            if (isKinematic)
                Player.rotation = Quaternion.Euler(0, playerAngleY, 0);
            else
                _playerRigidbody.MoveRotation(Quaternion.Euler(0, playerAngleY, 0));
        }

        #region Smoothing
        void Smooth()
        {
            switch (_previousControlState)
            {
                case ControlState.Distancing:
                    SmoothDistancing();
                    break;
                case ControlState.Rotation:
                    SmoothRotation();
                    break;
                case ControlState.Lifting:
                    SmoothLifting();
                    break;
                case ControlState.BorderRotation:
                    SmoothBorderRotation();
                    break;
                case ControlState.BorderLifting:
                    SmoothBorderLifting();
                    break;
                case ControlState.Rest:
                    switch (_previousSmoothingState)
                    {
                        case SmoothingState.Distancing:
                            SmoothDistancing();
                            break;
                        case SmoothingState.Rotation:
                            SmoothRotation();
                            break;
                        case SmoothingState.Lifting:
                            SmoothLifting();
                            break;
                        case SmoothingState.BorderRotation:
                            SmoothBorderRotation();
                            break;
                        case SmoothingState.BorderLifting:
                            SmoothBorderLifting();
                            break;
                    }
                    break;
            }
        }

        void SmoothDistancing()
        {
            if (Mathf.Abs(_distance - _currentDistance) > _distancingSmoothnessError)
            {
                _currentSmoothingState = SmoothingState.Distancing;
                _camera.position -= _camera.rotation * Vector3.forward * Mathf.Lerp(0, _distance - _currentDistance, _distancingSmoothness * Time.deltaTime);
                UpdateCurrentDistance();
            }
        }

        void SmoothRotation()
        {
            if (Mathf.Abs(_rotation - _camera.eulerAngles.y) > _rotationSmoothnessError)
            {
                _currentSmoothingState = SmoothingState.Rotation;
                _camera.RotateAround(_target.transform.position, Vector3.up, Mathf.LerpAngle(0, _rotation - _camera.eulerAngles.y, Time.deltaTime * _rotationSmoothness));
            }
        }

        void SmoothLifting()
        {
            if (Mathf.Abs(_height - _camera.eulerAngles.x) > _liftingSmoothnessError)
            {
                _currentSmoothingState = SmoothingState.Lifting;
                _camera.RotateAround(_target.transform.position, _camera.right, Mathf.LerpAngle(0, _height - _camera.eulerAngles.x, Time.deltaTime * _liftingSmoothness));
            }
        }

        void SmoothBorderRotation()
        {
            if (Mathf.Abs(_rotation - _camera.eulerAngles.y) > _rotationSmoothnessError)
            {
                _currentSmoothingState = SmoothingState.BorderRotation;
                _camera.RotateAround(_target.transform.position, Vector3.up, Mathf.LerpAngle(0, _rotation - _camera.eulerAngles.y, Time.deltaTime * _borderRotationSmoothness));
            }
        }

        void SmoothBorderLifting()
        {
            if (Mathf.Abs(_height - _camera.eulerAngles.x) > _liftingSmoothnessError)
            {
                _currentSmoothingState = SmoothingState.BorderLifting;
                _camera.RotateAround(_target.transform.position, _camera.right, Mathf.LerpAngle(0, _height - _camera.eulerAngles.x, Time.deltaTime * _borderLiftingSmoothness));
            }
        }

        void InterruptSmoothing()
        {
            _currentSmoothingState = SmoothingState.Rest;

            switch (_currentControlState)
            {
                case ControlState.Distancing:
                    _rotation = _camera.eulerAngles.y;
                    _height = _camera.eulerAngles.x;
                    break;
                case ControlState.Rotation:
                    _distance = _currentDistance;
                    _height = _camera.eulerAngles.x;
                    break;
                case ControlState.BorderRotation:
                    _distance = _currentDistance;
                    _rotation = _camera.eulerAngles.y;
                    break;
                case ControlState.Lifting:
                    _distance = _currentDistance;
                    _rotation = _camera.eulerAngles.y;
                    break;
                case ControlState.BorderLifting:
                    _distance = _currentDistance;
                    _rotation = _camera.eulerAngles.y;
                    break;
            }
        }
        #endregion

        #region Check
        bool IsValuePositive(int value)
        {
            return value > _minSliderValue;
        }
        #endregion
        #endregion
        // Camera following behind the player
        void FollowTarget()
        {
            float deltaTime = Time.deltaTime;

            Vector3 newPosition = _target.transform.position - _camera.rotation * Vector3.forward * _currentDistance;
            if (Vector3.Distance(_camera.position, newPosition) > _positionFollowingError)
                _camera.position = Vector3.Lerp(_camera.position, newPosition, deltaTime * _followingSmoothness);

            if (Mathf.Abs(_distance - _currentDistance) > _distanceFollowingError && _currentControlState != ControlState.Distancing && _currentSmoothingState != SmoothingState.Distancing)
                _camera.position -= _camera.rotation * Vector3.forward * Mathf.Lerp(0, _distance - _currentDistance, deltaTime * _followingSmoothness);

            if (Mathf.Abs(_rotation - _camera.eulerAngles.y) > _rotationFollowingError && _currentControlState != ControlState.Rotation && _currentSmoothingState != SmoothingState.Rotation &&
                                                                                          _currentControlState != ControlState.BorderRotation && _currentSmoothingState != SmoothingState.BorderRotation)
                _camera.RotateAround(_target.transform.position, Vector3.up, Mathf.LerpAngle(0, _rotation - _camera.eulerAngles.y, deltaTime * _followingSmoothness));

            if (Mathf.Abs(_height - _camera.eulerAngles.x) > _heightFollowingError && _currentControlState != ControlState.Lifting && _currentSmoothingState != SmoothingState.Lifting &&
                                                                                      _currentControlState != ControlState.BorderLifting && _currentSmoothingState != SmoothingState.BorderLifting)
                _camera.RotateAround(_target.transform.position, _camera.right, Mathf.LerpAngle(0, _height - _camera.eulerAngles.x, deltaTime * _followingSmoothness));
        }

        #region Updates
        // Update of variables according to target actual position and rotation
        void UpdateTarget()
        {
            _target.transform.SetPositionAndRotation(Player.position + PositionOffset, Player.rotation);
        }

        #region Camera
        // Stop of camera movement
        void ResetCameraState()
        {
            _currentControlState = ControlState.Rest;
            _currentSmoothingState = SmoothingState.Rest;
        }
        
        // Update of variables according to current distance between camera and player
        void UpdateCurrentDistance()
        {
            _currentDistance = Vector3.Distance(_camera.position, _target.transform.position);
        }
        
        // Update of variables according to previous control and following of camera
        void UpdatePreviousCameraState()
        {
            _previousControlState = _currentControlState;
            _previousSmoothingState = _currentSmoothingState;
        }
        #endregion

        #region Mouse
        // Update of variables according to current mouse/touchpad movement: dragging and scrolling
        void UpdateCurrentMouseState()
        {
            _currentScroll = Input.GetAxis("Mouse ScrollWheel");
            _currentMousePosition = Input.mousePosition;
            _currentMouseState = MouseState.Rest;

            if (Input.GetKey(DragKey))
                _currentMouseState = MouseState.Dragging;

            if (_currentScroll != 0)
                _currentMouseState = MouseState.Scrolling;
        }
        
        // Update of variables according to previous mouse/touchpad drag position
        void UpdatePreviousDragPosition()
        {
            if (_currentControlState == ControlState.Rotation || _currentControlState == ControlState.Lifting || _previousDragPosition == Vector3.zero && _currentMouseState == MouseState.Dragging)
                _previousDragPosition = Input.mousePosition;
            else
                _previousDragPosition = Vector3.zero;
        }
        #endregion

        #region Fields
        // Update and conversion of float fields according to int values in editor sliders
        void UpdateFloatFields()
        {
            ConvertSliderValueToField(ref _minDistance, MinDistance, _minUpperDistance, _minLowerDistance, _specialMinSliderValue, _specialMaxSliderValue);
            ConvertSliderValueToField(ref _maxDistance, MaxDistance, _maxUpperDistance, _maxLowerDistance, _specialMinSliderValue, _specialMaxSliderValue);
            ConvertSliderValueToField(ref _minHeight, MinHeight, _minUpperHeight, _minLowerHeight, _specialMinSliderValue, _specialMaxSliderValue);
            ConvertSliderValueToField(ref _maxHeight, MaxHeight, _maxUpperHeight, _maxLowerHeight, _specialMinSliderValue, _specialMaxSliderValue);

            ConvertSliderValueToField(ref _distance, Distance, _maxDistance, _minDistance);
            ConvertSliderValueToField(ref _height, Height, _maxHeight, _minHeight);
            ConvertSliderValueToField(ref _rotation, Rotation, _maxRotation);

            ConvertSliderValueToField(ref _distancingSpeed, DistancingSpeed, _maxScrollingSpeed);
            ConvertSliderValueToField(ref _rotationSpeed, RotationSpeed, _maxDraggingSpeed);
            ConvertSliderValueToField(ref _liftingSpeed, LiftingSpeed, _maxDraggingSpeed);
            ConvertSliderValueToField(ref _followingSmoothness, FollowingSmoothness, _maxSmoothness, _minSmoothness);

            ConvertSliderValueToField(ref _distancingSmoothness, DistancingSmoothness, _maxSmoothness, _minSmoothness);
            ConvertSliderValueToField(ref _rotationSmoothness, RotationSmoothness, _maxSmoothness, _minSmoothness);
            ConvertSliderValueToField(ref _liftingSmoothness, LiftingSmoothness, _maxSmoothness, _minSmoothness);

            ConvertSliderValueToField(ref _borderWidthPercent, BorderWidth, _maxBorderSizePercent);
            ConvertSliderValueToField(ref _borderHeightPercent, BorderHeight, _maxBorderSizePercent);
            ConvertSliderValueToField(ref _borderRotationSpeed, BorderRotationSpeed, _maxBorderRotationSpeed, _minBorderRotationSpeed);
            ConvertSliderValueToField(ref _borderLiftingSpeed, BorderLiftingSpeed, _maxBorderLiftingSpeed, _minBorderLiftingSpeed);
            ConvertSliderValueToField(ref _borderRotationSmoothness, BorderRotationSmoothness, _maxSmoothness, _minSmoothness); // TODO ???
            ConvertSliderValueToField(ref _borderLiftingSmoothness, BorderLiftingSmoothness, _maxSmoothness, _minSmoothness);

            _maxLiftingThreshold = ConvertByAspectRatio(_maxRotationThreshold);
        }
        
        // Update and conversion of int slider values (in editor) to float
        void UpdateIntFields()
        {
            ConvertFieldToSliderValue(ref MinDistance, _minDistance, _minUpperDistance, _minLowerDistance, _specialMinSliderValue, _specialMaxSliderValue);
            ConvertFieldToSliderValue(ref MaxDistance, _maxDistance, _maxUpperDistance, _maxLowerDistance, _specialMinSliderValue, _specialMaxSliderValue);
            ConvertFieldToSliderValue(ref MinHeight, _minHeight, _minUpperHeight, _minLowerHeight, _specialMinSliderValue, _specialMaxSliderValue);
            ConvertFieldToSliderValue(ref MaxHeight, _maxHeight, _maxUpperHeight, _maxLowerHeight, _specialMinSliderValue, _specialMaxSliderValue);

            ConvertFieldToSliderValue(ref Distance, _distance, _maxDistance, _minDistance);
            ConvertFieldToSliderValue(ref Height, _height, _maxHeight, _minHeight);
            ConvertFieldToSliderValue(ref Rotation, _rotation, _maxRotation);

            ConvertFieldToSliderValue(ref DistancingSpeed, _distancingSpeed, _maxScrollingSpeed);
            ConvertFieldToSliderValue(ref RotationSpeed, _rotationSpeed, _maxDraggingSpeed);
            ConvertFieldToSliderValue(ref LiftingSpeed, _liftingSpeed, _maxDraggingSpeed);
            ConvertFieldToSliderValue(ref FollowingSmoothness, _followingSmoothness, _maxSmoothness, _minSmoothness);

            ConvertFieldToSliderValue(ref DistancingSmoothness, _distancingSmoothness, _maxSmoothness, _minSmoothness);
            ConvertFieldToSliderValue(ref RotationSmoothness, _rotationSmoothness, _maxSmoothness, _minSmoothness);
            ConvertFieldToSliderValue(ref LiftingSmoothness, _liftingSmoothness, _maxSmoothness, _minSmoothness);

            ConvertFieldToSliderValue(ref BorderWidth, _borderWidthPercent, _maxBorderSizePercent);
            ConvertFieldToSliderValue(ref BorderHeight, _borderHeightPercent, _maxBorderSizePercent);
            ConvertFieldToSliderValue(ref BorderRotationSpeed, _borderRotationSpeed, _maxBorderRotationSpeed, _minBorderRotationSpeed);
            ConvertFieldToSliderValue(ref BorderLiftingSpeed, _borderLiftingSpeed, _maxBorderLiftingSpeed, _minBorderLiftingSpeed);
            ConvertFieldToSliderValue(ref BorderRotationSmoothness, _borderRotationSmoothness, _maxSmoothness, _minSmoothness);
            ConvertFieldToSliderValue(ref BorderLiftingSmoothness, _borderLiftingSmoothness, _maxSmoothness, _minSmoothness);
        }
        #endregion
        #endregion

        #region Values Corrections
        // Conversion of value (dragging) according to aspect ratio of game window
        float ConvertByAspectRatio(float value)
        {
            return value * Screen.height / Screen.width;
        }

        // Fix of value that bigger than valid maximum limit
        float NormalizeValue(float value, float maxValue)
        {
            if (Mathf.Abs(value) > maxValue)
                value = maxValue * Mathf.Sign(value);
            return value;
        }

        // Correction of not valid value
        void CorrectValue(ref float newValue, float minValue, float maxValue)
        {
            newValue = newValue > maxValue ? maxValue : newValue;
            newValue = newValue < minValue ? minValue : newValue;
        }

        // Conversion of scale value shown in slider from int to float
        void ConvertSliderValueToField(ref float currentFieldValue, int currentSliderValue, float maxFieldValue, float minFieldValue = _minSliderValue,
                                       float minSliderValue = _minSliderValue, float maxSliderValue = _maxSliderValue)
        {
            currentFieldValue = minFieldValue + (maxFieldValue - minFieldValue) * (currentSliderValue - minSliderValue) / (maxSliderValue - minSliderValue);
        }

        // Conversion of initial float value of variable to int scale for slider
        void ConvertFieldToSliderValue(ref int currentSliderValue, float currentFieldValue, float maxFieldValue, float minFieldValue = _minSliderValue,
                                       float minSliderValue = _minSliderValue, float maxSliderValue = _maxSliderValue)
        {
            currentSliderValue = Mathf.RoundToInt(minSliderValue + (maxSliderValue - minSliderValue) * (currentFieldValue - minFieldValue) / (maxFieldValue - minFieldValue));
        }
        #endregion
        #endregion
    };
}