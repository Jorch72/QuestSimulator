using Rondo.Generic.Utility;
using Rondo.QuestSim.Quests.Sources;
using Rondo.QuestSim.UI.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Reputation {

    public static class ReputationManager {

        private static Dictionary<QuestSourceFaction, ReputationTracker> m_ReputationDictionary = new Dictionary<QuestSourceFaction, ReputationTracker>();

        public static void Initialize() {
            for(int i = 0; i < 1; i++) {
                AddRandomFaction();
            }
        }

        private static QuestSourceFaction AddRandomFaction() {
            QuestSourceFaction newFaction = new QuestSourceFaction(ReputationBiases.UNKNOWN);
            QuestSourceFaction newRep = ReputationGenerator.GenerateReputationInstance(newFaction, ReputationBiases.UNKNOWN);
            AddFaction(newRep);
            return newFaction;
        }

        public static void AddFaction(QuestSourceFaction faction) {
            AddFactionRepInstance(faction);
        }

        public static void AddFactionRepInstance(QuestSourceFaction instance) {
            if (m_ReputationDictionary.ContainsKey(instance)) return;
            ReputationTracker newTracker = new ReputationTracker(instance);
            m_ReputationDictionary.Add(instance, newTracker);

            ReputationUI.Instance.AddReputationTracker(newTracker);
        }

        public static QuestSourceFaction GetRandomFaction() {
            List<QuestSourceFaction> factions = new List<QuestSourceFaction>(m_ReputationDictionary.Keys);
            return factions.GetRandom();
        }

        public static ReputationTracker GetReputationTracker(QuestSourceFaction faction) {
            if (!m_ReputationDictionary.ContainsKey(faction)) return null;
            return m_ReputationDictionary[faction];
        }

    }

}