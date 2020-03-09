using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class HitParticles : MonoBehaviour
{
    protected Transform _pool = default;

    public T CreateInPool<T>(Transform pool) where T : HitParticles
    {
        T hitParticles = Instantiate(this as T, pool);
        hitParticles._pool = pool;
        return hitParticles;
    }

    public T Create<T>(Vector3 position, Transform parent) where T : HitParticles
    {
        T hitParticles = Instantiate(this as T, parent);
        hitParticles.transform.position = position;
        return hitParticles;
    }

    public void Play(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        GetComponent<ParticleSystem>().Play();
    }

    public void SetColor(Color color)
    {
        ParticleSystem.MainModule mainModule = GetComponent<ParticleSystem>().main;
        mainModule.startColor = color;
    }

    protected virtual void OnParticleSystemStopped()
    {
        gameObject.SetActive(false);
        if (_pool)
        {
            transform.SetParent(_pool, false);
        }
    }
}
