using UnityEngine;
using UnityEngine.EventSystems;

namespace TavernOfChampions.UI
{
    public class UIHoverListener : MonoBehaviour
    {
        public bool IsPointerOverUI()
            => EventSystem.current.IsPointerOverGameObject();
    }
}
