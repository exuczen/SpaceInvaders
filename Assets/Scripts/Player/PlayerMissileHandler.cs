using MustHave.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissileHandler : MissileHandler<PlayerMissile>
{
    [SerializeField]
    private EnemySwarm _enemySwarm = default;

    protected override void CreateObjectInstance(PlayerMissile prefab, Transform pool)
    {
        prefab.Create(pool, _enemySwarm);
    }
}
