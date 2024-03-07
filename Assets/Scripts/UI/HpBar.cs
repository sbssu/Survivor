using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] Slider slider;

    public void UpdateHpBar(Transform pivot, float hp, float maxHp)
    {
        transform.position = Camera.main.WorldToScreenPoint(pivot.position);
        slider.value = hp / maxHp;
    }
}
