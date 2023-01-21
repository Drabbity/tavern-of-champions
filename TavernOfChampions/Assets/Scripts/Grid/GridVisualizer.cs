using TavernOfChampions.Logging;
using UnityEngine;

namespace TavernOfChampions.Grid
{
    public class GridVisualizer : MonoBehaviour
    {
        [SerializeField] private GridTile _tilePrefab;

        [SerializeField] private Color _tileColor1;
        [SerializeField] private Color _tileColor2;
        [SerializeField] private Color _overlayHighlightColor;

        private bool _isGridGenerated = false;
        private GridTile[,] _tiles;

        public void GenerateGrid(Vector2Int gridSize)
        {
            if (_isGridGenerated)
            {
                GameLogger.Instance.Warning("Cannot generate Grid. Grid already exists.", LoggerType.GRID, this);
                return;
            }

            _isGridGenerated = true;
            _tiles = new GridTile[gridSize.x, gridSize.y];

            GameLogger.Instance.Info("Generating Grid...", LoggerType.GRID, this);

            var gridParent = new GameObject("GridParent");

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    _tiles[x, y] = Instantiate(_tilePrefab, new Vector2(x, y), Quaternion.identity, gridParent.transform);
                    _tiles[x, y].Initialize((x + y) % 2 == 0 ? _tileColor1 : _tileColor2, _overlayHighlightColor);
                }
            }

            GameLogger.Instance.Info("Grid Generated.", LoggerType.GRID, this);
        }

        public void HighlightTile(Vector2Int tileIndex, bool highlight)
            => _tiles[tileIndex.x, tileIndex.y].HighlightTile(highlight);
    }
}
