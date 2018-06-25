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

    public class QuestDetailsWindow : MonoBehaviourSingleton<QuestDetailsWindow> {

        public TextMeshProUGUI questTitle;
        public TextMeshProUGUI difficultyText;
        public Button closeButton;
        public Button skipButton;
        public Button cancelButton;
        public Button acceptButton;
        public Button completeButton;
        public Button postButton;

        [Header("Mode objects")]
        [Header("Heroes")]
        public Button heroSelectionInstance;
        public RectTransform heroSelectedInstance;

        [Header("Item rewards")]
        public TMP_Dropdown heroRewardItemDropdown;
        public GameItemInstanceUI heroRewardItemInstance;
        public GameItemInstanceUI handlerRewardItemInstance;

        [Header("Gold rewards")]
        public TMP_InputField heroGoldRewardInput;
        public TextMeshProUGUI heroGoldRewardText;
        public TextMeshProUGUI handlerGoldReward;

        [Header("Additional rewards")]
        public TextMeshProUGUI handlerAdditionalReward;

        public Action OnWindowClose = delegate { };

        private QuestInstance m_CurrentQuest;
        private List<HeroInstance> m_AvailableHeroes = new List<HeroInstance>();
        private QuestMode m_WindowMode = QuestMode.ACTIVE_REVIEW;
        private List<GameObject> m_ItemsToDelete = new List<GameObject>();
        private HeroInstance m_SelectedHero = null;
        private int m_SelectedItemReward = 0;

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
                QuestManager.PostedQuests.Remove(m_CurrentQuest);
                QuestManager.ActiveQuests.Add(m_CurrentQuest, m_SelectedHero);
                CloseWindow();
            });

            completeButton.onClick.AddListener(() => {
                QuestManager.ActiveQuests.Remove(m_CurrentQuest);
                CloseWindow();
            });

            postButton.onClick.AddListener(() => {
                QuestManager.Requests.Remove(m_CurrentQuest);
                QuestManager.PostedQuests.Add(m_CurrentQuest);

                InventoryManager.Gold -= m_CurrentQuest.GoldReward.GoldCount;
                if (m_SelectedItemReward != 0) {
                    GameItem item = InventoryManager.OwnedItems[m_SelectedItemReward - 1];
                    m_CurrentQuest.ItemReward = new QuestRewardItem(item);
                    InventoryManager.MoveItemToReserved(item);
                }

                CloseWindow();
            });

            heroGoldRewardInput.onValueChanged.AddListener((value) => {
                if (string.IsNullOrEmpty(value)) {
                    value = "0";
                }
                int goldValue = int.Parse(value);
                m_CurrentQuest.GoldReward.GoldCount = goldValue;

                CheckPostButtonStatus();
            });

            heroGoldRewardInput.onDeselect.AddListener((value) => {
                if (string.IsNullOrEmpty(value)) {
                    heroGoldRewardInput.text = "0";
                }
            });

            heroRewardItemDropdown.onValueChanged.AddListener((value) => {
                m_SelectedItemReward = value;

                if (m_SelectedItemReward != 0) {
                    heroRewardItemDropdown.GetComponent<GameItemPopupCaller>().associatedItem = InventoryManager.OwnedItems[m_SelectedItemReward - 1];
                } else {
                    heroRewardItemDropdown.GetComponent<GameItemPopupCaller>().associatedItem = null;
                }
            });

            heroSelectionInstance.onClick.AddListener(() => {
                ReputationUI.Instance.gameObject.SetActive(true);
            });

            Instance = this;
            gameObject.SetActive(false);
        }

        private void CloseWindow() {
            gameObject.SetActive(false);
            ReputationUI.Instance.ResetAvailableHeroes();
            OnWindowClose();
        }

        public void OpenWindow(QuestInstance quest, QuestMode mode) {
            m_WindowMode = mode;
            Reset();
            if (quest == m_CurrentQuest) {
                gameObject.SetActive(!gameObject.activeSelf);
            } else {
                gameObject.SetActive(true);
            }
            m_CurrentQuest = quest;

            switch (mode) {
                case QuestMode.SETUP:
                    closeButton.gameObject.SetActive(false);
                    skipButton.gameObject.SetActive(false);
                    cancelButton.gameObject.SetActive(true);
                    acceptButton.gameObject.SetActive(false);
                    completeButton.gameObject.SetActive(false);
                    postButton.gameObject.SetActive(true);

                    RefreshItemRewardDropdown();
                    SetNoHero();
                    break;
                case QuestMode.HERO_SELECT:
                    closeButton.gameObject.SetActive(false);
                    skipButton.gameObject.SetActive(true);
                    cancelButton.gameObject.SetActive(true);
                    acceptButton.gameObject.SetActive(true);
                    completeButton.gameObject.SetActive(false);
                    postButton.gameObject.SetActive(false);

                    FindPotentialHeroes();
                    break;
                case QuestMode.POSTED_REVIEW:
                    closeButton.gameObject.SetActive(true);
                    skipButton.gameObject.SetActive(false);
                    cancelButton.gameObject.SetActive(true);
                    acceptButton.gameObject.SetActive(false);
                    completeButton.gameObject.SetActive(false);
                    postButton.gameObject.SetActive(false);

                    SetNoHero();
                    break;
                case QuestMode.ACTIVE_REVIEW:
                    closeButton.gameObject.SetActive(true);
                    skipButton.gameObject.SetActive(false);
                    cancelButton.gameObject.SetActive(true);
                    acceptButton.gameObject.SetActive(false);
                    completeButton.gameObject.SetActive(false);
                    postButton.gameObject.SetActive(false);

                    FindActiveHero();
                    break;
                case QuestMode.COMPLETED:
                    closeButton.gameObject.SetActive(false);
                    skipButton.gameObject.SetActive(false);
                    cancelButton.gameObject.SetActive(false);
                    acceptButton.gameObject.SetActive(false);
                    completeButton.gameObject.SetActive(true);
                    postButton.gameObject.SetActive(false);

                    FindActiveHero();
                    break;
            }

            heroRewardItemDropdown.gameObject.SetActive(mode == QuestMode.SETUP);
            heroRewardItemInstance.gameObject.SetActive(mode != QuestMode.SETUP);

            heroGoldRewardInput.gameObject.SetActive(mode == QuestMode.SETUP);
            heroGoldRewardText.gameObject.SetActive(mode != QuestMode.SETUP);

            heroSelectionInstance.gameObject.SetActive(mode == QuestMode.HERO_SELECT);
            heroSelectedInstance.gameObject.SetActive(mode != QuestMode.HERO_SELECT);

            questTitle.text = "<b><u>" + m_CurrentQuest.QuestSource.RequestTitle + "</u></b>\n<size=18><i>" + m_CurrentQuest.ObjectiveCount + " Objective" + (m_CurrentQuest.ObjectiveCount > 1 ? "s" : "") + "</i></size>";
            difficultyText.text = ""+ m_CurrentQuest.DifficultyLevel;
            heroGoldRewardText.text = ""+m_CurrentQuest.GoldReward.GoldCount;
            heroRewardItemInstance.SetItem(m_CurrentQuest.ItemReward);
            heroGoldRewardInput.text = "0";

            handlerGoldReward.text = m_CurrentQuest.HandlerGoldRewardEstimate;
            handlerRewardItemInstance.SetItem((GameItem)null);
            handlerAdditionalReward.text = m_CurrentQuest.AdditionalReward != null ? m_CurrentQuest.AdditionalReward.DisplayValue : "-";

        }

        private void FindPotentialHeroes() {
            foreach (HeroInstance hero in HeroManager.GetAvailableHeroes()) {
                if (m_CurrentQuest.WouldHeroAccept(hero)) {
                    m_AvailableHeroes.Add(hero);
                }
            }

            ReputationUI.Instance.SetAvailableHeroes(m_AvailableHeroes, SetSelectedHero);
        }

        private void SetSelectedHero(HeroInstance hero) {
            m_SelectedHero = hero;
            heroSelectionInstance.GetComponentInChildren<ReputationHeroInstanceUI>(true).ApplyHero(m_SelectedHero);
        }

        private void FindActiveHero() {
            heroSelectedInstance.GetComponentInChildren<ReputationHeroInstanceUI>(true).ApplyHero(QuestManager.ActiveQuests[m_CurrentQuest]);
        }

        private void SetNoHero() {
            heroSelectedInstance.GetComponentInChildren<ReputationHeroInstanceUI>(true).ApplyHero(null);
        }

        private void RefreshItemRewardDropdown() {
            List<string> itemRewardNames = new List<string>() { "-" };
            foreach(GameItem item in InventoryManager.OwnedItems) {
                itemRewardNames.Add(item.DisplayName);
            }

            heroRewardItemDropdown.ClearOptions();
            heroRewardItemDropdown.AddOptions(itemRewardNames);
            heroRewardItemDropdown.value = 0;
            heroRewardItemDropdown.RefreshShownValue();

            heroRewardItemDropdown.GetComponent<GameItemPopupCaller>().associatedItem = null;
        }

        public void SetItemInstancesOnDropdown() {
            GameItemPopupCaller[] itemInstances = heroRewardItemDropdown.GetComponentsInChildren<GameItemPopupCaller>();
            for (int i = 0; i < itemInstances.Length; i++) {
                if (i == 0 || i == 1) continue;
                itemInstances[i].associatedItem = (InventoryManager.OwnedItems[i - 2]);
            }
        }

        private void CheckPostButtonStatus() {
            bool isPostable = true;
            if (InventoryManager.Gold < m_CurrentQuest.GoldReward.GoldCount && InventoryManager.Gold >= 0) isPostable = false;
            postButton.interactable = isPostable;
        }

        private void CheckAcceptButtonStatus() {
            bool isAcceptable = true;
            if (m_SelectedHero == null) isAcceptable = false;
            acceptButton.interactable = isAcceptable;
        }

        public void Reset() {
            m_AvailableHeroes.Clear();

            foreach(GameObject obj in m_ItemsToDelete) {
                Destroy(obj);
            }
            m_ItemsToDelete.Clear();

            m_SelectedHero = null;
            m_SelectedItemReward = 0;
        }

        public enum QuestMode {
            SETUP,
            HERO_SELECT,
            POSTED_REVIEW,
            ACTIVE_REVIEW,
            COMPLETED
        }

    }

}