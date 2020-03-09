using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : Missile
{
    private EnemySwarm _enemySwarm = default;

    public PlayerMissile Create(Transform pool, EnemySwarm enemySwarm)
    {
        PlayerMissile missile = Create<PlayerMissile>(pool);
        missile._enemySwarm = enemySwarm;
        return missile;
    }

    private void Update()
    {
        _enemySwarm.OnPlayerMissileUpdate(this);
    }
}
