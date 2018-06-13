using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Quests {

    public static class QuestGenerator {

        public static QuestInstance GenerateQuestInstance(IQuestSource questSource, int objectiveCount = 1) {
            QuestInstance chain = new QuestInstance(questSource);
            chain.ObjectiveCount = objectiveCount;
            chain.DifficultyLevel = UnityEngine.Random.Range(0, questSource.MaxQuestDifficulty);
            return chain;
        }

    }

}