using Rondo.Generic.Utility;
using Rondo.QuestSim.Quests.Sources;
using Rondo.QuestSim.UI.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Heroes {

    public static class HeroManager {

        private static List<HeroInstance> m_Heroes = new List<HeroInstance>();

        public static void Initialize() {

        }

        private static HeroInstance AddRandomHero() {
            HeroInstance newHero = HeroGenerator.GenerateHero();
            AddHero(newHero);
            return newHero;
        }

        public static void AddHero(HeroInstance instance) {
            if (m_Heroes.Contains(instance)) return;
            m_Heroes.Add(instance);
        }

        public static HeroInstance GetRandomHero() {
            return m_Heroes.GetRandom();
        }

    }

}