using Rondo.Generic.Utility;
using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Quests.Sources;
using Rondo.QuestSim.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Rondo.QuestSim.Reputation {

    public static class ReputationGenerator {

        public static T GenerateReputationInstance<T>(
            T newInstance,
            ReputationPersonalities personality = ReputationPersonalities.UNKNOWN,
            ReputationMoralityTypes morality = ReputationMoralityTypes.UNKNOWN)
            where T : IQuestSource {

            newInstance.MoralityType = morality;

            newInstance.GenerateSettings();

            return newInstance;
        }

        public static void GenerateMorality(IQuestSource instance, ReputationMoralityTypes forcedType) {
            while (forcedType == ReputationMoralityTypes.UNKNOWN) {
                forcedType = EnumUtility.GetRandomEnumValue<ReputationMoralityTypes>();
                instance.MoralityType = forcedType;
            }

            instance.MoralityType = forcedType;
        }

        public static void GenerateQuestPreferences(QuestSourceFaction instance, ReputationPersonalities forcedType) {
            while (forcedType == ReputationPersonalities.UNKNOWN) {
                forcedType = EnumUtility.GetRandomEnumValue<ReputationPersonalities>();
            }

            instance.personalityType = forcedType;

            switch (instance.personalityType) {
                case ReputationPersonalities.VILLAGERS:
                    instance.questPreferenceChores = 1;
                    instance.questPreferenceDelivery = 1;
                    instance.questPreferenceMonsterSlaying = 0.1f;
                    instance.questPreferenceWar = 0;
                    instance.MaxQuestDifficulty = 3;
                    break;
                case ReputationPersonalities.GOVERNMENT:
                    instance.questPreferenceChores = 0.1f;
                    instance.questPreferenceDelivery = 1f;
                    instance.questPreferenceMonsterSlaying = 0.5f;
                    instance.questPreferenceWar = 0.1f;
                    instance.MaxQuestDifficulty = 5;
                    break;
                case ReputationPersonalities.MONSTER_SLAYING:
                    instance.questPreferenceChores = 0;
                    instance.questPreferenceDelivery = 0.25f;
                    instance.questPreferenceMonsterSlaying = 1f;
                    instance.questPreferenceWar = 0.1f;
                    instance.MaxQuestDifficulty = 7;
                    break;
                case ReputationPersonalities.WAR_EFFORT:
                    instance.questPreferenceChores = 0;
                    instance.questPreferenceDelivery = 0.2f;
                    instance.questPreferenceMonsterSlaying = 0f;
                    instance.questPreferenceWar = 1;
                    instance.MaxQuestDifficulty = 10;
                    break;
            }
        }

        public static void GenerateName(IQuestSource instance, params ReputationNameConventions[] choices) {
            GenerateName(instance, choices[UnityEngine.Random.Range(0, choices.Length)]);
        }

        public static void GenerateName(IQuestSource instance, ReputationNameConventions forcedType) {
            while (forcedType == ReputationNameConventions.UNKNOWN) {
                forcedType = EnumUtility.GetRandomEnumValue<ReputationNameConventions>();
            }

            StringBuilder sb = new StringBuilder();
            switch (forcedType) {
                case ReputationNameConventions.COMPOUND:
                    instance.DisplayName = NameDatabase.GetCompoundName(instance.MoralityType);
                    break;
                case ReputationNameConventions.GROUP:
                    instance.DisplayName = NameDatabase.GetGroupName();
                    break;
                case ReputationNameConventions.POINT_OF_INTEREST:
                    instance.DisplayName = NameDatabase.GetPointOfInterestName();
                    break;
                case ReputationNameConventions.TERRITORY:
                    instance.DisplayName = NameDatabase.GetTerritoryName();
                    break;
            }
        }

    }

}