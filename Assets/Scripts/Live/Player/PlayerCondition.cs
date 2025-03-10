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
        //���׹̳� �ڵ�ȸ��
        if (!PlayerManager.Instance.Player.controller.IsSprint)
        {
            Stamina.AddValue(Stamina.passiveValue * Time.deltaTime * 5);
            Debug.Log("���׹̳� ȸ����" + Stamina.curValue);
        }
    }

    //ü�� ȸ�� ���
    public void Heal(float amount)
    {
        Health.AddValue(amount);
    }

    //������ ������ ȣ��
    public void OnTakeDamage(int damage)
    {
        Health.SubValue(damage);
        onTakeDamage?.Invoke();
    }
}
