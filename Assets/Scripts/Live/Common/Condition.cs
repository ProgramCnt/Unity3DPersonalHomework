using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;
    public float startValue;
    public float maxValue;
    public float passiveValue;
    public Image uiBar;

    void Start()
    {
        startValue = maxValue;
        curValue = maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        //ui업데이트 필요
        uiBar.fillAmount = GetPercentage();
    }

    float GetPercentage()
    {
        return curValue / maxValue;
    }

    public void AddValue(float value)
    {
        curValue = Mathf.Min(curValue + value, maxValue);
    }

    public void SubValue(float value)
    {
        curValue = Mathf.Max(curValue - value, 0);
    }
}
