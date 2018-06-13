using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Quests.Sources {

    public class QuestSourceFaction : IQuestSource {

        public QuestSourceFaction(ReputationPersonalities personality) {
            personalityType = personality;
        }

        //Display options
        public string DisplayName { get; set; }
        public string RequestTitle { get { return "A request from " + DisplayName; } }

        public Sprite displayEmblem;

        //Quest preferences
        public float questPreferenceMonsterSlaying = 0.5f;
        public float questPreferenceDelivery = 0.5f;
        public float questPreferenceWar = 0.5f;
        public float questPreferenceChores = 0.5f;

        public int MaxQuestDifficulty { get; set; }

        public ReputationPersonalities personalityType = ReputationPersonalities.VILLAGERS;
        public ReputationMoralityTypes MoralityType { get; set; }
        private ReputationNameConventions m_NamingConvention;

        public void GenerateSettings() {
            ReputationGenerator.GenerateMorality(this, MoralityType);
            ReputationGenerator.GenerateQuestPreferences(this, personalityType);
            ReputationGenerator.GenerateName(this, ReputationNameConventions.GROUP);
            //ReputationGenerator.GenerateName(this, ReputationNameConventions.ADJECTIVES_GROUP, ReputationNameConventions.GROUP_OF_ADJECTIVE, ReputationNameConventions.GROUP_OF_THE_ADJECTIVE);
        }
    }
}