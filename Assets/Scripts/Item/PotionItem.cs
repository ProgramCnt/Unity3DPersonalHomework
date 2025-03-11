using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionItem : MonoBehaviour, IInteractable
{
    public ItemData itemData;

    [SerializeField]
    ItemSlot itemSlot;

    public void Interact()
    {
        if (itemData.curStack >= itemData.maxStack)
        {
            return;
        }
        itemSlot.UpdateCountUI(++itemData.curStack);
        Destroy(gameObject);
    }
}
