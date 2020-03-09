using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileHandler : ObjectPoolHandler<Missile>
{
    protected override void FillPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _prefab.Create<Missile>(_pool);
        }
    }

    public void SetMissilesSimulated(bool value)
    {
        foreach (Transform child in _container)
        {
            child.GetComponent<Missile>().Rigidbody.simulated = value;
        }
    }
}
