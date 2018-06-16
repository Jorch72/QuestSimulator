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
            for(int i = 0; i < 6; i++) {
                AddRandomFaction();
            }
        }

        private static void AddRandomFaction() {
            QuestSourceFaction newRep = ReputationGenerator.GenerateReputationInstance(new QuestSourceFaction(ReputationBiases.UNKNOWN), ReputationBiases.UNKNOWN);
            AddFactionRepInstance(newRep);
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

    }

}