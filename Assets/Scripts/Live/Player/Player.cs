using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;
    //public Equipment equip;

    //public ItemData itemData;
    //public Action addItem;

    //public Transform dropPosition;              //이거 초기화 여기에서 해줘도 괜찮지 않나??

    private void Awake()
    {
        PlayerManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        //equip = GetComponent<Equipment>();
    }
}
