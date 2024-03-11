using UnityEngine;

public class FollowCamera : Singleton<FollowCamera>
{
    [SerializeField] Camera cam;
    [SerializeField] Transform target;
    [SerializeField] float lerpAmount;

    Vector3 offset;
    Vector2 camSize;

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        Vector3 destination = target.position + offset;
        //destination = Background.Instance.InBoundary(destination, camSize);
        transform.position = Vector3.Lerp(transform.position, destination, lerpAmount * Time.deltaTime);
    }

    public void Setup(Transform target)
    {
        this.target = target;

        offset = transform.position - target.position;
        camSize.x = cam.orthographicSize * 2f * (Screen.width / (float)Screen.height);
        camSize.y = camSize.x;
    }
}
