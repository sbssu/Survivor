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

    Vector3 originPosition;     // ���� ��ġ (���� ��ǥ)
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

        transform.position = Camera.main.WorldToScreenPoint(originPosition);    // ��ġ �� ���.
        time = Mathf.Clamp(time + Time.deltaTime, 0f, showTime);                // Ÿ�� �� ����.

        float angle = time / showTime * 180f * Mathf.Deg2Rad;                   // ���� (0 > 180)
        float sin = Mathf.Clamp(Mathf.Sin(angle) * 1.2f, 0.0f, 1.0f);           // sin��.

        Color color = text.color;                           // Color �� ����.
        color.a = sin;                                      // ���İ��� Sin�׷��� ������ ���
        text.color = color;                                 // ���İ� ����
        transform.localScale = Vector3.one * sin;           // ������ ����

        // �ð��� �� �帣���� ����.
        if (time >= showTime)
            Destroy(gameObject);
    }
}
