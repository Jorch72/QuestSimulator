using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.ScriptableObjects {

    [CreateAssetMenu]
    public class ItemIconDatabase : ScriptableObject {
        public Sprite swordIcon;
        public Sprite hammerIcon;
        public Sprite daggerIcon;
        public Sprite shieldIcon;
        public Sprite staffIcon;
        public Sprite axeIcon;
        public Sprite chestIcon;
        public Sprite helmetIcon;
        public Sprite gauntletIcon;
        public Sprite greavesIcon;
        public Sprite leggingsIcon;
        public Sprite spauldersIcon;

        public Sprite GetSpriteForType(GameItemTypes state) {
            switch (state) {
                case GameItemTypes.SWORD:
                    return swordIcon;
                case GameItemTypes.HAMMER:
                    return hammerIcon;
                case GameItemTypes.DAGGER:
                    return daggerIcon;
                case GameItemTypes.SHIELD:
                    return shieldIcon;
                case GameItemTypes.STAFF:
                    return staffIcon;
                case GameItemTypes.AXE:
                    return axeIcon;
                case GameItemTypes.CHESTPIECE:
                    return chestIcon;
                case GameItemTypes.HELMET:
                    return helmetIcon;
                case GameItemTypes.GAUNTLETS:
                    return gauntletIcon;
                case GameItemTypes.GREAVES:
                    return greavesIcon;
                case GameItemTypes.LEGGINGS:
                    return leggingsIcon;
                case GameItemTypes.SPAULDERS:
                    return spauldersIcon;
                default:
                    return null;
            }
        }
    }

}
 
 