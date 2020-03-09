using MustHave.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissileHandler : MissileHandler
{
    [SerializeField]
    private EnemySwarm _enemySwarm = default;

    protected override void FillPool(int count)
    {
        PlayerMissile prefab = _prefab as PlayerMissile;
        for (int i = 0; i < count; i++)
        {
            prefab.Create(_pool, _enemySwarm);
        }
    }
}
