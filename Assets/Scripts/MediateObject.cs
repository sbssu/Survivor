using UnityEngine;

public class MediateObject : MonoBehaviour
{
    [SerializeField] float limitDistance;

    private void LateUpdate()
    {
        Vector3 playerPos = Player.Instance.transform.position;
        Vector3 myPos = transform.position;

        // x �Ǵ� y���� �Ÿ��� limit���� �־����� ��� �� ���� �������� ��ġ ����.
        bool flipX = Mathf.Abs(playerPos.x - myPos.x) >= limitDistance;
        bool flipY = Mathf.Abs(playerPos.y - myPos.y) >= limitDistance;
        if(flipX || flipY)
        {
            Vector3 movement = myPos - playerPos;
            movement.x *= flipX ? -1f : 1f;
            movement.y *= flipY ? -1f : 1f;
            transform.position = playerPos + movement;
        }
    }
}
