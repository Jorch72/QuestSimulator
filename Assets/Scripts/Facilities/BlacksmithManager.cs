using Rondo.Generic.Utility;
using Rondo.QuestSim.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Facilities {

    public static class BlacksmithManager {

        public static List<GameItem> ItemsOnSale { get { return m_ItemsOnSale; } set { m_ItemsOnSale = ItemUtility.SortByRarity(value); } }
        public static GameItemRarity BestQualityInStore { get; set; }

        private static List<GameItem> m_ItemsOnSale = new List<GameItem>();

        public static void Initialize() {
            m_ItemsOnSale = new List<GameItem>();
            BestQualityInStore = GameItemRarity.COMMON;

            RefreshItemsOnSale();
        }

        public static void RefreshItemsOnSale() {
            m_ItemsOnSale.Clear();
            for (int i = 0; i < 10; i++) {
                m_ItemsOnSale.Add(GameItemGenerator.GenerateItem(GameItemTypes.UNKNOWN, EnumUtility.GetRandomEnumValue<GameItemRarity>(1, (int)BestQualityInStore), UnityEngine.Random.Range(0f, 1f)));
            }
            m_ItemsOnSale = ItemUtility.SortByRarity(m_ItemsOnSale);
        }
    }

}