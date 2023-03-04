using UnityEngine;

namespace TavernOfChampions.Champion
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ChampionOutline : MonoBehaviour
    {
        [SerializeField] private Color _enemyColor;
        [SerializeField] private Color _allyColor;

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void ChangeOutlineColor(bool isEnemy)
        {
            if (isEnemy)
                _spriteRenderer.color = _enemyColor;
            else
                _spriteRenderer.color = _allyColor;
        }
    }
}

