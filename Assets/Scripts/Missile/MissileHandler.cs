using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileHandler : MonoBehaviour
{
    [SerializeField]
    protected Missile _missilePrefab = default;
    [SerializeField]
    protected Transform _missilePool = default;
    [SerializeField]
    protected Transform _missileContainer = default;

    public const int MISSILE_POOL_INIT_CAPACITY = 50;
    public const int MISSILE_POOL_DELTA_CAPACITY = 5;

    public Transform MissileContainer { get => _missileContainer; }
    public Transform MissilePool { get => _missilePool; }

    private void Awake()
    {
        FillMissilePool(MISSILE_POOL_INIT_CAPACITY);
    }

    protected virtual void FillMissilePool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _missilePrefab.Create<Missile>(_missilePool);
        }
    }

    public Missile GetMissileFromPool()
    {
        if (_missilePool.childCount == 0)
        {
            FillMissilePool(MISSILE_POOL_DELTA_CAPACITY);
        }
        return _missilePool.GetChild(_missilePool.childCount - 1).GetComponent<Missile>();
    }

    public void ClearContainer()
    {
        Transform[] children = new Transform[_missileContainer.childCount];
        for (int j = 0; j < _missileContainer.childCount; j++)
        {
            children[j] = _missileContainer.GetChild(j);
        }
        foreach (Transform child in children)
        {
            child.SetParent(_missilePool);
            child.gameObject.SetActive(false);
        }
    }

    public void SetMissilesSimulated(bool value)
    {
        foreach (Transform child in _missileContainer)
        {
            child.GetComponent<Missile>().Rigidbody.simulated = value;
        }
    }
}
