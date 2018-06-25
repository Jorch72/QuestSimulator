using Rondo.Generic.Utility;
using Rondo.QuestSim.Gameplay;
using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Reputation;
using Rondo.QuestSim.ScriptableObjects;
using UnityEngine;

namespace Rondo.QuestSim.General {

    public class SpriteFetcher : MonoBehaviourSingleton<SpriteFetcher> {

        public static IconDatabase Icons { get { return Instance.icons; } }

        public IconDatabase icons;

        private void Awake() {
            Instance = this;
        }
    }


}