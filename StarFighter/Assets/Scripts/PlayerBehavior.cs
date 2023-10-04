using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : EnemyBehavior
{
    public override bool IsPlayer()
    {
        return true;
    }

    protected override void Destroy()
    {
        //TODO : Game over
        Destroy(gameObject);
    }
}
