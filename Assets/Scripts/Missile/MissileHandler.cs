using MustHave.DesignPatterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileHandler<T> : ObjectPoolHandler<T> where T : Missile
{
    public void SetMissilesSimulated(bool value)
    {
        foreach (Transform child in _containers[0])
        {
            child.GetComponent<Missile>().Rigidbody.simulated = value;
        }
    }

    protected override void CreateObjectInstance(T prefab, Transform pool)
    {
        prefab.Create<T>(pool);
    }
}
