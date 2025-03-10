using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    [SerializeField]
    ItemData itemData;

    [SerializeField]
    TextMeshProUGUI countText;

    private void Start()
    {
        countText.text = itemData.curStack.ToString();
    }

    public void UseItem()                       //아이템 사용
    {
        if (itemData.itemType == ItemType.Consumable)               //아이템 타입이 소비품일 경우
        {
            if (itemData.curStack > 0)                              //아이템을 사용할 수 있는 경우
            {
                for (int i = 0; i < itemData.consumables.Length; i++)       //아이템 소비타입을 확인하여 소비타입에 따라 플레이어의 상태를 변경
                {
                    if (itemData.consumables[i].consumableType == ConsumableType.Health)
                    {
                        PlayerManager.Instance.Player.condition.Health.AddValue(itemData.consumables[i].value);
                    }
                    else if (itemData.consumables[i].consumableType == ConsumableType.Stamina)
                    {
                        PlayerManager.Instance.Player.condition.Stamina.AddValue(itemData.consumables[i].value);
                    }
                }
                
                UpdateCountUI(--itemData.curStack);             //UI 업데이트
            }
        }
    }

    public void UpdateCountUI(int count)
    {
        countText.text = count.ToString();
    }
}
