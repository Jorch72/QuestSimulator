
using System;
using UnityEngine;

namespace Rondo.Generic.Utility {

    public static class HeroUtility {

        public static void CalculateHeroLevel(float totalExperience, out int level, out int expForNextLevel, out float normalizedProgress) {
            float totalExpLeft = totalExperience;
            int expRequired = 10;
            int lastExpRequired = 0;
            int currentLevel = 1;
            while (totalExpLeft != 0) {
                if (expRequired > totalExpLeft) break;
                totalExpLeft -= expRequired;
                currentLevel++;
                lastExpRequired = expRequired;
                expRequired = Mathf.RoundToInt(expRequired * 1.25f);
            }

            level = currentLevel;
            expForNextLevel = expRequired;
            normalizedProgress = 1 - (((float)expRequired - totalExpLeft) / expRequired);
        }

        public static int GetTotalExperienceRequiredForLevel(int level) {
            int currentLevel = 1;
            int totalExp = 0;
            int expRequired = 10;
            while (currentLevel < level) {
                currentLevel++;
                expRequired = Mathf.RoundToInt(expRequired * 1.25f);
                totalExp += expRequired;
            }
            return totalExp;
        }

    }

}