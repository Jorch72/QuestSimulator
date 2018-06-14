using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Quests.Rewards {

    public class QuestRewardGold : IQuestReward {
        public int GoldCount { get; set; }
        public float RewardValue { get { return GoldCount; } }

        public void GiveToHero() {
            
        }
    }

}