using UnityEditor;
using UnityEngine;

namespace CameraEditor
{
    [CustomEditor(typeof(CameraController.CameraController))]
    [CanEditMultipleObjects]
    public class CameraEditor : Editor
    {
        #region Fields
        #region Serialized
        private SerializedObject _camera;
        private SerializedProperty _player, _positionOffset,
                                   _distance, _rotation, _height,
                                   _dragKey,
                                   _separateDrag, _playerRotation,
                                   _distancingSpeed, _rotationSpeed, _liftingSpeed, _followingSmoothness,
                                   _distancingSmoothness, _rotationSmoothness, _liftingSmoothness,
                                   _borderWidth, _borderHeight, _borderRotationSpeed, _borderLiftingSpeed,
                                   _borderRotationSmoothness, _borderLiftingSmoothness,
                                   _minDistance, _maxDistance, _minHeight, _maxHeight,
                                   _currentControllingState, _currentSmoothingState;

        #endregion

        #region Toolbar
        private int _currentToolbarIndex = 0, _previousToolbarIndex = 0;
        #endregion

        #region Slider
        private const int _minSliderValue = 0, _maxSliderValue = 100;
        private readonly int _specialMinSliderValue = 1, _specialMaxSliderValue = 10;
        #endregion

        #region Details
        private bool _distancingDetails = false, _rotationDetails = false, _liftingDetails = false;
        #endregion

        #region Spaces
        private const float _smallSpace = 5;
        private readonly float _mediumSpace = 10, _largeSpace = 15;
        #endregion

        #region Visual
        private Texture _logo;
        private GUIStyle _logoStyle, _groupStyle, _foldoutStyle;
        private readonly float _logoHeight = 20;
        private readonly int _groupOffset = 10, _foldoutOffset = 15, _logoOffset = 2;
        #endregion
        #endregion

        #region Methods
        #region Main
        void OnEnable()
        {
            _camera = new SerializedObject(target);

            //SetProperty(ref _currentControllingState, "_currentControlState");
            //SetProperty(ref _currentSmoothingState, "_currentSmoothingState");

            SetProperty(ref _player, "Player");
            SetProperty(ref _positionOffset, "PositionOffset");

            SetProperty(ref _distance, "Distance");
            SetProperty(ref _rotation, "Rotation");
            SetProperty(ref _height, "Height");

            SetProperty(ref _dragKey, "DragKey");
            SetProperty(ref _separateDrag, "SeparateDrag");
            SetProperty(ref _playerRotation, "PlayerRotation");

            SetProperty(ref _distancingSpeed, "DistancingSpeed");
            SetProperty(ref _rotationSpeed, "RotationSpeed");
            SetProperty(ref _liftingSpeed, "LiftingSpeed");
            SetProperty(ref _followingSmoothness, "FollowingSmoothness");

            SetProperty(ref _borderRotationSpeed, "BorderRotationSpeed");
            SetProperty(ref _borderLiftingSpeed, "BorderLiftingSpeed");
            SetProperty(ref _borderRotationSmoothness, "BorderRotationSmoothness");
            SetProperty(ref _borderLiftingSmoothness, "BorderLiftingSmoothness");
            SetProperty(ref _borderHeight, "BorderHeight");
            SetProperty(ref _borderWidth, "BorderWidth");

            SetProperty(ref _distancingSmoothness, "DistancingSmoothness");
            SetProperty(ref _rotationSmoothness, "RotationSmoothness");
            SetProperty(ref _liftingSmoothness, "LiftingSmoothness");

            SetProperty(ref _minDistance, "MinDistance");
            SetProperty(ref _maxDistance, "MaxDistance");
            SetProperty(ref _minHeight, "MinHeight");
            SetProperty(ref _maxHeight, "MaxHeight");

            _logo = (Texture)Resources.Load("Company Logo");
        }
        public override void OnInspectorGUI()
        {
            Logo();
            Toolbar(ref _currentToolbarIndex, new string[] { "Position", "Control", "Border", "Limits" }, new string[] { "Camera and target position", "Camera control settings", "Screen border control settings", "Camera position limits" });
            BeginGroup();

            switch (_currentToolbarIndex)
            {
                case 0:
                    //PropertyField(_currentControllingState, "Control", _largeSpace);
                    //PropertyField(_currentSmoothingState, "Smooth", _largeSpace);
                    Label("Target", "Camera target settings", true);
                    PropertyField(_player, "Player", _smallSpace, "Transform of player object");
                    PropertyField(_positionOffset, "Position Offset", _smallSpace, "Actual center of player");
                    Label("Camera", "Camera position");
                    Slider(_distance, "Distance", "Current distance between camera and player");
                    Slider(_rotation, "Rotation", "Current camera rotation around player");
                    Slider(_height, "Height", "Current camera height above player");
                    break;
                case 1:
                    Label("Distancing", "", true);
                    Slider(_distancingSpeed, "Speed");

                    if (IsPropertyPositive(_distancingSpeed))
                        Slider(_distancingSmoothness, "Smoothness");

                    bool isRotationEnabled = IsPropertyPositive(_rotationSpeed);
                    bool isLiftingEnabled = IsPropertyPositive(_liftingSpeed);

                    Label("Rotation", "Horizontal rotation");
                    Slider(_rotationSpeed, "Speed");
                    if (isRotationEnabled)
                        Slider(_rotationSmoothness, "Smoothness");

                    Label("Lifting", "Vertical rotation");
                    Slider(_liftingSpeed, "Speed");
                    if (isLiftingEnabled)
                        Slider(_liftingSmoothness, "Smoothness");

                    Label("Following", "Smoothness of camera following");
                    Slider(_followingSmoothness, "Smoothness");

                    if (isRotationEnabled || isLiftingEnabled)
                        PropertyField(_dragKey, "Drag Key", _largeSpace);

                    if (isRotationEnabled && isLiftingEnabled)
                        PropertyField(_separateDrag, "Separate Drag");

                    if (isRotationEnabled)
                        PropertyField(_playerRotation, "Player Rotation");
                    break;
                case 2:
                    bool isBorderRotationEnabled = IsPropertyPositive(_borderRotationSpeed);
                    bool isBorderLiftingEnabled = IsPropertyPositive(_borderLiftingSpeed);

                    Label("Rotation", "Border rotation", true);
                    Slider(_borderRotationSpeed, "Speed");
                    if (isBorderRotationEnabled)
                    {
                        Slider(_borderRotationSmoothness, "Smoothness");
                        Slider(_borderWidth, "Border Width");
                    }

                    Label("Lifting", "Border lifting");
                    Slider(_borderLiftingSpeed, "Speed");
                    if (isBorderLiftingEnabled)
                    {
                        Slider(_borderLiftingSmoothness, "Smoothness");
                        Slider(_borderHeight, "Border Height");
                    }
                    break;
                case 3:
                    Label("Distance", "Camera distance limits", true);
                    Slider(_minDistance, "Minimum", "", _specialMinSliderValue, _specialMaxSliderValue);
                    Slider(_maxDistance, "Maximum", "", _specialMinSliderValue, _specialMaxSliderValue);
                    Label("Height", "Camera height limits");
                    Slider(_minHeight, "Minimum", "", _specialMinSliderValue, _specialMaxSliderValue);
                    Slider(_maxHeight, "Maximum", "", _specialMinSliderValue, _specialMaxSliderValue);
                    break;
            }
            EndGroup();
        }
        #endregion

