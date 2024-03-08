using UnityEngine;

public class MediateObject : MonoBehaviour
{
    [SerializeField] float limitDistance;

    private void LateUpdate()
    {
        Vector3 playerPos = Player.Instance.transform.position;
        Vector3 myPos = transform.position;

        // x 또는 y축의 거리가 limit보다 멀어졌을 경우 각 축을 기준으로 위치 반전.
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
