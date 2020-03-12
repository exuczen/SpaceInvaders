using UnityEngine;

public class PlayerExplosion : HitParticles
{
    protected override void OnParticleSystemStopped()
    {
        base.OnParticleSystemStopped();
        GameManager.Instance.StartFailRoutine();
    }
}
