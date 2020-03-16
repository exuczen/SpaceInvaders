using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private MissileLauncher _missileLauncher = default;
    [SerializeField]
    private PlayerExplosion _explosionPrefab = default;
    [SerializeField]
    private HitParticles _hitParticlesPrefab = default;
    [SerializeField]
    private int _healthPoints = default;
    [SerializeField]
    private float _missileSpeed = default;
    [SerializeField]
    private int _missileDamageHP = default;

    private int _currentHP = default;

    private Vector2 _viewSize = default;

    private PlayerMissileHandler _missileHandler = default;

    private SpriteRenderer _sprite = default;

    private PlayerExplosion _explosion = default;

    private HitParticles _hitParticles = default;

    public Action<int> setPlayerHeathText = default;

    public PlayerMissileHandler MissileHandler { get => _missileHandler; }
    public Vector2 ViewSize { get => _viewSize; set => _viewSize = value; }

    private void Awake()
    {
        _missileHandler = GetComponent<PlayerMissileHandler>();
        _sprite = GetComponent<SpriteRenderer>();
        _explosion = _explosionPrefab.Create<PlayerExplosion>(transform.parent);
        _hitParticles = _hitParticlesPrefab.Create<HitParticles>(transform.parent);
        _explosion.gameObject.SetActive(false);
        _hitParticles.gameObject.SetActive(false);
    }

    private void Start()
    {
        SetCurrentHP(_healthPoints);
    }

    private void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        if (inputX != 0f)
        {
            Vector2 spriteSize = _sprite.bounds.size;
            float halfWidth = (_viewSize.x - spriteSize.x) / 2f;
            float nextPositionX = transform.localPosition.x + 5f * inputX * Time.deltaTime;
            nextPositionX = Mathf.Clamp(nextPositionX, -halfWidth, halfWidth);
            transform.localPosition = new Vector3(nextPositionX, 0f, 0f);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _missileLauncher.Shoot(_missileHandler, _missileSpeed, _missileDamageHP);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Layer.EnemyMissile)
        {
            Missile missile = collision.gameObject.GetComponent<Missile>();
            SetCurrentHP(Mathf.Max(0, _currentHP - missile.DamageHP));
            if (_currentHP <= 0)
            {
                gameObject.SetActive(false);
                _explosion.Play(transform.position);
            }
            else
            {
                _hitParticles.Play((collision.transform.position + transform.position) / 2f);
            }
            missile.OnHit();
        }
    }

    public void Restart()
    {
        gameObject.SetActive(true);
        transform.localPosition = Vector3.zero;

        _missileHandler.ClearContainers();

        SetCurrentHP(_healthPoints);
    }

    private void SetCurrentHP(int hp)
    {
        _currentHP = hp;
        setPlayerHeathText?.Invoke(hp);
    }
}
