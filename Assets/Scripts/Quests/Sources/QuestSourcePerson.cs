using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Quests.Sources {

    public class QuestSourcePerson : IQuestSource {

        public QuestSourcePerson(ReputationPersonalities personality) {
            personalityType = personality;
        }

        //Display options
        public string DisplayName { get; set; }
        public string RequestTitle { get { return "A personal request from " + DisplayName; } }

        public int MaxQuestDifficulty { get; set; }

        public ReputationPersonalities personalityType = ReputationPersonalities.VILLAGERS;
        public ReputationMoralityTypes MoralityType { get; set; }

        public void GenerateSettings() {
            ReputationGenerator.GenerateName(this, ReputationNameConventions.COMPOUND);
            ReputationGenerator.GenerateMorality(this, ReputationMoralityTypes.UNKNOWN);
        }
    }
}