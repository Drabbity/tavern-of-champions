using TavernOfChampions.Champion;
using TavernOfChampions.Logging;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TavernOfChampions.Turn;

namespace TavernOfChampions.Grid
{
    public class GridManager : MonoBehaviourPun
    {
        public static GridManager Instance { get; private set; }

        public int ChampionViewId => _championViewId++;
        [SerializeField] private int _championViewId;

        public ChampionController SelectedChampion
        {
            get
            {
                return _selectedChampion;
            }

            private set
            {
                GridVisualizer.ClearHighlights();

                _selectedChampion?.DeSelect();

                _selectedChampion = value;

                _selectedChampion?.Select();
            }
        }
        private ChampionController _selectedChampion = null;

        [field: SerializeField] public GridVisualizer GridVisualizer {  get; private set; }
        [field: SerializeField] public Vector2Int GridSize { get; private set; }

        [SerializeField] private TurnManager _turnManager;
        [SerializeField] private ChampionSelectionManager _selecitonManager;
        [SerializeField] private ChampionList _championList;

        private ChampionController[,] _championGrid;
        private GameObject _championParent;

        private bool _isBoardInverted = false;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }

        private void Start()
        {
            GridVisualizer.GenerateGrid(GridSize);
            _championGrid = GenerateChampionGrid(GridSize);
            _championParent = new GameObject("ChampionParent");
            _selecitonManager.Initialize(_championList);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                _isBoardInverted = !_isBoardInverted;
                RotateBoard(_isBoardInverted);
            }
        }

        [PunRPC]
        public void SpawnChampion(string championName, Vector2Int location, Player owner)
            => SpawnChampion(_championList.Champions[championName], location, owner);

        public void SpawnChampion(ChampionController champion, Vector2Int location, Player owner)
        {
            if (GetChampion(location))
                return;

            var tilePosition = GridVisualizer.GetTilePosition(location);

            _championGrid[location.x, location.y] = Instantiate(champion, tilePosition, Quaternion.identity, _championParent.transform);
            _championGrid[location.x, location.y].Initialize(this, _turnManager, location, owner);
            GameLogger.Instance.Info($"Champion { champion } spawned at tile { location }", LoggerType.CHAMPION, this);
        }

        public void SelectTile(Vector2Int tile)
        {
            if(_selecitonManager.SelectedChampions.Any())
            {
                if(!GetChampion(tile))
                {
                    base.photonView.RPC("SpawnChampion", RpcTarget.All, _selecitonManager.SelectedChampions.Dequeue(), tile, PhotonNetwork.LocalPlayer);
                }
            }
            else
            {
                if (!SelectedChampion || SelectedChampion.CurrentAction == null || !SelectedChampion.CurrentAction.GetLegalMoves().Any(x => x == tile))
                {
                    if (GetChampion(tile))
                    {
                        SelectedChampion = GetChampion(tile);
                    }
                    else
                    {
                        SelectedChampion = null;
                    }
                }
                else
                {
                    SelectedChampion.CurrentAction.Execute(tile);
                }
            }
        }

        public ChampionController GetChampion(Vector2Int tile)
            => _championGrid[tile.x, tile.y];

        public void MoveChampion(Vector2Int fromPosition, Vector2Int toPosition)
        {
            if(!GetChampion(fromPosition))
            {
                GameLogger.Instance.Error($"Can't move champion because there is no Champion at position { fromPosition }", LoggerType.CHAMPION, this); ;
                return;
            }
            if (GetChampion(toPosition))
            {
                GameLogger.Instance.Error($"Can't move champion because there is already a Champion at position { toPosition }", LoggerType.CHAMPION, this); ;
                return;
            }

            var champion = GetChampion(fromPosition);
            champion.CurrentPosition = toPosition;

            _championGrid[fromPosition.x, fromPosition.y] = null;
            _championGrid[toPosition.x, toPosition.y] = champion;

            champion.transform.position = GridVisualizer.GetTilePosition(toPosition);
        }

        private ChampionController[,] GenerateChampionGrid(Vector2Int gridSize)
            => new ChampionController[gridSize.x, gridSize.y];

        public void RotateBoard(bool isInverted)
        {
            GridVisualizer.RotateGrid(isInverted);
            RotateChampions(isInverted);
        }

        private void RotateChampions(bool isInverted)
        {
            var currentRotation = _championParent.transform.eulerAngles;
            currentRotation.z = (isInverted ? 180f : 0);

            _championParent.transform.eulerAngles = currentRotation;

            foreach (var champion in _championParent.GetComponentsInChildren<ChampionController>())
            {
                var championRotation = champion.transform.eulerAngles;
                championRotation.z = 0;

                champion.transform.eulerAngles = championRotation;
            }
        }
    }
}
