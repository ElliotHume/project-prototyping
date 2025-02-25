using Synty.AnimationBaseLocomotion.Samples.InputSystem;
using UnityEngine;

namespace _Prototyping.Camera
{
    public class TopDownCameraController : MonoBehaviour
    {
        private const int _LAG_DELTA_TIME_ADJUSTMENT = 20;

        [Header("Dependencies")]
        [Tooltip("The character game object")]
        [SerializeField]
        private GameObject _playerCharacter;
        [Tooltip("Main camera used for player perspective")]
        [SerializeField]
        private UnityEngine.Camera _mainCamera;
        [SerializeField]
        private Transform _cameraPositionTarget;

        [Header("Options")]
        [SerializeField]
        private bool _invertCamera;
        [SerializeField]
        private bool _canRotate;
        [SerializeField]
        private float _mouseSensitivity = 5f;
        [SerializeField]
        private Vector2 _cameraTiltBounds = new Vector2(-10f, 45f);
        [SerializeField]
        private float _positionalCameraLag = 1f;
        [SerializeField]
        private float _rotationalCameraLag = 1f;
        private float _cameraInversion;

        private InputReader _inputReader;
        private float _lastAngleX;
        private float _lastAngleY;

        private Vector3 _lastPosition;

        private float _newAngleX;

        private float _newAngleY;
        private Vector3 _newPosition;
        private float _rotationX;
        private float _rotationY;

        private Transform _cameraTransform;

        /// <inheritdoc cref="Start" />
        private void Start()
        {
            _cameraTransform = _mainCamera.transform;

            _inputReader = _playerCharacter.GetComponent<InputReader>();

            _cameraInversion = _invertCamera ? 1 : -1;

            transform.position = _playerCharacter.transform.position;
            transform.rotation = _playerCharacter.transform.rotation;

            _lastPosition = transform.position;

            _cameraTransform.SetLocalPositionAndRotation(_cameraPositionTarget.localPosition, _cameraPositionTarget.localRotation);
        }

        /// <inheritdoc cref="Update" />
        private void Update()
        {
            float positionalFollowSpeed = 1 / (_positionalCameraLag / _LAG_DELTA_TIME_ADJUSTMENT);
            float rotationalFollowSpeed = 1 / (_rotationalCameraLag / _LAG_DELTA_TIME_ADJUSTMENT);

            if (_canRotate)
            {
                _rotationX = _inputReader._mouseDelta.y * _cameraInversion * _mouseSensitivity;
                _rotationY = _inputReader._mouseDelta.x * _mouseSensitivity;

                _newAngleX += _rotationX;
                _newAngleX = Mathf.Clamp(_newAngleX, _cameraTiltBounds.x, _cameraTiltBounds.y);
                _newAngleX = Mathf.Lerp(_lastAngleX, _newAngleX, rotationalFollowSpeed * Time.deltaTime);
            
                _newAngleY += _rotationY;
                _newAngleY = Mathf.Lerp(_lastAngleY, _newAngleY, rotationalFollowSpeed * Time.deltaTime);
            }
            else
            {
                _newAngleX = 0f;
                _newAngleY = 0f;
            }

            _newPosition = _playerCharacter.transform.position;
            _newPosition = Vector3.Lerp(_lastPosition, _newPosition, positionalFollowSpeed * Time.deltaTime);

            transform.position = _newPosition;
            transform.eulerAngles = new Vector3(_newAngleX, _newAngleY, 0);

            _cameraTransform.SetLocalPositionAndRotation(_cameraPositionTarget.localPosition, _cameraPositionTarget.localRotation);

            _lastPosition = _newPosition;
            _lastAngleX = _newAngleX;
            _lastAngleY = _newAngleY;
        }
        
        /// <summary>
        /// Gets the position of the camera.
        /// </summary>
        /// <returns>The position of the camera.</returns>
        public Vector3 GetCameraPosition()
        {
            return _mainCamera.transform.position;
        }

        /// <summary>
        /// Gets the forward vector of the camera.
        /// </summary>
        /// <returns>The forward vector of the camera.</returns>
        public Vector3 GetCameraForward()
        {
            return _mainCamera.transform.forward;
        }

        /// <summary>
        /// Gets the forward vector of the camera with the Y value zeroed.
        /// </summary>
        /// <returns>The forward vector of the camera with the Y value zeroed.</returns>
        public Vector3 GetCameraForwardZeroedY()
        {
            return new Vector3(_mainCamera.transform.forward.x, 0, _mainCamera.transform.forward.z);
        }

        /// <summary>
        /// Gets the normalised forward vector of the camera with the Y value zeroed.
        /// </summary>
        /// <returns>The normalised forward vector of the camera with the Y value zeroed.</returns>
        public Vector3 GetCameraForwardZeroedYNormalised()
        {
            return GetCameraForwardZeroedY().normalized;
        }


        /// <summary>
        /// Gets the right vector of the camera with the Y value zeroed.
        /// </summary>
        /// <returns>The right vector of the camera with the Y value zeroed.</returns>
        public Vector3 GetCameraRightZeroedY()
        {
            return new Vector3(_mainCamera.transform.right.x, 0, _mainCamera.transform.right.z);
        }

        /// <summary>
        /// Gets the normalised right vector of the camera with the Y value zeroed.
        /// </summary>
        /// <returns>The normalised right vector of the camera with the Y value zeroed.</returns>
        public Vector3 GetCameraRightZeroedYNormalised()
        {
            return GetCameraRightZeroedY().normalized;
        }

        /// <summary>
        /// Gets the X value of the camera tilt.
        /// </summary>
        /// <returns>The X value of the camera tilt.</returns>
        public float GetCameraTiltX()
        {
            return _mainCamera.transform.eulerAngles.x;
        }
    }
}
