using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Inventory {

    public static class InventoryManager {

        public static int Gold { get; set; }
        public static List<GameItem> OwnedItems { get; set; }
        public static List<GameItem> ReservedItems { get; set; }

        public static void Initialize() {
            Gold = 0;
            OwnedItems = new List<GameItem>();
            ReservedItems = new List<GameItem>();

            for (int i = 0; i < 10; i++) {
                OwnedItems.Add(GameItemGenerator.GenerateItem( GameItemRarity.UNKNOWN, Random.Range(0f, 1f)));
            }
        }

        public static void MoveItemToReserved(GameItem item) {
            if (!OwnedItems.Contains(item)) return;
            OwnedItems.Remove(item);
            ReservedItems.Add(item);
        }

        public static void MoveItemToOwned(GameItem item) {
            if (!ReservedItems.Contains(item)) return;
            ReservedItems.Remove(item);
            OwnedItems.Add(item);
        }

    }

}