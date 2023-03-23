using Photon.Realtime;
using System.Collections.Generic;
using TavernOfChampions.Grid;
using UnityEngine;

namespace TavernOfChampions.Champion.Actions
{
    public static class LegalTileValidation
    {
        public static List<Vector2Int> ValidateBorders(List<Vector2Int> tiles, GridManager gridManager)
        {
            for(int i = tiles.Count - 1; i >= 0; i--)
            {
                var tile = tiles[i];

                if (tile.x < 0 || tile.x >= gridManager.GridSize.x ||
                    tile.y < 0 || tile.y >= gridManager.GridSize.y)
                {
                    tiles.RemoveAt(i);
                }
            }

            return tiles;
        }

        public static List<Vector2Int> ValidateChampions(List<Vector2Int> tiles, GridManager gridManager)
        {
            tiles = ValidateBorders(tiles, gridManager);

            for (int i = tiles.Count - 1; i >= 0; i--)
            {
                var tile = tiles[i];

                if (gridManager.GetChampion(tile))
                {
                    tiles.RemoveAt(i);
                }
            }

            return tiles;
        }

        public static List<Vector2Int> ValidateChampions(List<Vector2Int> tiles, GridManager gridManager, Player owner)
        {
            tiles = ValidateBorders(tiles, gridManager);

            for (int i = tiles.Count - 1; i >= 0; i--)
            {
                var tile = tiles[i];

                if (gridManager.GetChampion(tile) && gridManager.GetChampion(tile).Owner == owner)
                {
                    tiles.RemoveAt(i);
                }
            }

            return tiles;
        }
    }
}
