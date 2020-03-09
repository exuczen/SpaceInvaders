using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MustHave.Utilities;
using UnityEngine.SceneManagement;
using System.Linq;

public class EnemySwarm : EnemyGrid
{
    [SerializeField]
    private float _horizontalSpeed = 1f;

    private MissileHandler _missileHandler = default;
    private EnemiesHandler _enemiesHandler = default;

    private Enemy[] _array = default;
    private Vector2Int _arrayOffset = default;

    protected int _rowsCount = default;
    protected int _colsCount = default;

    private Vector2 _swarmSize = default;
    private Vector2 _velocity = default;

    public MissileHandler MissileHandler { get => _missileHandler; }

    public void Initialize()
    {
        _missileHandler = GetComponent<MissileHandler>();
        _enemiesHandler = GetComponent<EnemiesHandler>();
        _enemiesHandler.Initialize(_grid);
    }

    private void Update()
    {
        float gapHalfWidth = (_viewSize.x - _swarmSize.x) / 2f;
        Vector2 deltaPosition = _velocity * Time.deltaTime;
        Vector2 nextPosition = (Vector2)transform.localPosition + deltaPosition;
        float outerDistance = Mathf.Abs(nextPosition.x) - gapHalfWidth;
        if (outerDistance > 0f)
        {
            float sign = Mathf.Sign(nextPosition.x);
            _velocity.x = -sign * _horizontalSpeed;
            nextPosition.x -= 2f * sign * outerDistance;
        }
        transform.localPosition = nextPosition;
    }

    public void ClearMissileContainer()
    {
        _missileHandler.ClearContainer();
    }

    public void StartShooting()
    {
        List<Enemy> enemiesAtTop = GetEnemiesAtTop();
        float shotIntervalFactor = 1f * enemiesAtTop.Count / _colsCount;
        foreach (var enemy in enemiesAtTop)
        {
            enemy.StartShooting(_missileHandler, shotIntervalFactor);
        }
    }

    public void StopShooting()
    {
        for (int y = 0; y < _rowsCount; y++)
        {
            int rowOffset = y * _colsCount;
            for (int x = 0; x < _colsCount; x++)
            {
                Enemy enemy = _array[x + rowOffset];
                if (enemy)
                {
                    enemy.StopShooting();
                }
            }
        }
    }

    private Enemy GetEnemy(int x, int y)
    {
        if (x >= 0 && x < _colsCount && y >= 0 && y < _rowsCount)
        {
            return _array[x + y * _colsCount];
        }
        return null;
    }

    private void SetEnemyArrayElement(Enemy enemy, Vector2Int xy)
    {
        SetEnemyArrayElement(enemy, xy.x, xy.y);
    }

    private void SetEnemyArrayElement(Enemy enemy, int x, int y)
    {
        if (x >= 0 && x < _colsCount && y >= 0 && y < _rowsCount)
        {
            _array[x + y * _colsCount] = enemy;
        }
    }

    private Enemy GetEnemyAtCell(Vector2Int cell, out Vector2Int xy)
    {
        int x = _arrayOffset.x + cell.x;
        int y = _arrayOffset.y - cell.y;
        xy = new Vector2Int(x, y);
        return GetEnemy(x, y);
    }

    private List<Enemy> GetEnemiesAtTop()
    {
        List<Enemy> enemies = new List<Enemy>();
        for (int x = 0; x < _colsCount; x++)
        {
            int y = _rowsCount - 1;
            Enemy enemy = null;
            while (y >= 0 && (enemy = GetEnemy(x, y)) == null)
            {
                y--;
            }
            if (enemy)
            {
                enemies.Add(enemy);
            }
        }
        return enemies;
    }


    private int GetNonEmptyEnemyColumnsCount()
    {
        List<Enemy> enemies = new List<Enemy>();
        int count = 0;
        for (int x = 0; x < _colsCount; x++)
        {
            int y = 0;
            Enemy enemy = null;
            while (y < _rowsCount && (enemy = GetEnemy(x, y)) == null)
            {
                y++;
            }
            if (enemy)
            {
                count++;
            }
        }
        return count;
    }

    private void CreateArray(int level)
    {
        _colsCount = _enemyCols;
        _rowsCount = Mathf.Clamp(level + 2, _enemyRowsMin, _enemyRowsMax);
        _array = new Enemy[_colsCount * _rowsCount];
        _arrayOffset = new Vector2Int(_colsCount >> 1, (_viewRows >> 1) - 3);
        _swarmSize = new Vector2(_colsCount * _grid.cellSize.x, _rowsCount * _grid.cellSize.y);
    }

    private void ClearArray()
    {
        for (int i = 0; i < _array.Length; i++)
        {
            _array[i] = null;
        }
    }

    public void ResetSwarm(bool isNextLevel, int level)
    {
        if (isNextLevel)
        {
            CreateArray(level);
        }
        else
        {
            ClearArray();
        }

        _missileHandler.ClearContainer();
        _enemiesHandler.ClearContainers();
        _enemiesHandler.FillEnemyGrid(this, _array, _arrayOffset, _colsCount, _rowsCount, !isNextLevel);

        _velocity = new Vector2(_horizontalSpeed, 0f);

        transform.localPosition = Vector3.zero;
    }

    public void OnPlayerMissileUpdate(PlayerMissile missile)
    {
        Vector2Int cell = (Vector2Int)_grid.WorldToCell(missile.transform.position);
        Enemy enemy = GetEnemyAtCell(cell, out Vector2Int xy);
        //Debug.Log(GetType() + ".OnPlayerMissileUpdate: " + collider.bounds.ToString("f2"));
        if (enemy && missile.Collider.bounds.Intersects(enemy.Sprite.bounds))
        {
            //Debug.Log(GetType() + ".OnPlayerMissileUpdate: " + enemy.Sprite.bounds.ToString("f2") + " " + missile.Collider.bounds.ToString("f2"));
            //Debug.Log(GetType() + ".OnPlayerMissileUpdate: " + missile.Collider.bounds.Intersects(enemy.Sprite.bounds));
            enemy.OnHitByMissile(missile, out bool enemyDestoyed);
            missile.OnHit();
            if (enemyDestoyed)
            {
                SetEnemyArrayElement(null, xy);
                int enemiesCount = _enemiesHandler.GetEnemiesCount();
                if (enemiesCount == 0)
                {
                    GameManager.Instance.StartSuccessRoutine();
                }
                else if (xy.y > 0)
                {
                    int x = xy.x;
                    int y = xy.y - 1;
                    Enemy enemyBelow = null;
                    while (y >= 0 && (enemyBelow = GetEnemy(x, y)) == null)
                    {
                        y--;
                    }
                    if (enemyBelow)
                    {
                        //Debug.Log(GetType() + ".enemyBelow: " + x + " " + y);
                        //Debug.Log(GetType() + ".GetNonEmptyEnemyColumnsCount()=" + GetNonEmptyEnemyColumnsCount());
                        float shotIntervalFactor = 1f * GetNonEmptyEnemyColumnsCount() / _colsCount;
                        enemyBelow.StartShooting(_missileHandler, shotIntervalFactor);
                    }
                }
            }
        }
    }
}
