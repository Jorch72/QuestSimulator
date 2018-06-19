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

        [Header("Mode objects")]
        public GameObject heroSelectParent;
        public GameObject heroSelectedParent;

        public Action OnWindowClose = delegate { };

        private QuestInstance m_CurrentQuest;
        private List<HeroInstance> m_AvailableHeroes = new List<HeroInstance>();
        private PostedQuestMode m_WindowMode = PostedQuestMode.ACTIVE_REVIEW;
        private List<GameObject> m_ItemsToDelete = new List<GameObject>();
        private ReputationHeroInstanceUI m_SelectedHero = null;

        void Start() {
            closeButton.onClick.AddListener(() => {
                CloseWindow();
            });

            skipButton.onClick.AddListener(() => {
                CloseWindow();
            });

            cancelButton.onClick.AddListener(() => {
                m_CurrentQuest.RefundQuestRewards(true, true);
                QuestManager.PostedQuests.Remove(m_CurrentQuest);
                QuestManager.Requests.Add(m_CurrentQuest);
                CloseWindow();
            });


            acceptButton.onClick.AddListener(() => {
                if (m_SelectedHero == null) return;
                QuestManager.PostedQuests.Remove(m_CurrentQuest);
                QuestManager.ActiveQuests.Add(m_CurrentQuest, m_SelectedHero.Hero);
                CloseWindow();
            });


            Instance = this;
            gameObject.SetActive(false);
        }

        private void Update() {
            if (m_SelectedHero == null) return;
            float scale = Mathf.Sin(Time.time * 3).Map(-1, 1, 1, 1.1f);
            m_SelectedHero.GetComponent<RectTransform>().GetChild(0).localScale = new Vector3(scale, scale, scale);
        }

        private void CloseWindow() {
            gameObject.SetActive(false);
            OnWindowClose();
        }

        public void OpenWindow(QuestInstance quest, PostedQuestMode mode) {
            m_WindowMode = mode;
            Reset();
            if (quest == m_CurrentQuest) {
                gameObject.SetActive(!gameObject.activeSelf);
            } else {
                gameObject.SetActive(true);
            }
            m_CurrentQuest = quest;

            switch (mode) {
                case PostedQuestMode.HERO_SELECT:
                    closeButton.gameObject.SetActive(false);
                    skipButton.gameObject.SetActive(true);
                    cancelButton.gameObject.SetActive(true);
                    acceptButton.gameObject.SetActive(true);
                    break;
                case PostedQuestMode.POSTED_REVIEW:
                    closeButton.gameObject.SetActive(true);
                    skipButton.gameObject.SetActive(false);
                    cancelButton.gameObject.SetActive(true);
                    acceptButton.gameObject.SetActive(false);
                    break;
                case PostedQuestMode.ACTIVE_REVIEW:
                    closeButton.gameObject.SetActive(true);
                    skipButton.gameObject.SetActive(false);
                    cancelButton.gameObject.SetActive(true);
                    acceptButton.gameObject.SetActive(false);
                    break;
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

            switch (mode) {
                case PostedQuestMode.HERO_SELECT:
                    heroSelectParent.SetActive(true);
                    heroSelectedParent.SetActive(false);
                    FindPotentialHeroes();
                    break;
                case PostedQuestMode.ACTIVE_REVIEW:
                    heroSelectParent.SetActive(false);
                    heroSelectedParent.SetActive(true);
                    FindActiveHero();
                    break;
                case PostedQuestMode.POSTED_REVIEW:
                    heroSelectParent.SetActive(false);
                    heroSelectedParent.SetActive(false);
                    break;
            }
        }

        private void FindPotentialHeroes() {
            foreach (HeroInstance hero in HeroManager.GetAvailableHeroes()) {
                if (m_CurrentQuest.WouldHeroAccept(hero)) m_AvailableHeroes.Add(hero);
            }

            foreach (HeroInstance hero in m_AvailableHeroes) {
                ReputationHeroInstanceUI newInstance = Instantiate(heroInstanceReference);
                newInstance.ApplyHero(hero);
                newInstance.transform.SetParent(heroInstanceReference.transform.parent);
                newInstance.gameObject.SetActive(true);
                newInstance.transform.SetAsLastSibling();

                //Add button func
                newInstance.gameObject.AddComponent<Button>().onClick.AddListener(() => {
                    Vector2 delta;
                    if (m_SelectedHero != null) {
                        delta = m_SelectedHero.GetComponent<RectTransform>().sizeDelta;
                        m_SelectedHero.GetComponent<RectTransform>().sizeDelta = new Vector2(delta.x, 25);
                        m_SelectedHero.GetComponent<RectTransform>().GetChild(0).localScale = new Vector3(1, 1, 1);
                    }
                    m_SelectedHero = newInstance;

                    delta = m_SelectedHero.GetComponent<RectTransform>().sizeDelta;
                    m_SelectedHero.GetComponent<RectTransform>().sizeDelta = new Vector2(delta.x, 45);
                });

                m_ItemsToDelete.Add(newInstance.gameObject);
            }
        }

        private void FindActiveHero() {
            heroSelectedParent.GetComponentInChildren<ReputationHeroInstanceUI>().ApplyHero(QuestManager.ActiveQuests[m_CurrentQuest]);
        }

        public void Reset() {
            m_AvailableHeroes.Clear();

            foreach(GameObject obj in m_ItemsToDelete) {
                Destroy(obj);
            }
            m_ItemsToDelete.Clear();
        }

        public enum PostedQuestMode {
            HERO_SELECT,
            POSTED_REVIEW,
            ACTIVE_REVIEW
        }

    }

}