using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper : Enemy
{
    private void OnEnable()
    {
        transform.position = Player.Instance.transform.position + Vector3.right * 30f;
        Setup();
    }

    protected override void Movement()
    {
        // �÷��̾� ��ġ�� ������ �����δ�.
        Vector3 point = Player.Instance.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, point, 15 * Time.deltaTime);
    }

    public override void DeadForce()
    {
        
    }
}
