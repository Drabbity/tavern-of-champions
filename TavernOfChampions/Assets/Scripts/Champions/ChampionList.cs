using System.Collections.Generic;
using TavernOfChampions.Champion;
using UnityEngine;

namespace TavernOfChampions.Champion
{
    [CreateAssetMenu(fileName = "NewChampionList", menuName = "ChampionList")]
    public class ChampionList : ScriptableObject
    {
        [field: SerializeField] public SerializableStringDictionary<ChampionController> Champions { get; private set; }
    }
}
