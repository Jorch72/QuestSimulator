using Rondo.Generic.Utility;
using Rondo.Generic.Utility.UI;
using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Quests.Rewards;
using Rondo.QuestSim.Reputation;
using Rondo.QuestSim.UI.ActiveQuests;
using Rondo.QuestSim.UI.General;
using Rondo.QuestSim.UI.Reputation;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.PostedQuests {

    public class QuestDetailsHero : MonoBehaviour {

        [Header("Heroes")]
        public ReputationHeroInstanceUI heroSelectedInstance;
        public Button heroSelectButton;

        [Header("Item rewards")]
        public TMP_Dropdown heroRewardItemDropdown;
        public GameItemInstanceUI heroRewardItemInstance;

        [Header("Gold rewards")]
        public TMP_InputField heroGoldRewardInput;
        public TextMeshProUGUI heroGoldRewardText;

        [Header("Highlight lists")]
        public Graphic[] highlightsRewards;
        public Graphic[] highlightsHeroSelect;

        public List<HeroInstance> AvailableHeroes { get; set; }

        private QuestInstance CurrentQuest { get { return QuestDetailsWindow.Instance.CurrentQuest; } }
        private QuestDetailsWindowMode WindowMode { get { return QuestDetailsWindow.Instance.WindowMode; } }

        private HeroInstance m_SelectedHero = null;
        private int m_SelectedItemReward = 0;
        private int m_HeroNumber = 0;

        private void Awake() {
            AvailableHeroes = new List<HeroInstance>();
        }

        void Start() {
            heroGoldRewardInput.onValueChanged.AddListener((value) => {
                if (string.IsNullOrEmpty(value)) {
                    value = "0";
                }
                int goldValue = int.Parse(value);
                QuestDetailsWindow.Instance.SelectedGoldRewards[m_HeroNumber] = goldValue;

                CheckPostButtonStatus();
            });

            heroGoldRewardInput.onDeselect.AddListener((value) => {
                if (string.IsNullOrEmpty(value)) {
                    heroGoldRewardInput.text = "0";
                }
            });

            heroRewardItemDropdown.onValueChanged.AddListener((value) => {
                m_SelectedItemReward = value;
                GameItem item = null;

                if (m_SelectedItemReward != 0) {
                    item = InventoryManager.OwnedItems[m_SelectedItemReward - 1];
                }

                heroRewardItemDropdown.GetComponent<GameItemPopupCaller>().associatedItem = item;

                QuestDetailsWindow.Instance.SelectedItemRewards[m_HeroNumber] = item;

                CheckPostButtonStatus();
            });

            heroSelectButton.onClick.AddListener(() => {
                AvailableHeroes.Clear();
                foreach (HeroInstance hero in HeroManager.GetAvailableHeroes()) {
                    if (CurrentQuest.WouldHeroAccept(hero, m_HeroNumber) &&
                        !QuestDetailsWindow.Instance.HasHeroSelected(hero)) {
                        AvailableHeroes.Add(hero);
                    }
                }

                RightSideSwitch.Instance.ActivateObject(ReputationUI.Instance.gameObject, false);
                ReputationUI.Instance.SetAvailableHeroes(AvailableHeroes, SetSelectedHero);
            });
        }

        public void Initialize(int heroNumber) {
            m_HeroNumber = heroNumber;
        }

        public void Reload() {
            switch (WindowMode) {
                case QuestDetailsWindowMode.SETUP:
                    RefreshItemRewardDropdown();
                    SetNoHero();
                    UIHighlighter.Instance.GetGroup(QuestDetailsWindow.HIGHLIGHT_GROUP_ID).AddObjects(highlightsRewards, UIHighlighter.Instance.redHighlightColor, highlightsRewards[0].color);
                    break;
                case QuestDetailsWindowMode.HERO_SELECT:
                    UIHighlighter.Instance.GetGroup(QuestDetailsWindow.HIGHLIGHT_GROUP_ID).AddObjects(highlightsHeroSelect, UIHighlighter.Instance.redHighlightColor, highlightsHeroSelect[0].color);
                    break;
                case QuestDetailsWindowMode.POSTED_REVIEW:
                    SetNoHero();
                    break;
                case QuestDetailsWindowMode.ACTIVE_REVIEW:
                    FindActiveHero();
                    break;
                case QuestDetailsWindowMode.COMPLETED:
                    FindActiveHero();
                    break;
            }

            heroRewardItemDropdown.gameObject.SetActive(WindowMode == QuestDetailsWindowMode.SETUP);
            heroRewardItemInstance.gameObject.SetActive(WindowMode != QuestDetailsWindowMode.SETUP);

            heroGoldRewardInput.gameObject.SetActive(WindowMode == QuestDetailsWindowMode.SETUP);
            heroGoldRewardText.gameObject.SetActive(WindowMode != QuestDetailsWindowMode.SETUP);

            heroSelectButton.enabled = WindowMode == QuestDetailsWindowMode.HERO_SELECT;

            heroGoldRewardText.text = ""+CurrentQuest.GoldRewards[m_HeroNumber].GoldCount;
            heroRewardItemInstance.SetItem(CurrentQuest.ItemRewards[m_HeroNumber]);
            if(heroGoldRewardInput.gameObject.activeSelf) heroGoldRewardInput.text = "0";

            CheckAcceptButtonStatus();
            CheckPostButtonStatus();
        }

        public void SetSelectedHero(HeroInstance hero) {
            heroSelectedInstance.ApplyHero(hero);
            m_SelectedHero = hero;

            QuestDetailsWindow.Instance.SetSelectedHero(hero, m_HeroNumber);

            UIHighlighter.Instance.GetGroup(QuestDetailsWindow.HIGHLIGHT_GROUP_ID).RemoveObjects(highlightsHeroSelect);
        }

        public void FindActiveHero() {
            HeroInstance hero = QuestManager.ActiveQuests[CurrentQuest][m_HeroNumber];
            heroSelectedInstance.ApplyHero(hero);
        }

        public void SetNoHero() {
            heroSelectedInstance.ApplyHero(null);
        }

        private void RefreshItemRewardDropdown(bool retainSelection = false) {
            int selection = 0;

            List<string> itemRewardNames = new List<string>() { "-" };
            foreach(GameItem item in InventoryManager.OwnedItems) {
                itemRewardNames.Add(item.DisplayName);
            }

            heroRewardItemDropdown.ClearOptions();
            heroRewardItemDropdown.AddOptions(itemRewardNames);
            heroRewardItemDropdown.value = selection;
            heroRewardItemDropdown.RefreshShownValue();

            heroRewardItemDropdown.GetComponent<GameItemPopupCaller>().associatedItem = null;
        }

        public void SetItemInstancesOnDropdown() {
            GameItemPopupCaller[] itemInstances = heroRewardItemDropdown.GetComponentsInChildren<GameItemPopupCaller>();
            for (int i = 0; i < itemInstances.Length; i++) {
                if (i == 0 || i == 1) continue;
                GameItem item = (InventoryManager.OwnedItems[i - 2]);
                itemInstances[i].associatedItem = item;
                itemInstances[i].transform.GetChild(3).GetComponent<Image>().sprite = item.GetIcon();
            }
        }

        private void CheckPostButtonStatus() {
            bool isPostable = true;
            if ((InventoryManager.Gold < CurrentQuest.GetTotalGoldCount() && InventoryManager.Gold >= 0) ||
                (QuestDetailsWindow.Instance.SelectedGoldRewards[m_HeroNumber] == 0 && m_SelectedItemReward == 0)) isPostable = false;

            QuestDetailsWindow.Instance.PostButtonStatuses[m_HeroNumber] = isPostable;

            if (isPostable) {
                UIHighlighter.Instance.GetGroup(QuestDetailsWindow.HIGHLIGHT_GROUP_ID).RemoveObjects(highlightsRewards);
            } else {
                UIHighlighter.Instance.GetGroup(QuestDetailsWindow.HIGHLIGHT_GROUP_ID).AddObjects(highlightsRewards, UIHighlighter.Instance.redHighlightColor, highlightsRewards[0].color);
            }

            QuestDetailsWindow.Instance.CheckPostButtonStatus();
        }

        private void CheckAcceptButtonStatus() {
            bool isAcceptable = true;
            if (m_SelectedHero == null) isAcceptable = false;

            QuestDetailsWindow.Instance.AcceptButtonStatuses[m_HeroNumber] = isAcceptable;
        }

    }

}