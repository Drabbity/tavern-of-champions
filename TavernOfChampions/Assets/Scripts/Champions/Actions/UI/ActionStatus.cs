using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TavernOfChampions.Champion.Actions.UI
{
    public class ActionStatus : MonoBehaviour
    {
        [SerializeField] private Image _symbolImage;
        [SerializeField] private TextMeshProUGUI _countText;

        public void SetSprite(Sprite sprite)
            => _symbolImage.sprite = sprite;

        public void SetCount(int count)
            => _countText.text = count.ToString();
    }
}