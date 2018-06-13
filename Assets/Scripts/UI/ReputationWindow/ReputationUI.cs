using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.Reputation {

    public class ReputationUI : MonoBehaviour {

        public ReputationInstanceUI instancePrefab;
        public RectTransform reputationInstanceParent;
        public Button openCloseToggle;

        private RectTransform m_RectTransform;

        private void Awake() {
            m_RectTransform = GetComponent<RectTransform>();

            openCloseToggle.onClick.AddListener(ToggleOpenCloseState);
            gameObject.SetActive(false);
        }

        private void ToggleOpenCloseState() {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        public void AddReputationTracker(ReputationTracker tracker) {
            ReputationInstanceUI newInstance = Instantiate(instancePrefab);
            newInstance.GetComponent<RectTransform>().SetParent(reputationInstanceParent);
            newInstance.ApplyReputation(tracker);
        }

    }

}