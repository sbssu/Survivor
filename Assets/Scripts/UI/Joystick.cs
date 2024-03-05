using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Joystick : MonoBehaviour
{
    [SerializeField] RectTransform stickParent;
    [SerializeField] RectTransform stickRect;
    [SerializeField] Vector2 dead;
    [SerializeField] UnityEvent<Vector2> onMove;

    float maxDistance;  // �ִ� �Ÿ�
    bool isPress;       // ������ ���ΰ�?

    private void Start()
    {
        // RectTransform.sizeDelta:Vector2
        // => x:width, y:height
        maxDistance = stickParent.sizeDelta.x / 2f;
    }

    void Update()
    {
        Move();
    }
    private void Move()
    {
        // �Է��� ���� (=��ġ)
        if (Input.GetMouseButtonDown(0))
        {
            // joystick���ο��� ��ġ �� �̵��� �����Ѵ�.
            if (RectTransformUtility.RectangleContainsScreenPoint(stickParent, Input.mousePosition))
                isPress = true;
        }

        // ���콺�� ������ ���� ��
        if (isPress && Input.GetMouseButton(0))
        {
            // ���콺 ��ġ �������κ��� ���������� �Ÿ��� maxDistance�� ���� �ʴ� ���.
            if (Vector3.Distance(stickParent.position, Input.mousePosition) <= maxDistance)
                stickRect.position = Input.mousePosition;
            else
            {
                // �������� ��ġ�������� ���ϴ� �������͸� ����� �� �ִ�Ÿ�(=��Į��)�� ���� ��ġ ����.
                Vector3 direction = (Input.mousePosition - stickParent.position).normalized;
                stickRect.position = stickParent.position + direction * maxDistance;
            }
        }
        else
        {
            stickRect.localPosition = Vector2.zero;
            isPress = false;
        }

        // �̺�Ʈ�� ������ �Է°� ���.
        {
            // ���� vector�� �� ���� maxDistance��� � ������ �����°�?
            Vector3 direction = stickRect.position - stickParent.position;
            Vector2 input = Vector2.zero;
            input.x = direction.x / maxDistance;
            input.y = direction.y / maxDistance;

            // �Է� ���� �� ����.
            if (Mathf.Abs(input.x) <= dead.x)
                input.x = 0f;
            if (Mathf.Abs(input.y) <= dead.y)
                input.y = 0f;

            // �̺�Ʈ ȣ��.
            onMove?.Invoke(input);
        }
    }
}
