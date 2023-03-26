/*using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace TavernOfChampions.Champion.Actions
{
    public class ActionSpawner : ChampionAction
    {
        [SerializeField] private ChampionController _spawnPrefab;

        [PunRPC]
        public override void Execute(Player player, Vector2Int tile)
        {
            _gridManager.SpawnChampion(_spawnPrefab, tile, player);

            if(PhotonNetwork.LocalPlayer == player)
                _championController.CurrentAction = null;
        }

        public override Vector2Int[] GetLegalMoves()
        {
            List<Vector2Int> spawnableTiles = new List<Vector2Int>();
            spawnableTiles.Add(new Vector2Int(0, 0));

            return spawnableTiles.ToArray();
        }
    }
}*/