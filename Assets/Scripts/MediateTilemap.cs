using UnityEngine;

public class MediateTilemap : MonoBehaviour
{
    [SerializeField] BoxCollider2D box;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "MainCamera" || Player.Instance == null)
            return;

        Vector3 playerPos = Player.Instance.transform.position;
        Vector3 position = transform.position;

        float offsetX = playerPos.x - position.x;
        float offsetY = playerPos.y - position.y;

        // �÷��̾���� �Ÿ� ���� �� x�� �� ��� x������ y�� �� ��� y������ �̵��Ѵ�.
        if (Mathf.Abs(offsetX) > Mathf.Abs(offsetY))
            transform.position += Vector3.right * (offsetX < 1 ? -1 : 1) * box.size.x * 2f;
        else
            transform.position += Vector3.up * (offsetY < 1 ? -1 : 1) * box.size.y * 2f;
    }
}
