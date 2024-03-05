using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopUI : Singleton<TopUI>
{
    [SerializeField] DamageText damagePrefab;
    [SerializeField] Text levelText;
    [SerializeField] Text killCountText;
    [SerializeField] Image expFill;

    public void UpdateExp(float current, float max)
    {
        if (max <= 0)
            expFill.fillAmount = 0f;
        else
            expFill.fillAmount = current / max;
    }
    public void UpdateLevel(int amount)
    {
        levelText.text = $"Lv.{amount}";
    }
    public void UpdateKillCount(int amount)
    {
        killCountText.text = amount.ToString("N0");
    }


    public void AppearDamage(Vector3 hitPoint, float amount)
    {
        DamageText newText = Instantiate(damagePrefab, transform);        
        newText.Setup(hitPoint, Mathf.RoundToInt(amount));
    }
}
