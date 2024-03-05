using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Transform target;
    [SerializeField] float lerpAmount;

    Vector3 offset;
    Vector2 camSize;

    void Start()
    {
        offset = transform.position - target.position;
        camSize.x = cam.orthographicSize * 2f * (Screen.width / (float)Screen.height);
        camSize.y = camSize.x;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 destination = target.position + offset;
        destination = Background.Instance.InBoundary(destination, camSize);
        transform.position = Vector3.Lerp(transform.position, destination, lerpAmount * Time.deltaTime);
    }
}
