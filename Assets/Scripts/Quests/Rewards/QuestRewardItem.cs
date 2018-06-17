using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Quests.Rewards {

    public class QuestRewardItem : IQuestReward {

        public GameItem Item { get; private set; }
        public float RewardValue { get { return Item.OverallPower * ((int)Item.Rarity * 0.5f) + ((int)Item.Rarity * 10); } }

        public QuestRewardItem(GameItem item) {
            Item = item;
        }

        public void GiveToHero() {
            
        }
    }

}