using Rondo.Generic.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Inventory {

    public class GameItem {

        public string DisplayName { get; set; }
        public int OverallPower { get { return (AttackPower + DefencePower) * ((int)Rarity + 1); } }
        public GameItemRarity Rarity { get; set; }
        public int AttackPower { get { return Mathf.RoundToInt(BaseAttackPower * (BaseQuality + 1)) * (int)Rarity; } }
        public int DefencePower { get { return Mathf.RoundToInt(BaseDefencePower * (BaseQuality + 1)) * (int)Rarity; } }

        public float BaseQuality { get; set; }
        public float BaseAttackPower { get; set; }
        public float BaseDefencePower { get; set; }

        public GameItem(GameItemRarity rarity, float quality) {
            Rarity = rarity;
            BaseQuality = quality.Map(0, 1, 0.4f, 0.8f);
        }
    }

}