using MustHave.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissileHandler : MissileHandler
{
    [SerializeField]
    private EnemySwarm _enemySwarm = default;

    protected override void FillMissilePool(int count)
    {
        PlayerMissile prefab = _missilePrefab as PlayerMissile;
        for (int i = 0; i < count; i++)
        {
            prefab.Create(_missilePool, _enemySwarm);
        }
    }
}
