using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour, IDamageable
{
    [SerializeField]
    Condition health;
    [SerializeField]
    Condition stamina;

    public Condition Health { get { return health; } }
    public Condition Stamina { get { return stamina; } }

    public event Action onTakeDamage;

    // Update is called once per frame
    void Update()
    {
        //스테미나 자동회복
        if (!PlayerManager.Instance.Player.controller.IsSprint)
        {
            Stamina.AddValue(Stamina.passiveValue * Time.deltaTime * 5);
            Debug.Log("스테미나 회복중" + Stamina.curValue);
        }
    }

    //체력 회복 기능
    public void Heal(float amount)
    {
        Health.AddValue(amount);
    }

    //데미지 받을때 호출
    public void OnTakeDamage(int damage)
    {
        Health.SubValue(damage);
        onTakeDamage?.Invoke();
    }
}
