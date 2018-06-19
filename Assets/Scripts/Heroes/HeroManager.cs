using Rondo.Generic.Utility;
using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Quests.Sources;
using Rondo.QuestSim.UI.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Heroes {

    public static class HeroManager {

        private static Dictionary<HeroInstance, QuestSourceFaction> m_Heroes = new Dictionary<HeroInstance, QuestSourceFaction>();

        public static void Initialize() {

        }

        public static void AddHero(HeroInstance instance, QuestSourceFaction faction) {
            if (m_Heroes.ContainsKey(instance)) return;
            m_Heroes.Add(instance, faction);
        }

        public static HeroInstance GetRandomHero() {
            return GetAllHeroes().GetRandom();
        }

        public static List<HeroInstance> GetAllHeroes() {
            return new List<HeroInstance>(m_Heroes.Keys);
        }

        public static List<HeroInstance> GetAvailableHeroes() {
            List<HeroInstance> heroes = new List<HeroInstance>(m_Heroes.Keys);
            foreach(KeyValuePair<QuestInstance, HeroInstance> activeQuests in QuestManager.ActiveQuests) {
                heroes.Remove(activeQuests.Value);
            }
            return heroes;
        }

        public static QuestSourceFaction GetHeroFaction(HeroInstance hero) {
            if (!m_Heroes.ContainsKey(hero)) return null;
            return m_Heroes[hero];
        }

    }

}