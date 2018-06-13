using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Quests.Rewards {

    public class QuestRewardItem : IQuestReward {
        public float RewardValue { get { return 1; } }

        public void GiveToHero() {
            
        }
    }

}