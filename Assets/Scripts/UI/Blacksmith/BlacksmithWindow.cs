using Rondo.Generic.Utility;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.UI.General;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.Blacksmith {

    public class BlacksmithWindow : MonoBehaviourSingleton<BlacksmithWindow> {

        public BlacksmithItem blacksmithItemTemplate;
        public RectTransform buyItemsParent;
        public RectTransform sellItemsParent;
        public RectTransform contractsParent;

        private RectTransform m_RectTransform;

        private void Awake() {
            Instance = this;

            m_RectTransform = GetComponent<RectTransform>();

            gameObject.SetActive(false);
        }

        private void OnEnable() {
            FillSellItems();
        }

        private void FillSellItems() {
            DeleteInstancesFromParent(sellItemsParent);

            foreach (GameItem item in InventoryManager.OwnedItems) {
                BlacksmithItem newInstance = Instantiate(blacksmithItemTemplate);
                newInstance.transform.SetParent(sellItemsParent);
                newInstance.gameObject.SetActive(true);
                newInstance.SetItem(item);
                newInstance.itemInstance.GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f, 1);
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