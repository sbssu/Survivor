using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Background : MonoBehaviour
{
    public static Background Instance { get; private set; }

    [SerializeField] Tilemap tilemap;

    Vector3 center;     // 중심점 위치.
    Vector3 min;        // 최소 지점 (좌측 하단)
    Vector3 max;        // 최대 지점 (우측 상단)
    Vector2 size;       // 크기.

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

        // Bound:특정 영역
        // => ClosetPoint:Vector3
        //    = point위치를 경계 내부로 변환시켜 반환.
        Vector3 point = tilemap.localBounds.ClosestPoint(position);
        point.z = z;
        return point;
    }
    public Vector3 InBoundary(Vector3 position, Vector2 camSize)
    {
        float z = position.z;

        // 경계를 타일 맵 크기에서 카메라 사이즈를 뺀 나머지로 정한다.
        Bounds boundary = new Bounds(center, size - camSize);
        Vector3 point = boundary.ClosestPoint(position);
        point.z = z;
        return point;
    }

}
