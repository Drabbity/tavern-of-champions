using TavernOfChampions.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TavernOfChampions.Grid
{
    public class GridTile : MonoBehaviour
    {
        private readonly Color _OVERLAY_DEFAULT_COLOR = Color.clear;

        [SerializeField] private SpriteRenderer _mainTile;
        [SerializeField] private SpriteRenderer _overlayTile;

        private UIHoverListener _uiHoverListener;
        private Color _overlayHighlightColor;

        private Vector2Int _tileLocation;

        public void Initialize(Color tileColor, Color overlayHighlightColor, Vector2Int tileLocation, UIHoverListener uiHoverListener)
        {
            _overlayHighlightColor = overlayHighlightColor;
            _tileLocation = tileLocation;

            _mainTile.color = tileColor;
            HighlightTile(false);

            _uiHoverListener = uiHoverListener;
        }

        public void HighlightTile(bool highlight)
        {
            if (highlight)
                _overlayTile.color = _overlayHighlightColor;
            else
                _overlayTile.color = _OVERLAY_DEFAULT_COLOR;
        }

        public void OnMouseDown()
        {
            if(!_uiHoverListener.IsPointerOverUI())
                GridManager.Instance.SelectTile(_tileLocation);
        }
    }
}
