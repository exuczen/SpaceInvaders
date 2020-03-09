using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosionHandler : ObjectPoolHandler<EnemyExplosion>
{
    public void StartExplosion(Vector3 position, Color color)
    {
        EnemyExplosion explosion = GetObjectFromPool();
        explosion.transform.SetParent(_container);
        explosion.SetColor(color);
        explosion.Play(position);
    }

    protected override void FillPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _prefab.CreateInPool<EnemyExplosion>(_pool);
        }
    }
}
