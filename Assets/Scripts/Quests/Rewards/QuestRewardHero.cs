using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.Quests.Sources;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Quests.Rewards {

    public class QuestRewardHero : IQuestReward {

        public HeroInstance Hero { get; private set; }
        public float RewardValue { get { return Hero.PowerLevel; } }

        public QuestRewardHero(QuestSourceFaction faction) {
            Hero = HeroGenerator.GenerateHero(faction);
        }
    }

}