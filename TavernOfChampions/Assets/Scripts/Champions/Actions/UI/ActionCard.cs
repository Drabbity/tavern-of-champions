using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TavernOfChampions.Champion.Actions.UI
{
    public class ActionCard : MonoBehaviour
    {
        [SerializeField] private Image _actionCardSymbolImage;
        [SerializeField] private Button _actionCardButton;

        public void SetUp(Sprite symbolSprite, UnityAction onButtonClick)
        {
            _actionCardSymbolImage.sprite = symbolSprite;
            _actionCardButton.onClick.AddListener(onButtonClick);
        }
    }
}
