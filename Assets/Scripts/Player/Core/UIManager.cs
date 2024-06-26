using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Class handles movement, actions, and main references to other systems
namespace Player
{
    public class UIManager : MonoBehaviour
    {
        #region Fields

        [Header("UI References")]
        public GameObject _mainInvWholeCon;
        public GameObject _mainInvHalfCon;
        public GameObject _mainSelectionMenu;
        public GameObject _inventoryUI;
        public GameObject _craftingUI;
        public GameObject _equipmentUI;
        public GameObject _pauseMenuUI;

        public Button _craftBtn;
        public Button _inventoryBtn;
        public Button _equipBtn;

        [Header("States")]
        public bool isAnyUIOpen = false;
        public bool isInventoryOpen = false;
        public bool isCraftingOpen = false;
        public bool isEquipmentOpen = false;
        public bool isPauseMenuOpen = false;

        #endregion

        #region Main

        // Update is called once per frame
        public void HandleUIInteractions()
        {
            KeypressChecks();
            StateCheck();
        }

        #endregion

        #region Open Menus
        /// <summary>
        /// Method for opening the inventory UI
        /// </summary>
        protected void OpenInventory()
        {
            if (_inventoryUI == null) { return; }

            bool isOpen = _inventoryUI.activeSelf;

            isInventoryOpen = !isOpen;

            SetInventoryContainer(isInventoryOpen, false);

            if (isInventoryOpen) { HideInactive(true, false, false); }
            else { _inventoryUI.SetActive(false); }
        }
        /// <summary>
        /// Method for opening the quick crafting UI
        /// </summary>
        protected void OpenCraftingMenu()
        {
            if (_craftingUI == null) { return; }

            bool isOpen = _craftingUI.activeSelf;

            isCraftingOpen = !isOpen;

            SetInventoryContainer(false, isCraftingOpen);

            if (isCraftingOpen) { HideInactive(false, true, false); }
            else { _craftingUI.SetActive(false); }
        }
        /// <summary>
        /// Method for opening the quick crafting UI
        /// </summary>
        protected void OpenEquipMenu()
        {
            if (_equipmentUI == null) { return; }

            bool isOpen = _equipmentUI.activeSelf;

            isEquipmentOpen = !isOpen;

            SetInventoryContainer(false, isEquipmentOpen);

            if (isEquipmentOpen) { HideInactive(false, false, true); }
            else { _equipmentUI.SetActive(false); }
        }
        /// <summary>
        /// Method for opening the pause menu
        /// </summary>
        protected void OpenPauseMenu()
        {
            if(_pauseMenuUI == null) { return; }

            bool isOpen = _pauseMenuUI.activeSelf;

            isPauseMenuOpen = !isOpen;

            _pauseMenuUI.SetActive(isPauseMenuOpen);
        }
        #endregion

        #region Checks
        public void StateCheck()
        {
            if(!_inventoryUI || !_equipmentUI || !_craftingUI) { return; }

            isInventoryOpen = _inventoryUI.activeSelf;
            isCraftingOpen = _craftingUI.activeSelf;
            isEquipmentOpen = _equipmentUI.activeSelf;

            isAnyUIOpen = isInventoryOpen || isCraftingOpen || isEquipmentOpen;

            if (isCraftingOpen) { _craftBtn.Select(); }
            if (isInventoryOpen) { _inventoryBtn.Select(); }
            if (isEquipmentOpen) { _equipBtn.Select(); }

            if(isInventoryOpen || isCraftingOpen || isEquipmentOpen)
            {
                _mainSelectionMenu.SetActive(true);
            }
            else { _mainSelectionMenu.SetActive(false); }
        }

        public void KeypressChecks()
        {
            if (Keyboard.current.iKey.wasReleasedThisFrame)
            {
                //Debug.Log("I Pressed");
                OpenInventory();
            }

            if (Keyboard.current.cKey.wasReleasedThisFrame)
            {
                //Debug.Log("C Pressed");
                OpenCraftingMenu();
            }

            if (Keyboard.current.eKey.wasReleasedThisFrame)
            {
                //Debug.Log("E Pressed");
                OpenEquipMenu();
            }

            if (Keyboard.current.escapeKey.wasReleasedThisFrame)
            {
                if (isAnyUIOpen) { CloseAnyUI(); }
                else { OpenPauseMenu(); }
            }
        }
        #endregion

        #region Helpers
        public void SetInventoryContainer(bool wholeState, bool halfState)
        {
            _mainInvWholeCon.SetActive(wholeState);
            _mainInvHalfCon.SetActive(halfState);
        }
        public void HideInactive(bool inv, bool craft, bool equip)
        {
            _inventoryUI.SetActive(inv);
            _craftingUI.SetActive(craft);
            _equipmentUI.SetActive(equip);
        }
        public void CloseAnyUI()
        {
            HideInactive(false, false, false);
            SetInventoryContainer(false, false);
        }
        #endregion
    }
}
