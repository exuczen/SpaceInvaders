using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private int _damageHP = default;

    protected Transform _pool = default;
    private Rigidbody2D _rigidbody = default;
    private BoxCollider2D _collider = default;
    private SpriteRenderer _sprite = default;

    public BoxCollider2D Collider { get => _collider; }
    public int DamageHP { get => _damageHP; }
    public Rigidbody2D Rigidbody { get => _rigidbody; }

    private void Initialize(Transform pool)
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _pool = pool;
    }

    public T Create<T>(Transform pool) where T : Missile
    {
        T missile = Instantiate(this as T, pool);
        missile.Initialize(pool);
        return missile;
    }

    public void SetSpriteColor(Color color)
    {
        _sprite.color = color;
    }

    public void Launch(Vector3 worldPos, float speed, int damageHP)
    {
        transform.position = worldPos;
        _damageHP = damageHP;
        _rigidbody.simulated = true;
        _rigidbody.velocity = new Vector2(0f, speed);
    }

    public void OnHit()
    {
        _rigidbody.velocity = Vector2.zero;
        transform.SetParent(_pool, false);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (_rigidbody)
        {
            _rigidbody.velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Layer.ViewBounds)
        {
            OnHit();
        }
    }
}
