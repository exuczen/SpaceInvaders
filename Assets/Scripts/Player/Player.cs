using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private MissileLauncher _missileLauncher = default;
    [SerializeField]
    private int _healthPoints = default;
    [SerializeField]
    private float _missileSpeed = default;
    [SerializeField]
    private int _missileDamageHP = default;

    private int _currentHP = default;

    private Vector2 _viewSize = default;

    private MissileHandler _missileHandler = default;

    private SpriteRenderer _sprite = default;

    public Action<int> setPlayerHeathText = default;
    public MissileHandler MissileHandler { get => _missileHandler; }
    public Vector2 ViewSize { get => _viewSize; set => _viewSize = value; }

    private void Awake()
    {
        _missileHandler = GetComponent<MissileHandler>();
        _sprite = GetComponent<SpriteRenderer>();
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
            missile.OnHit();
            if (_currentHP <= 0)
            {
                GameManager.Instance.StartFailRoutine();
            }
        }
    }

    public void Restart()
    {
        _missileHandler.ClearContainer();

        transform.localPosition = Vector3.zero;
        SetCurrentHP(_healthPoints);
    }

    private void SetCurrentHP(int hp)
    {
        _currentHP = hp;
        setPlayerHeathText?.Invoke(hp);
    }
}
