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
        public List<IQuestReward> ItemRewards { get; set; }
        public int DaysLeftOnPost { get; set; }
        public int DaysLeftOnQuest { get; set; }

        public QuestInstance(IQuestSource source) {
            QuestSource = source;
            GoldReward = new QuestRewardGold();
            ItemRewards = new List<IQuestReward>();
            DaysLeftOnPost = 5;
            DaysLeftOnQuest = 3;

            DisplayName = "Quest name";
        }

    }

}