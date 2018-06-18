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

        public string DisplayName { get; set; }
        public int ObjectiveCount { get; set; }
        public IQuestSource QuestSource { get; private set; }
        public QuestTypes QuestType { get; set; }
        public int DifficultyLevel { get; set; }
        public QuestRewardGold GoldReward { get; set; }
        public List<QuestRewardItem> ItemRewards { get; set; }
        public int DaysLeftOnPost { get; set; }
        public int DaysLeftOnQuest { get; set; }

        private int AverageExpectedGoldReward { get { return Mathf.RoundToInt((DifficultyLevel + 1) * 10 * (ObjectiveCount * 0.75f)); } }
        private float AverageExpectedItemReward { get { return (DifficultyLevel + 1) * 20 * (ObjectiveCount * 0.5f); } }
        private int ExperiencePoints { get { return (DifficultyLevel + 1) * 5 * ObjectiveCount; } }

        public QuestInstance(IQuestSource source) {
            QuestSource = source;
            GoldReward = new QuestRewardGold();
            ItemRewards = new List<QuestRewardItem>();
            DaysLeftOnPost = 5;
            DaysLeftOnQuest = 3;

            DisplayName = "Quest name";
        }

        public bool WouldHeroAccept(HeroInstance hero) {
            float preferenceValue = 0;

            preferenceValue += (hero.QuestPrefRewardGold / AverageExpectedGoldReward) * (GoldReward.RewardValue);
            preferenceValue += (hero.QuestPrefRewardItem / AverageExpectedItemReward) * (GetTotalItemRewardValue());

            //Difficulty scaler, should be replaced by hero.PowerLevel
            float maxDifficultyDifference = 7;
            preferenceValue *= Mathf.Pow((maxDifficultyDifference - Mathf.Abs(hero.QuestPrefDifficulty - DifficultyLevel)) / maxDifficultyDifference, 1.5f);

            return preferenceValue > 0.7f;
        }

        private float GetTotalItemRewardValue() {
            float value = 0;
            foreach(QuestRewardItem itemReward in ItemRewards) {
                value += itemReward.RewardValue;
            }
            return value;
        }

        public void CompleteQuest(HeroInstance hero) {
            foreach (QuestRewardItem itemReward in ItemRewards) {
                hero.EquipmentLevel += itemReward.Item.OverallPower;
            }
            hero.Experience += ExperiencePoints;
            hero.HeroState = HeroStates.IDLE;

            QuestSourceFaction faction = HeroManager.GetHeroFaction(hero);
            ReputationManager.GetReputationTracker(faction).ModifyReputation(ExperiencePoints * 0.1f);

            InventoryManager.Gold += Mathf.RoundToInt(AverageExpectedGoldReward * 1.5f * Random.Range(0.8f, 1.2f));
        }

        public void RefundQuestRewards(bool refundGold, bool refundItems) {
            if (refundGold) {
                InventoryManager.Gold += GoldReward.GoldCount;
            }

            if (refundItems) {
                foreach (QuestRewardItem itemReward in ItemRewards) {
                    InventoryManager.MoveItemToOwned(itemReward.Item);
                }
            }
        }
    }

}