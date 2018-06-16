using Rondo.Generic.Utility;
using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.Reputation {

    public class ReputationUI : MonoBehaviourSingleton<ReputationUI> {

        public ReputationInstanceUI reputationInstancePrefab;
        public ReputationHeroInstanceUI heroInstancePrefab;
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
            ReputationInstanceUI newInstance = Instantiate(reputationInstancePrefab);
            newInstance.GetComponent<RectTransform>().SetParent(reputationInstanceParent);
            newInstance.ApplyReputation(tracker);

            for (int i = 0; i < Random.Range(3, 6); i++) {
                newInstance.AddHero(heroInstancePrefab, HeroManager.GetRandomHero());
            }
        }

    }

}