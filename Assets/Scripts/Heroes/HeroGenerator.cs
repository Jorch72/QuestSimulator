using Rondo.Generic.Utility;
using Rondo.QuestSim.Quests.Sources;
using Rondo.QuestSim.UI.Reputation;
using Rondo.QuestSim.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Heroes {

    public static class HeroGenerator {

        public static HeroInstance GenerateHero() {
            HeroInstance newHero = new HeroInstance();

            newHero.DisplayName = NameDatabase.GetHeroName();
            newHero.Nickname = Random.Range(0, 4) == 0 ? NameDatabase.GetCompoundName() : "";
            newHero.Class = EnumUtility.GetRandomEnumValue<HeroClasses>();
            newHero.Experience = Random.Range(0, 1000);
            newHero.IsDiscovered = Random.Range(0, 2) == 0 ? true : false;
            newHero.IsDiscovered = true;

            return newHero;
        }

    }

}