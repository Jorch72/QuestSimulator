using Rondo.Generic.Utility;
using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Quests.Rewards;
using Rondo.QuestSim.Reputation;
using Rondo.QuestSim.UI.General;
using Rondo.QuestSim.UI.Reputation;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.PostedQuests {

    public class PostedQuestWindow : MonoBehaviourSingleton<PostedQuestWindow> {

        public TextMeshProUGUI questTitle;
        public TextMeshProUGUI difficultyText;
        public TextMeshProUGUI goldRewardText;
        public GameItemInstanceUI itemInstanceReference;
        public ReputationHeroInstanceUI heroInstanceReference;
        public Button closeButton;
        public Button skipButton;
        public Button cancelButton;
        public Button acceptButton;

        public Action OnWindowClose = delegate { };

        private QuestInstance m_CurrentQuest;
        private List<HeroInstance> m_AvailableHeroes = new List<HeroInstance>();
        private bool m_WatchOnlyMode = false;
        private List<GameObject> m_ItemsToDelete = new List<GameObject>();
        private HeroInstance m_SelectedHero = null;

        void Start() {
            closeButton.onClick.AddListener(() => {
                CloseWindow();
            });

            skipButton.onClick.AddListener(() => {
                CloseWindow();
            });

            cancelButton.onClick.AddListener(() => {
                QuestManager.PostedQuests.Remove(m_CurrentQuest);
                QuestManager.Requests.Add(m_CurrentQuest);
                CloseWindow();
            });


            acceptButton.onClick.AddListener(() => {
                if (m_SelectedHero == null) return;
                QuestManager.PostedQuests.Remove(m_CurrentQuest);
                QuestManager.ActiveQuests.Add(m_CurrentQuest, m_SelectedHero);
                CloseWindow();
            });


            Instance = this;
            gameObject.SetActive(false);
        }

        private void CloseWindow() {
            gameObject.SetActive(false);
            OnWindowClose();
        }

        public void OpenWindow(QuestInstance quest, bool watchOnlyMode) {
            m_WatchOnlyMode = watchOnlyMode;
            Reset();
            if (quest == m_CurrentQuest) {
                gameObject.SetActive(!gameObject.activeSelf);
            } else {
                gameObject.SetActive(true);
            }
            m_CurrentQuest = quest;

            if (m_WatchOnlyMode) {
                closeButton.gameObject.SetActive(true);
                skipButton.gameObject.SetActive(false);
                cancelButton.gameObject.SetActive(true);
                acceptButton.gameObject.SetActive(false);
            } else {
                closeButton.gameObject.SetActive(false);
                skipButton.gameObject.SetActive(true);
                cancelButton.gameObject.SetActive(true);
                acceptButton.gameObject.SetActive(true);
            }


            questTitle.text = "<b><u>" + m_CurrentQuest.QuestSource.RequestTitle + "</u></b>\n<size=18><i>" + m_CurrentQuest.ObjectiveCount + " Objective(s)</i></size>";
            difficultyText.text = ""+ m_CurrentQuest.DifficultyLevel;
            goldRewardText.text = ""+m_CurrentQuest.GoldReward.GoldCount;

            foreach (QuestRewardItem itemReward in m_CurrentQuest.ItemRewards) {
                GameItemInstanceUI newInstance = Instantiate(itemInstanceReference);
                newInstance.SetItem(itemReward.Item);
                newInstance.transform.SetParent(itemInstanceReference.transform.parent);
                newInstance.gameObject.SetActive(true);
                newInstance.transform.SetAsLastSibling();
                m_ItemsToDelete.Add(newInstance.gameObject);
            }

            if (m_WatchOnlyMode) {
                heroInstanceReference.transform.parent.gameObject.SetActive(false);
                return;
            } else {
                heroInstanceReference.transform.parent.gameObject.SetActive(true);
            }

            foreach (HeroInstance hero in HeroManager.GetAllHeroes()) {
                if (quest.WouldHeroAccept(hero)) m_AvailableHeroes.Add(hero);
            }

            foreach (HeroInstance hero in m_AvailableHeroes) {
                ReputationHeroInstanceUI newInstance = Instantiate(heroInstanceReference);
                newInstance.ApplyHero(hero);
                newInstance.transform.SetParent(heroInstanceReference.transform.parent);
                newInstance.gameObject.SetActive(true);
                newInstance.transform.SetAsLastSibling();
                m_ItemsToDelete.Add(newInstance.gameObject);
            }
        }

        public void Reset() {
            m_AvailableHeroes.Clear();

            foreach(GameObject obj in m_ItemsToDelete) {
                Destroy(obj);
            }
            m_ItemsToDelete.Clear();
        }

    }

}