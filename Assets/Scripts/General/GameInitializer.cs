using Rondo.Generic.Utility;
using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Quests.Sources;
using Rondo.QuestSim.Reputation;
using Rondo.QuestSim.UI.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.General {

    public class GameInitializer : MonoBehaviourSingleton<GameInitializer> {

        private void Awake() {
            HeroManager.Initialize();
            ReputationManager.Initialize();
            QuestManager.Initialize();
            InventoryManager.Initialize();
        }

    }

}