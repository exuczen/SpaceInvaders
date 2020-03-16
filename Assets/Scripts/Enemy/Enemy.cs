using MustHave.DesignPatterns;
using MustHave.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PoolObject
{
    [SerializeField]
    private EnemyMissileLauncher _missileLauncher = default;
    [SerializeField, Tooltip("If false, enemy specs is loaded from Application.persistentDataPath/EnemySpecs, otherwise inspector values are used.")]
    private bool _useInspectorParams = default;

    [SerializeField, ConditionalHide("_useInspectorParams", true)]
    private int _healthPoints = default;
    [SerializeField, ConditionalHide("_useInspectorParams", true)]
    private int _missileDamageHP = 1;
    [SerializeField, ConditionalHide("_useInspectorParams", true)]
    private float _missileSpeed = default;
    [SerializeField, ConditionalHide("_useInspectorParams", true)]
    private float _shotIntevalMin = default;
    [SerializeField, ConditionalHide("_useInspectorParams", true)]
    private float _shotIntevalMax = default;

    private SpriteRenderer _sprite = null;

    private int _currentHP = default;

    public SpriteRenderer Sprite { get => _sprite; }
    public int HealthPoints { get => _healthPoints; }
    public int MissileDamageHP { get => _missileDamageHP; }
    public float MissileSpeed { get => _missileSpeed; }
    public float ShotIntevalMin { get => _shotIntevalMin; }
    public float ShotIntevalMax { get => _shotIntevalMax; }

    public bool UseInspectorParams { get => _useInspectorParams; }

    private void Initialize(Transform pool)
    {
        _pool = pool;
        _sprite = GetComponent<SpriteRenderer>();
        _currentHP = _healthPoints;

        transform.localPosition = Vector3.zero;
    }

    public void StartShooting(EnemyMissileHandler handler, float shotIntervalFactor)
    {
        _missileLauncher.StartShooting(handler, this, shotIntervalFactor);
    }

    public void StopShooting()
    {
        _missileLauncher.StopShooting();
    }

    public void SetSpriteScaleToFitCellSize(Vector2 cellSize, float normalizedSize)
    {
        float scale = GetSpriteScale(cellSize, normalizedSize);
        transform.localScale = new Vector2(scale, scale);
    }

    private float GetSpriteScale(Vector2 cellSize, float normalizedSize)
    {
        _sprite = GetComponent<SpriteRenderer>();
        Vector2 spriteSize = _sprite.sprite.bounds.size;
        float scale = spriteSize.x > spriteSize.y ? cellSize.x / spriteSize.x : cellSize.y / spriteSize.y;
        return normalizedSize * scale;
    }

    public void SetCellPosition(EnemyGrid grid, Vector2Int cell)
    {
        transform.position = grid.GetCellWorldPosition(cell);
    }

    public Enemy Create(Transform pool)
    {
        Enemy enemy = Instantiate(this, pool);
        enemy.Initialize(pool);
        return enemy;
    }

    public void OnHitByMissile(PlayerMissile missile, EnemyParticlesHandler _particlesHandler, out bool destroyed)
    {
        _currentHP = Mathf.Max(0, _currentHP - missile.DamageHP);
        destroyed = _currentHP <= 0;
        if (destroyed)
        {
            _particlesHandler.StartExplosion(transform.position, _missileLauncher.MissileColor);
            _missileLauncher.StopShooting();
            MoveToPool();
        }
        else
        {
            _particlesHandler.StartHitParticles(transform.position, _missileLauncher.MissileColor);
        }
    }

    public void SaveParamsToJson()
    {
        EnemyParams.SaveToJson(this);
        Debug.Log(GetType() + ".SaveParamsToJson: " + "EnemyParams saved to " + EnemyParams.GetPath(name));
    }

    public void LoadParamsFromJson()
    {
        EnemyParams enemyParams = EnemyParams.LoadFromJson(name);
        if (enemyParams != null)
        {
            _healthPoints = enemyParams.HealthPoints;
            _missileDamageHP = enemyParams.MissileDamageHP;
            _missileSpeed = enemyParams.MissileSpeed;
            _shotIntevalMin = enemyParams.ShotIntevalMin;
            _shotIntevalMax = enemyParams.ShotIntevalMax;
            Debug.Log(GetType() + ".LoadParamsFromJson: " + "EnemyParams loaded from " + EnemyParams.GetPath(name));
        }
        else
        {
            Debug.LogWarning(GetType() + ".LoadParamsFromJson: " + EnemyParams.GetPath(name) + " does not exists.");
            SaveParamsToJson();
        }
    }

    protected override void OnMoveToPool()
    {
        _currentHP = _healthPoints;
    }
}
