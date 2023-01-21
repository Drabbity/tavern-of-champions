using TavernOfChampions.Helper;
using UnityEngine;

namespace TavernOfChampions.CameraMovement
{
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] private float _sensitivity = 2f;
        [SerializeField] private float _minZoom = 1f;
        [SerializeField] private float _maxZoom = 10f;

        private Camera _mainCamera;
        private float _cameraZPosition;

        private void Start()
        {
            _mainCamera = Camera.main;
            _cameraZPosition = _mainCamera.transform.position.z;
        }

        private void Update()
        {
            ZoomCamera();
        }

        private void ZoomCamera()
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                var preZoomMousePosition = MouseHelper.GetMouseWorldPosition();

                _mainCamera.orthographicSize = CalculateNewCameraZoom();
                _mainCamera.transform.position = CalculateNewCameraPosition(preZoomMousePosition);
            }
        }

        private float CalculateNewCameraZoom()
        {
            float zoom = _mainCamera.orthographicSize;
            zoom -= Input.GetAxis("Mouse ScrollWheel") * _sensitivity;
            zoom = Mathf.Clamp(zoom, _minZoom, _maxZoom);

            return zoom;
        }

        private Vector3 CalculateNewCameraPosition(Vector3 preZoomMousePosition)
        {
            var postZoomMousePosition = MouseHelper.GetMouseWorldPosition();
            var mousePositionDifference = preZoomMousePosition - postZoomMousePosition;

            var currentCameraPosition = _mainCamera.transform.position;

            var newCameraPosition = currentCameraPosition + mousePositionDifference;
            newCameraPosition.z = _cameraZPosition;

            return newCameraPosition;
        }
    }
}
