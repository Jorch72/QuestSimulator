using Rondo.Generic.Utility;
using Rondo.QuestSim.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Inventory {

    public static class GameItemGenerator {

        public static GameItem GenerateItem(
            GameItemRarity rarity = GameItemRarity.UNKNOWN,
            float quality = 0.5f) {

            GameItem newItem = new GameItem(EnumUtility.GetRandomEnumValue<GameItemRarity>(), quality);

            newItem.DisplayName = NameDatabase.GetItemName(newItem.Rarity);
            newItem.BaseAttackPower = Random.Range(1, 5);
            newItem.BaseDefencePower = Random.Range(1, 5);
            return newItem;
        }

    }

}