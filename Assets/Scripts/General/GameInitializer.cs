using Rondo.Generic.Utility;
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
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Q)) {
                DayManager.EndDay();
            }
        }

    }

}