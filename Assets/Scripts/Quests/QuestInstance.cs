using Rondo.Generic.Utility;
using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Quests.Rewards;
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

        private int AverageExpectedGoldReward { get { return (DifficultyLevel + 1) * 10; } }
        private float AverageExpectedItemReward { get { return (DifficultyLevel + 1) * 20; } }
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

            Debug.Log("Gave " + hero.DisplayName + " " + ExperiencePoints + " exp");
        }
    }

}