using UnityEngine;

namespace TavernOfChampions.Helper
{
    public static class MouseHelper
    {
        public static Vector3 GetMouseWorldPosition()
                => Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
