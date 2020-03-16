using MustHave.DesignPatterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class HitParticles : PoolObject
{
    private Vector3 _initialScale = default;

    public T CreateInPool<T>(Transform pool) where T : HitParticles
    {
        T hitParticles = Create<T>(pool);
        hitParticles._pool = pool;
        return hitParticles;
    }

    public T Create<T>(Transform parent) where T : HitParticles
    {
        T hitParticles = Instantiate(this as T, parent);
        hitParticles.transform.localPosition = Vector3.zero;
        hitParticles._initialScale = hitParticles.transform.localScale;
        return hitParticles;
    }

    public void Play(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        GetComponent<ParticleSystem>().Play();
    }

    public void SetScaleFactor(float scaleFactor)
    {
        transform.localScale = scaleFactor * _initialScale;
    }

    public void SetColor(Color color)
    {
        ParticleSystem.MainModule mainModule = GetComponent<ParticleSystem>().main;
        mainModule.startColor = color;
    }

    protected virtual void OnParticleSystemStopped()
    {
        if (_pool)
        {
            MoveToPool();
        }
        else
        {
            transform.localScale = _initialScale;
            gameObject.SetActive(false);
        }
    }

    protected override void OnMoveToPool()
    {
        transform.localScale = _initialScale;
    }
}
