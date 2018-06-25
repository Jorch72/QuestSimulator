using Rondo.Generic.Utility;
using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.Quests.Rewards;
using Rondo.QuestSim.Quests.Sources;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Quests {

    public class QuestInstance {

        private static float HANDLER_GOLD_VARIANCE_MIN = 0.8f;
        private static float HANDLER_GOLD_VARIANCE_MAX = 1.2f;

        public string DisplayName { get; set; }
        public int ObjectiveCount { get; set; }
        public IQuestSource QuestSource { get; private set; }
        public QuestTypes QuestType { get; set; }
        public int DifficultyLevel { get; set; }
        public QuestRewardGold GoldReward { get; set; }
        public QuestRewardItem ItemReward { get; set; }
        public IQuestReward AdditionalReward { get; set; }
        public List<QuestRewardHero> HeroRewards { get; set; }
        public int DaysLeftOnPost { get; set; }
        public int DaysLeftOnQuest { get; set; }
        public string HandlerGoldRewardEstimate { get { return Mathf.RoundToInt(HandlerAverageExpectedGoldReward * HANDLER_GOLD_VARIANCE_MIN) + " - " + Mathf.RoundToInt(HandlerAverageExpectedGoldReward * HANDLER_GOLD_VARIANCE_MAX); } }

        private int AverageExpectedGoldReward { get { return Mathf.RoundToInt((DifficultyLevel + 1) * 10 * (ObjectiveCount * 0.75f)); } }
        private float AverageExpectedItemReward { get { return (DifficultyLevel + 1) * 20 * (ObjectiveCount * 0.5f); } }
        private int ExperiencePoints { get { return (DifficultyLevel + 1) * 5 * ObjectiveCount; } }
        private int HandlerAverageExpectedGoldReward { get { return Mathf.RoundToInt(AverageExpectedGoldReward * 1.5f); } }

        public QuestInstance(IQuestSource source) {
            QuestSource = source;
            GoldReward = new QuestRewardGold();
            HeroRewards = new List<QuestRewardHero>();
            DaysLeftOnPost = 5;
            DaysLeftOnQuest = 3;

            DisplayName = "Quest name";
        }

        public bool WouldHeroAccept(HeroInstance hero) {
            float preferenceValue = 0;

            preferenceValue += (hero.QuestPrefRewardGold / AverageExpectedGoldReward) * (GoldReward.RewardValue);
            preferenceValue += (hero.QuestPrefRewardItem / AverageExpectedItemReward) * (GetTotalItemRewardValue());

            //Difficulty scaler, should be replaced by hero.PowerLevel
            float maxDifficultyDifference = 3;
            float difficultyScaler = (maxDifficultyDifference - Mathf.Abs(hero.QuestPrefDifficulty - DifficultyLevel)) / maxDifficultyDifference;
            preferenceValue *= difficultyScaler;

            return preferenceValue > 0.7f;
        }

        private float GetTotalItemRewardValue() {
            return ItemReward != null ? ItemReward.RewardValue : 0;
        }

        public void CompleteQuest(HeroInstance hero) {
            if(ItemReward != null) ItemReward.ApplyReward(hero);
            if (AdditionalReward != null) AdditionalReward.ApplyReward(hero);

            hero.Experience += ExperiencePoints;
            hero.HeroState = HeroStates.IDLE;

            QuestSourceFaction faction = HeroManager.GetHeroFaction(hero);
            ReputationManager.GetReputationTracker(faction).ModifyReputation(ExperiencePoints * 0.1f);

            InventoryManager.Gold += Mathf.RoundToInt(HandlerAverageExpectedGoldReward * Random.Range(HANDLER_GOLD_VARIANCE_MIN, HANDLER_GOLD_VARIANCE_MAX));
            InventoryManager.Stars += DifficultyLevel;
        }

        public void RefundQuestRewards(bool refundGold, bool refundItem) {
            if (refundGold) {
                InventoryManager.Gold += GoldReward.GoldCount;
            }

            if (refundItem && ItemReward != null) {
                InventoryManager.MoveItemToOwned(ItemReward.Item);
            }
        }
    }

}