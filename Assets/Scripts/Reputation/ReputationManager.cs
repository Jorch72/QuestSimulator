using Rondo.Generic.Utility;
using Rondo.QuestSim.Quests.Sources;
using Rondo.QuestSim.UI.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Reputation {

    public class ReputationManager : MonoBehaviourSingleton<ReputationManager> {

        public ReputationUI reputationUI;

        private Dictionary<QuestSourceFaction, ReputationTracker> m_ReputationDictionary = new Dictionary<QuestSourceFaction, ReputationTracker>();

        private void Awake() {

        }

        private void Start() {
            for(int i = 0; i < 6; i++) {
                AddRandomFaction();
            }
        }

        private void AddRandomFaction() {
            QuestSourceFaction newRep = ReputationGenerator.GenerateReputationInstance(new QuestSourceFaction(ReputationPersonalities.UNKNOWN), ReputationPersonalities.UNKNOWN, ReputationMoralityTypes.UNKNOWN);
            AddFactionRepInstance(newRep);
        }

        public void AddFactionRepInstance(QuestSourceFaction instance) {
            if (m_ReputationDictionary.ContainsKey(instance)) return;
            ReputationTracker newTracker = new ReputationTracker(instance);
            m_ReputationDictionary.Add(instance, newTracker);

            reputationUI.AddReputationTracker(newTracker);
        }

        public QuestSourceFaction GetRandomFaction() {
            List<QuestSourceFaction> factions = new List<QuestSourceFaction>(m_ReputationDictionary.Keys);
            return factions.GetRandom();
        }

    }

}