using Rondo.QuestSim.Quests.Rewards;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Heroes {

    public class HeroInstance {

        public string DisplayName { get; set; }
        public string Class { get; set; }
        public int Experience { get; set; }

        public int Level { get { return (Experience / 20) + 1; } }
        public string ClassProgress { get { return "Lv" + Level + " " + Class; } }

        public HeroInstance() {
            DisplayName = "Hero name";
            Class = "Wizard";
            Experience = Random.Range(0, 1000);
        }

    }

}