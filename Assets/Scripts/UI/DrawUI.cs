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
        // �Ű������� �Ѱܹ��� ������ �迭�� ���� UI����.
        for (int i = 0; i < items.Length; i++)
            drawElements[i].Setup(items[i]);

        // �̺�Ʈ ��� �� ������ Ȱ��ȭ.
        this.selectEvent = selectEvent;
        window.SetActive(true);
    }
    public void OnSelectedItem(int index)
    {
        selectEvent?.Invoke(index);
        window.SetActive(false);
    }
}
