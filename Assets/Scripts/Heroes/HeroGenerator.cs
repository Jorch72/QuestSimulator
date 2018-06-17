using Rondo.Generic.Utility;
using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Quests.Sources;
using Rondo.QuestSim.UI.Reputation;
using Rondo.QuestSim.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Heroes {

    public static class HeroGenerator {

        public static HeroInstance GenerateHero(bool isTemporaryHero = false) {
            HeroInstance newHero = new HeroInstance();

            newHero.DisplayName = NameDatabase.GetHeroName();
            //newHero.Nickname = UnityEngine.Random.Range(0, 4) == 0 ? NameDatabase.GetCompoundName() : "";
            newHero.Class = EnumUtility.GetRandomEnumValue<HeroClasses>();
            newHero.Experience = UnityEngine.Random.Range(0, 1000);
            newHero.EquipmentLevel = UnityEngine.Random.Range(0, 100);
            newHero.HeroState = UnityEngine.Random.Range(0, 2) == 0 ? HeroStates.UNDISCOVERED : HeroStates.IDLE;

            Dictionary<QuestTypes, float> tempQuestValues = new Dictionary<QuestTypes, float>();
            float tempQuestValuesTotal = 0;
            foreach (QuestTypes questType in Enum.GetValues(typeof(QuestTypes))) {
                tempQuestValues.Add(questType, UnityEngine.Random.Range(0f, 10f));
                tempQuestValuesTotal = tempQuestValues[questType];
            }
            foreach (QuestTypes questType in Enum.GetValues(typeof(QuestTypes))) {
                newHero.QuestTypePreferences[questType] = tempQuestValues[questType] / tempQuestValuesTotal;
            }

            newHero.QuestPrefRewardGold = UnityEngine.Random.Range(0f, 1f);
            newHero.QuestPrefRewardItem = 1 - newHero.QuestPrefRewardGold;

            if(!isTemporaryHero) HeroManager.AddHero(newHero);

            return newHero;
        }

    }

}