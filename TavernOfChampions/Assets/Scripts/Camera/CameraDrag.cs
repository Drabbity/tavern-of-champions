using TavernOfChampions.Helper;
using UnityEngine;

namespace TavernOfChampions.CameraMovement
{
    public class CameraDrag : MonoBehaviour
    {
        [SerializeField] private int _dragMouseButton = 2;

        private GameObject _camera;
        private bool _isDragging = false;
        private Vector3 _startingMousePosition;

        private void Start()
            => _camera = Camera.main.gameObject;

        private void Update()
        {
            DragCamera();
        }

        private void DragCamera()
        {
            if (Input.GetMouseButton(_dragMouseButton))
            {
                if (!_isDragging)
                {
                    _isDragging = true;
                    _startingMousePosition = MouseHelper.GetMouseWorldPosition();
                }

                var offset = MouseHelper.GetMouseWorldPosition() - _camera.transform.position;
                var _newCameraPosition = _startingMousePosition - offset;

                _camera.transform.position = _newCameraPosition;
            }
            else
            {
                _isDragging = false;
            }
        }
    }
}
