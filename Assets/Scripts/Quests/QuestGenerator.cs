using Rondo.Generic.Utility;
using Rondo.QuestSim.Quests.Rewards;
using Rondo.QuestSim.Quests.Sources;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Quests {

    public static class QuestGenerator {

        private static WeightedRandom<int> m_QuestSourceChoser = new WeightedRandom<int>(
            new int[3] { 0, 1, 2 },
            new int[3] { 1, 2, 1 });

        private static WeightedRandom<int> m_QuestSizeChoser = new WeightedRandom<int>(
            new int[5] { 1, 2, 3, 4, 5 },
            new int[5] { 5, 3, 3, 2, 1 });

        public static int daysSinceHeroRecruit = Random.Range(10, 20);
        public static int daysSinceFactionRecruit = Random.Range(25, 35);

        public static QuestInstance GenerateQuestInstance() {
            int questObjectiveSize = m_QuestSizeChoser.GetRandomValue();
            int sourceChoice = m_QuestSourceChoser.GetRandomValue();

            IQuestReward additionalReward = null;
            IQuestSource qSource;
            if (sourceChoice == 0) {
                qSource = ReputationGenerator.GenerateReputationInstance(new QuestSourceRumor());
            } else if (sourceChoice == 1) {
                qSource = ReputationManager.GetRandomFaction();
                QuestSourceFaction factionSource = qSource as QuestSourceFaction;
                if (Random.Range(0, daysSinceHeroRecruit) == 0) {
                    additionalReward = new QuestRewardHero(factionSource);
                    daysSinceHeroRecruit = Random.Range(10, 20);
                } else if (Random.Range(0, daysSinceFactionRecruit) == 0) {
                    additionalReward = new QuestRewardFaction(factionSource.AverageHeroLevel + Random.Range(3, 6));
                    daysSinceFactionRecruit = Random.Range(25, 35);
                }
                } else {
                qSource = ReputationGenerator.GenerateReputationInstance(new QuestSourcePerson(EnumUtility.GetRandomEnumValue<ReputationBiases>()));
            }

            QuestInstance quest = GenerateQuestInstance(qSource, questObjectiveSize);
            quest.AdditionalReward = additionalReward;
            return quest;
        }

        public static QuestInstance GenerateQuestInstance(IQuestSource questSource, int objectiveCount = 1) {
            QuestInstance quest = new QuestInstance(questSource);
            quest.DurationInDays = objectiveCount;
            quest.DifficultyLevel = questSource.QuestDifficulty;
            quest.QuestType = EnumUtility.GetRandomEnumValue<QuestTypes>();
            return quest;
        }

    }

}