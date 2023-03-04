using TavernOfChampions.Champion;
using UnityEngine;
using UnityEngine.UI;

namespace TavernOfChampions.UI
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Button))]
    public class ChampionSelection : MonoBehaviour
    {
        public void Initialize(Sprite championBanner, string championName, ChampionSelectionManager selectionManager)
        {
            GetComponent<Image>().sprite = championBanner;
            GetComponent<Button>().onClick.AddListener(() => { selectionManager.SelectChampion(championName); });
        }
    }
}