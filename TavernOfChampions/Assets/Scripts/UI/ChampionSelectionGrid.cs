using TavernOfChampions.Champion;
using UnityEngine;

namespace TavernOfChampions.UI
{
    public class ChampionSelectionGrid : MonoBehaviour
    {
        [SerializeField] private ChampionSelection _championSelectionPrefab;
        [SerializeField] private Transform _content;

        public void Initialize(ChampionList championList, ChampionSelectionManager selectionManager)
        {
            foreach (var stringChampion in championList.Champions)
            {
                var newChampionSelection = Instantiate(_championSelectionPrefab, _content);
                newChampionSelection.Initialize(stringChampion.Value.ChampionBanner, stringChampion.Key, selectionManager);
            }
        }
    }
}
