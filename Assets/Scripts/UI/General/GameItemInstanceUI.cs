using Rondo.QuestSim.General;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.Quests.Rewards;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.General {

    [RequireComponent(typeof(GameItemPopupCaller))]
    public class GameItemInstanceUI : MonoBehaviour {

        public Image icon;
        public TextMeshProUGUI titleText;

        private GameItemPopupCaller m_ItemPopupCaller;

        public void SetItem(QuestRewardItem reward) {
            if(reward == null) {
                SetItem((GameItem)null);
            } else {
                SetItem(reward.Item);
            }
        }

        public void SetItem(GameItem item) {
            if(m_ItemPopupCaller == null) m_ItemPopupCaller = GetComponent<GameItemPopupCaller>();

            if(item == null) {
                titleText.text = "-";
            } else {
                titleText.text = item.DisplayName;
                icon.overrideSprite = item.GetIcon();
            }

            m_ItemPopupCaller.associatedItem = item;
        }

    }

}