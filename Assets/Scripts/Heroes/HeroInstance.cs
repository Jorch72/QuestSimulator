using Rondo.Generic.Utility;
using Rondo.QuestSim.Quests;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Heroes {

    public class HeroInstance {

        public string DisplayName { get { return GetDisplayName(); } set { m_DisplayName = value; } }
        public string Nickname { get; set; }
        public HeroClasses Class { get; set; }
        public int Experience { get { return HeroState != HeroStates.UNDISCOVERED ? m_Experience : 0; } set { m_Experience = value; OnExperienceChange(); } }
        public int EquipmentLevel { get; set; }
        public HeroStates HeroState { get; set; }

        public int Level { get { return (Experience / 20) + 1; } }
        public float LevelProgress { get { return HeroState != HeroStates.UNDISCOVERED ? m_LevelProgress : 0; } }
        public string ClassProgress { get { return HeroState != HeroStates.UNDISCOVERED ? ("Lv" + Level + " " + Class.ToString().ToCamelCase()) : "???"; } }
        public int PowerLevel { get { return Mathf.RoundToInt((EquipmentLevel * 0.5f) + (Level + 1)); } }

        public Dictionary<QuestTypes, float> QuestTypePreferences { get; set; }
        public int QuestPrefDifficulty { get { return Level / 10; } }
        public float QuestPrefRewardItem { get; set; }
        public float QuestPrefRewardGold { get; set; }

        public Action OnExperienceChange = delegate { };

        private string m_DisplayName;
        private int m_Experience = 0;
        private int m_Level = 1;
        private float m_LevelProgress = 0;

        public HeroInstance() {
            QuestTypePreferences = new Dictionary<QuestTypes, float>();
            HeroState = HeroStates.UNDISCOVERED;

            OnExperienceChange += CalculateLevels;
        }

        private string GetDisplayName() {
            if (HeroState != HeroStates.UNDISCOVERED) {
                if (string.IsNullOrEmpty(Nickname)) {
                    return m_DisplayName;
                } else {
                    return m_DisplayName.Replace(" ", " \"" + Nickname + "\" ");
                }
            } else {
                return "???";
            }
        }

        private void CalculateLevels() {
            int totalExpLeft = m_Experience;
            int expRequired = 10;
            int lastExpRequired = 0;
            int currentLevel = 1;
            while(totalExpLeft != 0) {
                if (expRequired > totalExpLeft) break;
                totalExpLeft -= expRequired;
                currentLevel++;
                lastExpRequired = expRequired;
                expRequired = Mathf.RoundToInt(expRequired * 1.25f);
            }

            m_Level = currentLevel;
            m_LevelProgress = ((float)expRequired - totalExpLeft) / expRequired;
        }

    }

}