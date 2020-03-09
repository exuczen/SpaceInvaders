using UnityEngine;

public class PlayerExplosion : HitParticles
{
    private void OnParticleSystemStopped()
    {
        gameObject.SetActive(false);
        GameManager.Instance.StartFailRoutine();
    }
}
