using Rondo.Generic.Utility;
using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Quests.Sources;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Quests {

    public static class QuestManager {

        public static List<QuestInstance> Requests { get; set; }
        public static List<QuestInstance> PostedQuests { get; set; }
        public static Dictionary<QuestInstance, HeroInstance> ActiveQuests { get; set; }

        private static WeightedRandom<int> m_QuestSourceChoser;
        private static WeightedRandom<int> m_QuestSizeChoser;

        public static void Initialize() {
            Requests = new List<QuestInstance>();
            PostedQuests = new List<QuestInstance>();
            ActiveQuests = new Dictionary<QuestInstance, HeroInstance>();

            m_QuestSourceChoser = new WeightedRandom<int>(
                new int[3] { 0, 1, 2 },
                new int[3] { 1, 2, 1 });

            m_QuestSizeChoser = new WeightedRandom<int>(
                new int[5] { 1, 2, 3, 4, 5 },
                new int[5] { 5, 3, 3, 2, 1 });

            RefreshRequests();
        }

        public static void RefreshRequests() {
            Requests.Clear();

            QuestSourceFaction faction = ReputationManager.GetRandomFaction();
            int requestCount = Random.Range(4, 6);

            for (int i = 0; i < requestCount; i++) {
                int questObjectiveSize = m_QuestSizeChoser.GetRandomValue();
                int sourceChoice = m_QuestSourceChoser.GetRandomValue();
                IQuestSource qSource;
                if (sourceChoice == 0) {
                    qSource = ReputationGenerator.GenerateReputationInstance(new QuestSourceRumor());
                } else if(sourceChoice == 1){
                    qSource = ReputationManager.GetRandomFaction();
                } else {
                    qSource = ReputationGenerator.GenerateReputationInstance(new QuestSourcePerson(EnumUtility.GetRandomEnumValue<ReputationBiases>()));
                }
                QuestInstance newChain = QuestGenerator.GenerateQuestInstance(qSource, questObjectiveSize);
                Requests.Add(newChain);
            }
        }
    }

}