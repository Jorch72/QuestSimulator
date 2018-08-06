
using Rondo.QuestSim.Inventory;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.Generic.Utility {

    public static class ItemUtility {

        public static List<GameItem> SortByRarity(List<GameItem> items) {
            items.Sort((i1, i2) => i1.Rarity.CompareTo(i2.Rarity));
            return items;
        }

    }

}