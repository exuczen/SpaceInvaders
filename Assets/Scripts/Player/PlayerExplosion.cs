using UnityEngine;

public class PlayerExplosion : HitParticles
{
    protected override void OnParticleSystemStopped()
    {
        gameObject.SetActive(false);
        GameManager.Instance.StartFailRoutine();
    }
}
