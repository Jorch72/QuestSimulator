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

            return newHero;
        }

    }

}