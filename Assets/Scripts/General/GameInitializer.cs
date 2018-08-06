using Rondo.Generic.Utility;
using Rondo.QuestSim.Facilities;
using Rondo.QuestSim.Gameplay;
using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Reputation;
using UnityEngine;

namespace Rondo.QuestSim.General {

    public class GameInitializer : MonoBehaviourSingleton<GameInitializer> {

        private void Awake() {
            HeroManager.Initialize();
            ReputationManager.Initialize();
            QuestManager.Initialize();
            InventoryManager.Initialize();
            BlacksmithManager.Initialize();
        }

        private void Start() {
            InventoryManager.Gold = 100;
            InventoryManager.Stars = 0;
        }

    }

}