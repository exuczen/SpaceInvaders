using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class HitParticles : MonoBehaviour
{
    protected Transform _pool = default;

    private Vector3 _initialScale = default;

    public T CreateInPool<T>(Transform pool) where T : HitParticles
    {
        T hitParticles = Instantiate(this as T, pool);
        hitParticles._pool = pool;
        hitParticles._initialScale = hitParticles.transform.localScale;
        return hitParticles;
    }

    public T Create<T>(Vector3 position, Transform parent) where T : HitParticles
    {
        T hitParticles = Instantiate(this as T, parent);
        hitParticles.transform.position = position;
        hitParticles._initialScale = hitParticles.transform.localScale;
        return hitParticles;
    }

    public void Play(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        GetComponent<ParticleSystem>().Play();
    }

    public void Set(Transform parent, Vector3 position, Color color, float scaleFactor)
    {
        SetColor(color);
        transform.SetParent(parent);
        transform.position = position;
        transform.localScale = scaleFactor * _initialScale;
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
            transform.localScale = _initialScale;
            transform.SetParent(_pool, false);
        }
    }
}
