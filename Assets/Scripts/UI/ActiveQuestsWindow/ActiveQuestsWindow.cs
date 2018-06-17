using Rondo.Generic.Utility;
using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Quests.Sources;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.ActiveQuests {

    public class ActiveQuestsWindow : MonoBehaviourSingleton<ActiveQuestsWindow> {

        public ActiveQuestInstance instancePrefab;
        public RectTransform postedQuestsParent;
        public RectTransform activeQuestsParent;
        public Button openCloseToggle;

        private RectTransform m_RectTransform;

        private void Awake() {
            m_RectTransform = GetComponent<RectTransform>();

            openCloseToggle.onClick.AddListener(ToggleOpenCloseState);
            gameObject.SetActive(false);
        }

        private void OnEnable() {
            Reload();
        }

        private void ToggleOpenCloseState() {
            gameObject.SetActive(!gameObject.activeSelf);
        }


        public void Reload() {
            DeleteInstancesFromParent(postedQuestsParent);
            DeleteInstancesFromParent(activeQuestsParent);

            foreach(QuestInstance quest in QuestManager.PostedQuests) {
                ActiveQuestInstance newInstance = Instantiate(instancePrefab);
                newInstance.GetComponent<RectTransform>().SetParent(postedQuestsParent);
                newInstance.ApplyQuestChain(quest);
            }

            foreach (QuestInstance quest in QuestManager.ActiveQuests.Keys) {
                ActiveQuestInstance newInstance = Instantiate(instancePrefab);
                newInstance.GetComponent<RectTransform>().SetParent(activeQuestsParent);
                newInstance.ApplyQuestChain(quest);
            }
        }

        private void DeleteInstancesFromParent(RectTransform parent) {
            bool skipFirst = true;
            foreach(RectTransform child in parent) {
                if (skipFirst) {
                    skipFirst = false;
                    continue;
                }

                Destroy(child.gameObject);
            }
        }

    }

}