using Rondo.Generic.Utility;
using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.Quests.Rewards;
using Rondo.QuestSim.Quests.Sources;
using Rondo.QuestSim.Reputation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Quests {

    public class QuestInstance {

        private static float HANDLER_GOLD_VARIANCE_MIN = 0.8f;
        private static float HANDLER_GOLD_VARIANCE_MAX = 1.2f;

        public string DisplayName { get; set; }
        public int DurationInDays { get { return m_DurationInDays; } set { m_DurationInDays = value; DaysLeftOnQuest = value; } }
        public IQuestSource QuestSource { get; private set; }
        public QuestTypes QuestType { get; set; }
        public string QuestTypeDisplay { get { return QuestType.ToString().Replace('_', ' '); } }
        public int DifficultyLevel { get; set; }
        public QuestRewardGold GoldReward { get; set; }
        public QuestRewardItem ItemReward { get; set; }
        public IQuestReward AdditionalReward { get; set; }
        public QuestRewardItem HandlerItemReward { get; set; }
        public int DaysLeftOnPost { get; set; }
        public int DaysLeftOnQuest { get { return m_DaysLeftOnQuest; } set { m_DaysLeftOnQuest = value; OnDaysLeftUpdate(); } }
        public string HandlerGoldRewardEstimate { get { return Mathf.RoundToInt(HandlerAverageExpectedGoldReward * HANDLER_GOLD_VARIANCE_MIN) + " - " + Mathf.RoundToInt(HandlerAverageExpectedGoldReward * HANDLER_GOLD_VARIANCE_MAX); } }
        public int AveragePowerLevel { get { return ((DifficultyLevel * HeroInstance.LEVELS_PER_QUEST_STAR + 1) * HeroInstance.BASE_POWER_PER_LEVEL); } }

        public Action OnDaysLeftUpdate = delegate { };

        private int AverageExpectedGoldReward { get { return Mathf.RoundToInt((DifficultyLevel + 1) * 15 * (DurationInDays * 0.25f)); } }
        private float AverageExpectedItemReward { get { return (DifficultyLevel + 1) * 20 * (DurationInDays * 0.5f); } }
        private int ExperiencePoints { get { return (DifficultyLevel + 1) * 5 * DurationInDays; } }
        private int HandlerAverageExpectedGoldReward { get { return Mathf.RoundToInt(AverageExpectedGoldReward * 2 * (HandlerItemReward == null ? 1 : 0.5f)); } }

        private int m_DurationInDays;
        private int m_DaysLeftOnQuest;

        public QuestInstance(IQuestSource source) {
            QuestSource = source;
            GoldReward = new QuestRewardGold();
            DaysLeftOnPost = 5;

            DisplayName = "Quest name";
        }

        public bool WouldHeroAccept(HeroInstance hero) {
            if (hero.HeroState == HeroStates.WOUNDED || hero.HeroState == HeroStates.DEAD) return false;

            float preferenceValue = 0;

            preferenceValue += (hero.QuestPrefRewardGold / AverageExpectedGoldReward) * (GoldReward.RewardValue);
            preferenceValue += (hero.QuestPrefRewardItem / AverageExpectedItemReward) * (GetTotalItemRewardValue());

            float maxDifficultyDifference = 3;
            float difficultyScaler = (maxDifficultyDifference - Mathf.Abs(hero.QuestPrefDifficulty - DifficultyLevel)) / maxDifficultyDifference;
            preferenceValue *= difficultyScaler;

            float powerLevelScaler = (float)hero.PowerLevel / Mathf.Clamp(AveragePowerLevel, 1, float.MaxValue);
            preferenceValue *= powerLevelScaler;

            preferenceValue *= hero.QuestTypePreferences[QuestType];

            return preferenceValue > 0.7f;
        }

        public int GetHeroSuccessRate(HeroInstance hero) {
            float successChance = 95 * (1 - ((float)DifficultyLevel).Map(0, 10, 0, 1));

            //Star difference
            float starDiff = hero.QuestPrefDifficultyFloat - DifficultyLevel;
            successChance += (starDiff * 10);

            float heroPowerDiff = hero.PowerLevel - hero.BasePowerLevel;
            successChance += (heroPowerDiff / hero.Level) * 0.1f;

            successChance *= hero.Class.GetQuestModifier(QuestType);

            return Mathf.Clamp(Mathf.RoundToInt(successChance), 0, 100);
        }

        private float GetTotalItemRewardValue() {
            return ItemReward != null ? ItemReward.RewardValue : 0;
        }

        public bool CompleteQuest(HeroInstance hero) {

            //Check if the hero completed it or not
            int successRate = GetHeroSuccessRate(hero);
            int failChance = UnityEngine.Random.Range(0, 101);
            if (failChance > successRate) {
                RefundQuestRewards(true, true);

                failChance = UnityEngine.Random.Range(0, 101);
                if (failChance < 100 - successRate + 20) {
                    hero.HeroState = HeroStates.DEAD;
                } else {
                    hero.HeroState = HeroStates.WOUNDED;
                    hero.WoundedDays = ((100 - successRate) / 10) + 4;
                }
                return false;
            }

            if (ItemReward != null) ItemReward.ApplyReward(hero);
            if (AdditionalReward != null) AdditionalReward.ApplyReward(hero);

            hero.Experience += ExperiencePoints;
            hero.HeroState = HeroStates.IDLE;

            QuestSourceFaction faction = HeroManager.GetHeroFaction(hero);
            ReputationManager.GetReputationTracker(faction).ModifyReputation(ExperiencePoints * 0.1f);

            if (HandlerItemReward != null) InventoryManager.OwnedItems.Add(HandlerItemReward.Item);
            InventoryManager.Gold += Mathf.RoundToInt(HandlerAverageExpectedGoldReward * UnityEngine.Random.Range(HANDLER_GOLD_VARIANCE_MIN, HANDLER_GOLD_VARIANCE_MAX));
            InventoryManager.Stars += DifficultyLevel;
            return true;
        }

        public void RefundQuestRewards(bool refundGold, bool refundItem) {
            if (refundGold) {
                Debug.Log("Refunding gold: " + GoldReward.GoldCount);
                InventoryManager.Gold += GoldReward.GoldCount;
            }

            if (refundItem && ItemReward != null) {
                InventoryManager.MoveItemToOwned(ItemReward.Item);
            }
        }
    }

}