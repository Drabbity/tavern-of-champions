using UnityEngine;
using UnityEngine.UI;

namespace TavernOfChampions.UI.Menu
{
    [RequireComponent(typeof(Image))]
    public class ReadyButton : MonoBehaviour
    {
        [SerializeField] private Color _readyColor = Color.green;
        [SerializeField] private Color _notReadyColor = Color.red;

        private Image _buttonImage;

        public void Start()
        {
            _buttonImage = GetComponent<Image>();
            SetButtonStatus(false);
        }

        public void SetButtonStatus(bool ready)
        {
            if (ready)
                _buttonImage.color = _readyColor;
            else
                _buttonImage.color = _notReadyColor;
        }
    }
}
