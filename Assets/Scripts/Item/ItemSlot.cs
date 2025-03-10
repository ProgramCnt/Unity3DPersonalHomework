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

    public void UseItem()                       //������ ���
    {
        if (itemData.itemType == ItemType.Consumable)               //������ Ÿ���� �Һ�ǰ�� ���
        {
            if (itemData.curStack > 0)                              //�������� ����� �� �ִ� ���
            {
                for (int i = 0; i < itemData.consumables.Length; i++)       //������ �Һ�Ÿ���� Ȯ���Ͽ� �Һ�Ÿ�Կ� ���� �÷��̾��� ���¸� ����
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
                
                UpdateCountUI(--itemData.curStack);             //UI ������Ʈ
            }
        }
    }

    public void UpdateCountUI(int count)
    {
        countText.text = count.ToString();
    }
}
