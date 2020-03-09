using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParticlesHandler : ObjectPoolHandler<HitParticles>
{
    public void StartHitParticles(Vector3 position, Color color)
    {
        HitParticles explosion = GetObjectFromPool();
        explosion.Set(_container, position, color, 0.7f);
        explosion.Play(position);
    }

    public void StartExplosion(Vector3 position, Color color)
    {
        HitParticles explosion = GetObjectFromPool();
        explosion.Set(_container, position, color, 1f);
        explosion.Play(position);
    }

    protected override void FillPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _prefab.CreateInPool<HitParticles>(_pool);
        }
    }
}
