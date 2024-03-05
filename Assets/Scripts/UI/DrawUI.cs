using System;
using UnityEngine;

public class DrawUI : Singleton<DrawUI>
{
    [SerializeField] GameObject window;
    [SerializeField] DrawElementUI[] drawElements;

    Action<int> selectEvent;

    private void Start()
    {
        window.SetActive(false);
    }

    public void ShowDrawUI(Item[] items, Action<int> selectEvent)
    {
        // 매개변수로 넘겨받은 아이템 배열을 통해 UI세팅.
        for (int i = 0; i < items.Length; i++)
            drawElements[i].Setup(items[i]);

        // 이벤트 등록 및 윈도우 활성화.
        this.selectEvent = selectEvent;
        window.SetActive(true);
    }
    public void OnSelectedItem(int index)
    {
        selectEvent?.Invoke(index);
        window.SetActive(false);
    }
}
