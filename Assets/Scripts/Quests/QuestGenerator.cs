using Rondo.Generic.Utility;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Quests {

    public static class QuestGenerator {

        public static QuestInstance GenerateQuestInstance(IQuestSource questSource, int objectiveCount = 1) {
            QuestInstance quest = new QuestInstance(questSource);
            quest.ObjectiveCount = objectiveCount;
            quest.DifficultyLevel = UnityEngine.Random.Range(0, questSource.MaxQuestDifficulty);
            quest.QuestType = EnumUtility.GetRandomEnumValue<QuestTypes>();


            return quest;
        }

    }

}