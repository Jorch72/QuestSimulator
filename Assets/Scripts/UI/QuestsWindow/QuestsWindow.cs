using Rondo.Generic.Utility;
using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Quests.Sources;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.ActiveQuests {

    public class QuestsWindow : MonoBehaviourSingleton<QuestsWindow> {

        public QuestInstanceUI instancePrefab;
        public RectTransform postedQuestsParent;
        public RectTransform activeQuestsParent;
        public RectTransform requestQuestsParent;
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
            DeleteInstancesFromParent(requestQuestsParent);

            foreach(QuestInstance quest in QuestManager.PostedQuests) {
                QuestInstanceUI newInstance = Instantiate(instancePrefab);
                newInstance.GetComponent<RectTransform>().SetParent(postedQuestsParent, false);
                newInstance.ApplyQuestChain(quest);
            }

            foreach (QuestInstance quest in QuestManager.ActiveQuests.Keys) {
                QuestInstanceUI newInstance = Instantiate(instancePrefab);
                newInstance.GetComponent<RectTransform>().SetParent(activeQuestsParent, false);
                newInstance.ApplyQuestChain(quest);
            }

            foreach (QuestInstance quest in QuestManager.Requests) {
                QuestInstanceUI newInstance = Instantiate(instancePrefab);
                newInstance.GetComponent<RectTransform>().SetParent(requestQuestsParent, false);
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