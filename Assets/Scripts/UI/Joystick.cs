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

    float maxDistance;  // 최대 거리
    bool isPress;       // 누르는 중인가?

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
        // 입력의 시작 (=터치)
        if (Input.GetMouseButtonDown(0))
        {
            // joystick내부에서 터치 시 이동을 시작한다.
            if (RectTransformUtility.RectangleContainsScreenPoint(stickParent, Input.mousePosition))
                isPress = true;
        }

        // 마우스를 누르고 있을 때
        if (isPress && Input.GetMouseButton(0))
        {
            // 마우스 터치 지점으로부터 원점까지의 거리가 maxDistance를 넘지 않는 경우.
            if (Vector3.Distance(stickParent.position, Input.mousePosition) <= maxDistance)
                stickRect.position = Input.mousePosition;
            else
            {
                // 원점에서 터치지점으로 향하는 단위벡터를 계산한 뒤 최대거리(=스칼라)를 곱해 위치 고정.
                Vector3 direction = (Input.mousePosition - stickParent.position).normalized;
                stickRect.position = stickParent.position + direction * maxDistance;
            }
        }
        else
        {
            stickRect.localPosition = Vector2.zero;
            isPress = false;
        }

        // 이벤트에 전달할 입력값 계산.
        {
            // 현재 vector의 각 축이 maxDistance대비 어떤 비율을 가지는가?
            Vector3 direction = stickRect.position - stickParent.position;
            Vector2 input = Vector2.zero;
            input.x = direction.x / maxDistance;
            input.y = direction.y / maxDistance;

            // 입력 데드 값 적용.
            if (Mathf.Abs(input.x) <= dead.x)
                input.x = 0f;
            if (Mathf.Abs(input.y) <= dead.y)
                input.y = 0f;

            // 이벤트 호출.
            onMove?.Invoke(input);
        }
    }
}
