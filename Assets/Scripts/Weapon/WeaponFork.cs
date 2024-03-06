using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class WeaponFork : Gun
{
    protected override bool SearchTarget(out Vector2 dir)
    {
        dir = Player.Instance.direction;
        return true;
    }
}
