using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Background : MonoBehaviour
{
    public static Background Instance { get; private set; }

    [SerializeField] Tilemap tilemap;

    Vector3 center;     // �߽��� ��ġ.
    Vector3 min;        // �ּ� ���� (���� �ϴ�)
    Vector3 max;        // �ִ� ���� (���� ���)
    Vector2 size;       // ũ��.

    private void Awake()
    {
        Instance = this;
        
        center = Vector3.zero;
        center.x = tilemap.origin.x + tilemap.size.x / 2f;
        center.y = tilemap.origin.y + tilemap.size.y / 2f;

        min = tilemap.origin;
        max = tilemap.origin + tilemap.size;
        size = new Vector2(tilemap.size.x, tilemap.size.y);
    }

    public Vector3 InBoundary(Vector3 position)
    {
        float z = position.z;

        // Bound:Ư�� ����
        // => ClosetPoint:Vector3
        //    = point��ġ�� ��� ���η� ��ȯ���� ��ȯ.
        Vector3 point = tilemap.localBounds.ClosestPoint(position);
        point.z = z;
        return point;
    }
    public Vector3 InBoundary(Vector3 position, Vector2 camSize)
    {
        float z = position.z;

        // ��踦 Ÿ�� �� ũ�⿡�� ī�޶� ����� �� �������� ���Ѵ�.
        Bounds boundary = new Bounds(center, size - camSize);
        Vector3 point = boundary.ClosestPoint(position);
        point.z = z;
        return point;
    }

}
