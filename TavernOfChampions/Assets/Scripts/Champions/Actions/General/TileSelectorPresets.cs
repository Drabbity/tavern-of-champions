using System.Collections.Generic;
using UnityEngine;

namespace TavernOfChampions.Champion.Actions
{
    public static class TileSelectorPresets
    {
        public static List<Vector2Int> SelectRadius(int radius, Vector2Int center)
        {
            List<Vector2Int> tiles = new List<Vector2Int>();

            for(int x = -radius; x <= radius; x++)
            {
                for(int y = -radius; y <= radius; y++)
                {
                    if (Mathf.Abs(x) == radius || Mathf.Abs(y) == radius)
                        tiles.Add(new Vector2Int(x + center.x, y + center.y));
                }
            }

            return tiles;
        }
    }
}
