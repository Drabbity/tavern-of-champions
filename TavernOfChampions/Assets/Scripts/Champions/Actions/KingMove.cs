using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace TavernOfChampions.Champion.Actions
{
    public class KingMove : ChampionAction
    {
        [SerializeField] private int _moves = 5;

        private int[,] _moveMap;
        private List<Vector2Int> _movableTiles = new List<Vector2Int>();

        [PunRPC]
        public override void Execute(Player player, Vector2Int tile)
        {
            _gridManager.MoveChampion(_championController.CurrentPosition, tile);

            if (PhotonNetwork.LocalPlayer == player)
                _championController.CurrentAction = null;
        }

        public override Vector2Int[] GetLegalMoves()
        {
            _movableTiles.Clear();
            _moveMap = CreateMoveMap(_gridManager.GridSize);
            SelectTile(_championController.CurrentPosition, _moves);

            return _movableTiles.ToArray();
        }

        private void SelectTile(Vector2Int tile, int moves)
        {
            if (moves < 0 ||
                tile.x < 0 || tile.x >= _gridManager.GridSize.x ||
                tile.y < 0 || tile.y >= _gridManager.GridSize.y ||
                _moveMap[tile.x, tile.y] >= moves || (_gridManager.GetChampion(tile) && moves != _moves))
                return;

            _moveMap[tile.x, tile.y] = moves;
            if(moves != _moves)
            {
                _movableTiles.Add(tile);
            }

            SelectTile(new Vector2Int(tile.x + 1, tile.y), moves - 1);
            SelectTile(new Vector2Int(tile.x - 1, tile.y), moves - 1);
            SelectTile(new Vector2Int(tile.x, tile.y + 1), moves - 1);
            SelectTile(new Vector2Int(tile.x, tile.y - 1), moves - 1);
        }

        private int[,] CreateMoveMap(Vector2Int size)
        {
            int[,] moveMap = new int[size.x, size.y];
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    moveMap[x, y] = -1;
                }
            }

            return moveMap;
        }
    }
}
