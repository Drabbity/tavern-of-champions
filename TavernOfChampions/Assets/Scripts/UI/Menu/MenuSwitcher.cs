using System;
using UnityEngine;

namespace TavernOfChampions.UI.Menu
{
    public class MenuSwitcher : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _roomMenu;

        public void SwitchMenu(MenuType menuType)
        {
            switch(menuType)
            {
                case MenuType.MAIN_MENU:
                    _mainMenu.SetActive(true);
                    _roomMenu.SetActive(false);
                    break;

                case MenuType.ROOM_MENU:
                    _mainMenu.SetActive(false);
                    _roomMenu.SetActive(true);
                    break;

                default:
                    throw new NotImplementedException($"Code for MenuType { menuType } does not exist");
            }
        }
    }

    public enum MenuType
    {
        MAIN_MENU,
        ROOM_MENU,
    }
}
