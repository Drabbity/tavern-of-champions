using UnityEngine;

namespace TavernOfChampions.Grid
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private Vector2Int _gridSize;
        [SerializeField] private GridVisualizer _gridVisualizer;

        void Start()
        {
            _gridVisualizer.GenerateGrid(_gridSize);
        }
    }
}

