using Rondo.Generic.Utility;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.UI.General;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.Inventory {

    public class InventoryWindow : MonoBehaviourSingleton<InventoryWindow> {

        public GameItemInstanceUI itemPrefab;
        public RectTransform availableItemsParent;
        public RectTransform reservedItemsParent;
        public Button openCloseToggle;

        private RectTransform m_RectTransform;

        private void Awake() {
            Instance = this;

            m_RectTransform = GetComponent<RectTransform>();

            gameObject.SetActive(false);

            openCloseToggle.onClick.AddListener(ToggleOpenCloseState);
        }

        private void ToggleOpenCloseState() {
            RightSideSwitch.Instance.ActivateObject(gameObject);
        }

        private void OnEnable() {
            FillAvailableItems();
            FillReservedItems();
        }

        private void FillAvailableItems() {
            AddItemsFromListToParent(InventoryManager.OwnedItems, availableItemsParent);
        }

        private void FillReservedItems() {
            AddItemsFromListToParent(InventoryManager.ReservedItems, reservedItemsParent);
        }

        private void AddItemsFromListToParent(List<GameItem> itemList, RectTransform parent) {
            DeleteInstancesFromParent(parent);

            itemList = ItemUtility.SortByRarity(itemList);

            foreach (GameItem item in itemList) {
                GameItemInstanceUI newInstance = Instantiate(itemPrefab);
                newInstance.transform.SetParent(parent);
                newInstance.SetItem(item);
            }
        }

        private void DeleteInstancesFromParent(RectTransform parent) {
            bool skipFirst = true;
            foreach (RectTransform child in parent) {
                if (skipFirst) {
                    skipFirst = false;
                    continue;
                }

                Destroy(child.gameObject);
            }
        }
    }

}