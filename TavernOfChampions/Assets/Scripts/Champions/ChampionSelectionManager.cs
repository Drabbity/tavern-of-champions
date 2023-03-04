using System.Collections.Generic;
using TavernOfChampions.GameState;
using TavernOfChampions.UI;
using UnityEngine;

namespace TavernOfChampions.Champion
{
    public class ChampionSelectionManager : MonoBehaviour
    {
        [SerializeField] private ChampionSelectionGrid _selectionGrid;
        [SerializeField] private GameStateManager _gameStateManager;

        public Queue<string> SelectedChampions { get; private set; } = new Queue<string>();

        public void Initialize(ChampionList championList)
        {
            _selectionGrid.Initialize(championList, this);
        }

        public void SelectChampion(string champion)
        {
            if (SelectedChampions.Count >= 5)
                return;

            SelectedChampions.Enqueue(champion);

            if (SelectedChampions.Count == 5)
                _gameStateManager.SwitchState();
        }
    }
}
