using TavernOfChampions.Logging;
using TavernOfChampions.UI;
using UnityEngine;

namespace TavernOfChampions.Grid
{
    public class GridVisualizer : MonoBehaviour
    {
        [SerializeField] private GridTile _tilePrefab;
        [SerializeField] private UIHoverListener _uiHoverListener;

        [SerializeField] private Color _tileColor1;
        [SerializeField] private Color _tileColor2;
        [SerializeField] private Color _overlayHighlightColor;

        private bool _isGridGenerated = false;
        private Vector2Int _gridSize;
        private GameObject _gridParent;
        private GridTile[,] _tiles;

        public void GenerateGrid(Vector2Int gridSize)
        {
            if (_isGridGenerated)
            {
                GameLogger.Instance.Warning("Cannot generate Grid. Grid already exists.", LoggerType.GRID, this);
                return;
            }

            _gridSize = gridSize;
            _isGridGenerated = true;
            _tiles = new GridTile[gridSize.x, gridSize.y];

            GameLogger.Instance.Info("Generating Grid...", LoggerType.GRID, this);

            _gridParent = new GameObject("GridParent");

            Vector2 centerOffset = gridSize / 2;
            centerOffset.x -= (gridSize.x % 2 == 0 ? 0.5f : 0);
            centerOffset.y -= (gridSize.y % 2 == 0 ? 0.5f : 0);

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    _tiles[x, y] = Instantiate(_tilePrefab, new Vector2(x - centerOffset.x, y - centerOffset.y), Quaternion.identity, _gridParent.transform);
                    _tiles[x, y].Initialize((x + y) % 2 == 0 ? _tileColor1 : _tileColor2, _overlayHighlightColor, new Vector2Int(x, y), _uiHoverListener);
                }
            }

            GameLogger.Instance.Info("Grid Generated.", LoggerType.GRID, this);
        }

        public void HighlightTiles(Vector2Int[] tiles)
        {
            ClearHighlights();

            foreach(var tile in tiles)
            {
                HighlightTile(tile, true);
            }
        }

        public void ClearHighlights()
        {
            for(int x = 0; x < _gridSize.x; x++)
            {
                for(int y = 0; y < _gridSize.y; y++)
                {
                    HighlightTile(new Vector2Int(x, y), false);
                }
            }
        }

        public void HighlightTile(Vector2Int tileIndex, bool highlight)
            => _tiles[tileIndex.x, tileIndex.y].HighlightTile(highlight);

        public Vector2 GetTilePosition(Vector2Int tileIndex)
            => _tiles[tileIndex.x, tileIndex.y].transform.position;

        public void RotateGrid(bool isInverted)
        {
            var currentRotation = _gridParent.transform.eulerAngles;
            currentRotation.z = (isInverted ? 180f : 0);

            _gridParent.transform.eulerAngles = currentRotation;
        }
    }
}
