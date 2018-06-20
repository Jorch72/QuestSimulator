using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Quests.Sources {

    public class QuestSourceFaction : IQuestSource {

        private static int MIN_HEROES_PER_FACTION = 2;
        private static int MAX_HEROES_PER_FACTION = 6;

        public QuestSourceFaction(ReputationBiases personality) {
            personalityType = personality;
            Heroes = new List<HeroInstance>();
        }

        //Display options
        public string DisplayName { get; set; }
        public string RequestTitle { get { return "A request from " + DisplayName; } }

        public Sprite displayEmblem;

        //Quest preferences
        public float questPreferenceMonsterSlaying = 0.5f;
        public float questPreferenceDelivery = 0.5f;
        public float questPreferenceWar = 0.5f;
        public float questPreferenceChores = 0.5f;

        public int QuestDifficulty { get { return GetQuestDifficulty(); } }

        public ReputationBiases personalityType = ReputationBiases.VILLAGERS;
        private ReputationNameConventions m_NamingConvention;

        //Heroes
        public List<HeroInstance> Heroes { get; set; }

        public void GenerateSettings() {
            ReputationGenerator.GenerateQuestPreferences(this, personalityType);
            ReputationGenerator.GenerateName(this, ReputationNameConventions.GROUP);

            for(int i = 0; i < Random.Range(MIN_HEROES_PER_FACTION, MAX_HEROES_PER_FACTION + 1); i++) {
                Heroes.Add(HeroGenerator.GenerateHero(this));
            }
        }

        private int GetQuestDifficulty() {
            int totalHeroDifficulty = 0;

            foreach(HeroInstance hero in Heroes) {
                totalHeroDifficulty += hero.QuestPrefDifficulty;
            }

            totalHeroDifficulty /= Heroes.Count;
            totalHeroDifficulty = Mathf.Clamp(totalHeroDifficulty + Random.Range(-2, 2), 0, 10);
            return totalHeroDifficulty;
        }
    }
}