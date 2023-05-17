using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TavernOfChampions.Champion.Actions.UI
{
    public class ActionCardsList : MonoBehaviour
    {
        public static ActionCardsList Instance { get; private set; }

        [SerializeField] private GameObject _cardPrefab;

        private List<GameObject> _cards = new List<GameObject>();

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }

        public void PopulateList(ChampionAction[] actions, ChampionController controller)
        {
            foreach(var action in actions)
            {
                var newCard = Instantiate(_cardPrefab, transform);
                newCard.GetComponent<ActionCard>().SetUp(action.ActionCardSymbolSprite, () => { controller.CurrentAction = action; });
                _cards.Add(newCard);
            }
        }

        public void ClearList()
        {
            foreach(var card in _cards)
            {
                Destroy(card);
            }
            _cards.Clear();
        }
    }
}
