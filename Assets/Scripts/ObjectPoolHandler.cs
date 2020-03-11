using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPoolHandler<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField]
    protected T _prefab = default;
    [SerializeField]
    protected Transform _pool = default;
    [SerializeField]
    protected Transform _container = default;

    public const int POOL_INIT_CAPACITY = 50;
    public const int POOL_DELTA_CAPACITY = 5;

    public Transform Container { get => _container; }
    public Transform Pool { get => _pool; }

    private void Awake()
    {
        FillPool(POOL_INIT_CAPACITY);
    }

    protected abstract void FillPool(int count);

    public T GetObjectFromPool()
    {
        if (_pool.childCount == 0)
        {
            FillPool(POOL_DELTA_CAPACITY);
        }
        T child = _pool.GetChild(_pool.childCount - 1).GetComponent<T>();
        child.transform.SetParent(_container, false);
        child.gameObject.SetActive(true);
        return child;
    }

    public void ClearContainer()
    {
        Transform[] children = new Transform[_container.childCount];
        for (int j = 0; j < _container.childCount; j++)
        {
            children[j] = _container.GetChild(j);
        }
        foreach (Transform child in children)
        {
            child.SetParent(_pool, false);
            child.gameObject.SetActive(false);
        }
    }
}
