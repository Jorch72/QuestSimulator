﻿using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Quests {

    public interface IQuestSource {
        string DisplayName { get; set; }
        string RequestTitle { get; }

        ReputationMoralityTypes MoralityType { get; set; }
        int MaxQuestDifficulty { get; set; }

        void GenerateSettings();
    }

}