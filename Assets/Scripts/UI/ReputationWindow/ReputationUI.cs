using Rondo.Generic.Utility;
using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Reputation;
using System;
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
        private Dictionary<HeroInstance, ReputationHeroInstanceUI> m_HeroInstances = new Dictionary<HeroInstance, ReputationHeroInstanceUI>();
        private Action<HeroInstance> m_OnHeroClicked;

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

            foreach(HeroInstance hero in tracker.FactionInstance.Heroes) {
                ReputationHeroInstanceUI heroInstanceUI = newInstance.AddHero(heroInstancePrefab, hero);
                m_HeroInstances.Add(hero, heroInstanceUI);
                heroInstanceUI.GetComponent<Button>().onClick.AddListener(() => { OnHeroClick(heroInstanceUI); });
            }
        }

        public void SetAvailableHeroes(List<HeroInstance> heroes, Action<HeroInstance> onHeroClick) {
            foreach(HeroInstance hero in m_HeroInstances.Keys) {
                m_HeroInstances[hero].SetAlpha(heroes.Contains(hero) ? 1 : 0.5f);
            }

            m_OnHeroClicked = onHeroClick;
        }

        public void ResetAvailableHeroes() {
            foreach (HeroInstance hero in m_HeroInstances.Keys) {
                m_HeroInstances[hero].SetAlpha(1);
            }
        }

        private void OnHeroClick(ReputationHeroInstanceUI instance) {
            if (m_OnHeroClicked == null) return;
            m_OnHeroClicked(instance.Hero);
        }

    }

}