        #region Inspector
        void BeginGroup()
        {
            if (_groupStyle == null)
                SetGroupStyle();
            EditorGUILayout.BeginVertical(_groupStyle);
            _camera.Update();
            EditorGUI.BeginChangeCheck();
        }
        void EndGroup()
        {
            if (EditorGUI.EndChangeCheck())
            {
                _camera.ApplyModifiedProperties();
                ResetFocusControl();
            }
            _previousToolbarIndex = _currentToolbarIndex;
            EditorGUILayout.EndVertical();
        }
        #endregion

        #region Elements
        void Logo()
        {
            if (_logoStyle == null)
                SetLogoStyle();
            GUILayout.Label(_logo, _logoStyle, GUILayout.Height(_logoHeight), GUILayout.ExpandWidth(true));
        }
        void Toolbar(ref int currentIndex, string[] texts, string[] tooltips)
        {
            int length = texts.Length;
            GUIContent[] contents = new GUIContent[length];
            for (int i = 0; i < length; i++)
                contents[i] = new GUIContent(texts[i], tooltips[i]);
            currentIndex = GUILayout.Toolbar(currentIndex, contents);
        }
        void Label(string text, string tooltip = "", bool onTop = false)
        {
            if (!onTop)
                Space(_largeSpace);

            EditorGUILayout.LabelField(new GUIContent(text, tooltip), new GUIStyle { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.BoldAndItalic });
            Space(_smallSpace);
        }
        void PropertyField(SerializedProperty property, string label, float spaceSize = _smallSpace, string tooltip = "")
        {
            Space(spaceSize);
            EditorGUILayout.PropertyField(property, new GUIContent(label, tooltip));
        }
        void Slider(SerializedProperty property, string label, string tooltip = "", int minSliderValue = _minSliderValue, int maxSliderValue = _maxSliderValue, bool inFoldout = false)
        {
            Space(_smallSpace);
            EditorGUILayout.IntSlider(property, minSliderValue, maxSliderValue, new GUIContent((inFoldout ? "     " : "") + label, tooltip));
        }
        void Foldout(ref bool value, string label)
        {
            Space(_smallSpace);

            if (_foldoutStyle == null)
                SetFoldoutStyle();

            value = EditorGUILayout.Foldout(value, label, true, _foldoutStyle);
        }
        void Space(float pixels)
        {
            GUILayout.Space(pixels);
        }
        #endregion

        #region Check
        bool IsPropertyPositive(SerializedProperty property)
        {
            return property.intValue > _minSliderValue;
        }
        #endregion

        #region Updates
        void SetProperty(ref SerializedProperty property, string name)
        {
            property = _camera.FindProperty(name);
        }
        void SetLogoStyle()
        {
            _logoStyle = new GUIStyle { alignment = TextAnchor.MiddleCenter, padding = new RectOffset(0, 0, _logoOffset, _logoOffset) };
        }
        void SetGroupStyle()
        {
            _groupStyle = GUI.skin.box;
            _groupStyle.padding = new RectOffset(_groupOffset, _groupOffset, _groupOffset, _groupOffset);
        }
        void SetFoldoutStyle()
        {
            _foldoutStyle = EditorStyles.foldout;
            _foldoutStyle.padding = new RectOffset();
            _foldoutStyle.contentOffset = new Vector2(_foldoutOffset, 0);
        }
        void ResetFocusControl()
        {
            if (_currentToolbarIndex != _previousToolbarIndex)
                GUI.FocusControl(null);
        }
        #endregion
        #endregion
    }
}
