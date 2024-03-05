using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : RootBehaiour
{
    public enum TYPE
    {
        NORMAL,
        CRITICAL,
        GOLD,
    }


    [SerializeField] Text text;
    [SerializeField] float showTime;

    Vector3 originPosition;     // 생성 위치 (월드 좌표)
    float time;

    public void Setup(Vector3 originPosition, int amount, TYPE type = TYPE.NORMAL)
    {
        this.originPosition = originPosition;

        time = 0.0f;
        text.text = amount.ToString();
    }

    void Update()
    {
        if (isPauseObject)
            return;

        transform.position = Camera.main.WorldToScreenPoint(originPosition);    // 위치 값 계산.
        time = Mathf.Clamp(time + Time.deltaTime, 0f, showTime);                // 타임 값 증가.

        float angle = time / showTime * 180f * Mathf.Deg2Rad;                   // 각도 (0 > 180)
        float sin = Mathf.Clamp(Mathf.Sin(angle) * 1.2f, 0.0f, 1.0f);           // sin값.

        Color color = text.color;                           // Color 값 대입.
        color.a = sin;                                      // 알파값을 Sin그래프 값으로 계산
        text.color = color;                                 // 알파값 대입
        transform.localScale = Vector3.one * sin;           // 스케일 증감

        // 시간이 다 흐르고나면 제거.
        if (time >= showTime)
            Destroy(gameObject);
    }
}
