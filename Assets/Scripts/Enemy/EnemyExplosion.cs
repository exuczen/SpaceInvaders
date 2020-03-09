using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosion : HitParticles
{
    private void OnParticleSystemStopped()
    {
        gameObject.SetActive(false);
        transform.SetParent(_pool, false);
    }
}
