using Rondo.Generic.Utility;
using Rondo.QuestSim.Gameplay;
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

        private static WeightedRandom<int> m_QuestAmountChoser;

        public static void Initialize() {
            Requests = new List<QuestInstance>();
            PostedQuests = new List<QuestInstance>();
            ActiveQuests = new Dictionary<QuestInstance, HeroInstance>();

            m_QuestAmountChoser = new WeightedRandom<int>(
                new int[3] { 0, 1, 2 },
                new int[3] { 1, 3, 1 });

            DayManager.Instance.OnNextDay += AddDailyRequest;

            GenerateStartingQuests();
        }

        private static void AddDailyRequest() {
            int questCount = m_QuestAmountChoser.GetRandomValue();
            for (int i = 0; i < questCount; i++) {
                QuestInstance newQuest = QuestGenerator.GenerateQuestInstance();
                Requests.Add(newQuest);
            }
        }

        public static void GenerateStartingQuests() {
            Requests.Clear();

            int requestCount = Random.Range(4, 6);

            for (int i = 0; i < requestCount; i++) {
                QuestInstance newQuest = QuestGenerator.GenerateQuestInstance();
                Requests.Add(newQuest);
            }
        }

    }

}