using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class PlayerExplosion : MonoBehaviour
{
    public PlayerExplosion Create(Vector3 position, Transform parent)
    {
        return Instantiate(this, position, Quaternion.identity, parent);
    }

    public void Play(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        GetComponent<ParticleSystem>().Play();
    }

    private void OnParticleSystemStopped()
    {
        gameObject.SetActive(false);
        GameManager.Instance.StartFailRoutine();
    }
}
