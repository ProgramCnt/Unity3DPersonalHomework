using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ Ÿ��
public enum ItemType
{
    Consumable,
    Resource
}

//������ �Һ�Ÿ��
public enum ConsumableType
{
    Health,
    Stamina
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType consumableType;
    public float value;
}

//������ ScriptableObject ������
[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Item Info")]
    public string itemName;
    public string itemDescription;
    public ItemType itemType;
    public Sprite itemIcon;
    public GameObject prefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStack;
    public int curStack;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;
    public float cooldown;

    [Header("Equip")]
    public GameObject equipPrefab;
}