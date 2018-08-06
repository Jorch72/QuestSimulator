using Rondo.Generic.Utility;
using Rondo.QuestSim.Facilities;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.UI.General;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.Blacksmith {

    public class BlacksmithWindow : MonoBehaviourSingleton<BlacksmithWindow> {

        public BlacksmithItem blacksmithItemTemplate;
        public RectTransform buyItemsParent;
        public RectTransform contractsParent;

        private RectTransform m_RectTransform;

        private void Awake() {
            Instance = this;

            m_RectTransform = GetComponent<RectTransform>();

            gameObject.SetActive(false);
        }

        private void OnEnable() {
            FillBuyItems();
        }

        private void FillBuyItems() {
            DeleteInstancesFromParent(buyItemsParent);

            foreach (GameItem item in BlacksmithManager.ItemsOnSale) {
                BlacksmithItem newInstance = Instantiate(blacksmithItemTemplate);
                newInstance.transform.SetParent(buyItemsParent);
                newInstance.gameObject.SetActive(true);
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