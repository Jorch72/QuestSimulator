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

            while (rarity == GameItemRarity.UNKNOWN) rarity = EnumUtility.GetRandomEnumValue<GameItemRarity>();

            GameItem newItem = new GameItem(rarity, quality);

            newItem.DisplayName = NameDatabase.GetItemName(newItem.Rarity);
            newItem.BaseAttackPower = Random.Range(1f, 3f);
            newItem.BaseDefencePower = Random.Range(1f, 3f);
            return newItem;
        }

    }

}