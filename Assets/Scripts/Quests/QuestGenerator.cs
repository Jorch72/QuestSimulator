using Rondo.Generic.Utility;
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

        public static QuestInstance GenerateQuestInstance() {
            int questObjectiveSize = m_QuestSizeChoser.GetRandomValue();
            int sourceChoice = m_QuestSourceChoser.GetRandomValue();
            IQuestSource qSource;
            if (sourceChoice == 0) {
                qSource = ReputationGenerator.GenerateReputationInstance(new QuestSourceRumor());
            } else if (sourceChoice == 1) {
                qSource = ReputationManager.GetRandomFaction();
            } else {
                qSource = ReputationGenerator.GenerateReputationInstance(new QuestSourcePerson(EnumUtility.GetRandomEnumValue<ReputationBiases>()));
            }
            
            return QuestGenerator.GenerateQuestInstance(qSource, questObjectiveSize);
        }

        public static QuestInstance GenerateQuestInstance(IQuestSource questSource, int objectiveCount = 1) {
            QuestInstance quest = new QuestInstance(questSource);
            quest.ObjectiveCount = objectiveCount;
            quest.DifficultyLevel = questSource.QuestDifficulty;
            quest.QuestType = EnumUtility.GetRandomEnumValue<QuestTypes>();
            return quest;
        }

    }

}