using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Inventory {

    public class GameItem {

        public string DisplayName { get; set; }
        public int PowerClass { get { return ((int)Rarity + 1) * 2; } }
        public GameItemRarity Rarity { get; set; }
        public int AttackPower { get { return Mathf.RoundToInt(BaseAttackPower * (BaseQuality + 1)) * (int)Rarity; } }
        public int DefencePower { get { return Mathf.RoundToInt(BaseDefencePower * (BaseQuality + 1)) * (int)Rarity; } }

        public float BaseQuality { get; set; }
        public int BaseAttackPower { get; set; }
        public int BaseDefencePower { get; set; }

        public GameItem(GameItemRarity rarity, float quality) {
            Rarity = rarity;
            BaseQuality = quality;
        }
    }

}