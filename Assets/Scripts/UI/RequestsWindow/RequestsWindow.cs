using Rondo.Generic.Utility;
using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Quests.Sources;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.Requests {

    public class RequestsWindow : MonoBehaviourSingleton<RequestsWindow> {

        public RequestInstance requestPrefab;
        public RectTransform requestInstanceParent;
        public Button openCloseToggle;
        public RequestPostWindow postWindow;

        private RectTransform m_RectTransform;

        private void Awake() {
            m_RectTransform = GetComponent<RectTransform>();

            openCloseToggle.onClick.AddListener(ToggleOpenCloseState);
            gameObject.SetActive(false);
        }

        private void OnEnable() {
            ReloadInstances();
        }

        private void ToggleOpenCloseState() {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        public void ReloadInstances() {
            bool isFirst = true;
            foreach(RectTransform child in requestInstanceParent) {
                if (isFirst) {
                    isFirst = false;
                    continue;
                }
                Destroy(child.gameObject);
            }

            foreach(QuestInstance request in QuestManager.Requests) {
                AddRequest(request);
            }
        }

        public void AddRequest(QuestInstance questInstance) {
            RequestInstance newInstance = Instantiate(requestPrefab);
            newInstance.GetComponent<RectTransform>().SetParent(requestInstanceParent);
            newInstance.ApplyQuestChain(questInstance);
        }

    }

}