using UnityEngine;

namespace TavernOfChampions.Grid
{
    public class GridTile : MonoBehaviour
    {
        private readonly Color _OVERLAY_DEFAULT_COLOR = Color.clear;

        [SerializeField] private SpriteRenderer _mainTile;
        [SerializeField] private SpriteRenderer _overlayTile;

        private Color _overlayHighlightColor;

        public void Initialize(Color tileColor, Color overlayHighlightColor)
        {
            _overlayHighlightColor = overlayHighlightColor;

            _mainTile.color = tileColor;
            HighlightTile(false);
        }

        public void HighlightTile(bool highlight)
        {
            if (highlight)
                _overlayTile.color = _overlayHighlightColor;
            else
                _overlayTile.color = _OVERLAY_DEFAULT_COLOR;
        }
    }
}
