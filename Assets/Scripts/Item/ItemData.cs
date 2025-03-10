using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템 타입
public enum ItemType
{
    Consumable,
    Resource
}

//아이템 소비타입
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

//아이템 ScriptableObject 데이터
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