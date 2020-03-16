using MustHave.DesignPatterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParticlesHandler : ObjectPoolHandler<HitParticles>
{
    private void StartHitParticles(Vector3 position, Color color, float scaleFactor)
    {
        HitParticles explosion = GetObjectFromPool();
        explosion.SetColor(color);
        explosion.SetScaleFactor(scaleFactor);
        explosion.Play(position);
    }

    public void StartHitParticles(Vector3 position, Color color)
    {
        StartHitParticles(position, color, 0.7f);
    }

    public void StartExplosion(Vector3 position, Color color)
    {
        StartHitParticles(position, color, 1f);
    }

    protected override void CreateObjectInstance(HitParticles prefab, Transform pool)
    {
        prefab.CreateInPool<HitParticles>(pool);
    }
}
