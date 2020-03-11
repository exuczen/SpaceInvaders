using MustHave.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesHandler : MonoBehaviour
{
    [SerializeField]
    protected float _normalizedSpriteSize = 1f;
    [SerializeField]
    private Enemy[] _prefabs = default;
    [SerializeField]
    private Transform[] _containers = default;
    [SerializeField]
    private Transform[] _pools = default;

    private const int POOL_INIT_CAPACITY = 50;
    private const int POOL_DELTA_CAPACITY = 5;

    private int[] _rowsCounts = null;

    public void Initialize(Grid grid)
    {
        foreach (Transform container in _containers)
        {
            container.DestroyAllChildren();
        }
        foreach (Enemy prefab in _prefabs)
        {
            if (!prefab.UseInspectorParams)
            {
                prefab.LoadParamsFromJson();
            }
            prefab.SetSpriteScaleToFitCellSize(grid.cellSize, _normalizedSpriteSize);
        }
        for (int i = 0; i < _prefabs.Length; i++)
        {
            FillEnemyPool(i, POOL_INIT_CAPACITY);
        }
    }

    public int GetEnemiesCount()
    {
        int count = 0;
        foreach (Transform container in _containers)
        {
            count += container.childCount;
        }
        return count;
    }

    private Enemy GetEnemyFromPool(int poolIndex)
    {
        Transform enemyPool = _pools[poolIndex];
        Transform container = _containers[poolIndex];
        if (enemyPool.childCount == 0)
        {
            FillEnemyPool(poolIndex, POOL_DELTA_CAPACITY);
        }
        Enemy enemy = enemyPool.GetChild(enemyPool.childCount - 1).GetComponent<Enemy>();
        enemy.ResetMutableParams();
        enemy.transform.SetParent(container, false);
        enemy.gameObject.SetActive(true);
        return enemy;
    }

    private void FillEnemyPool(int poolIndex, int enemyCount)
    {
        Enemy prefab = _prefabs[poolIndex];
        Transform enemyPool = _pools[poolIndex];
        for (int i = 0; i < enemyCount; i++)
        {
            prefab.Create(enemyPool);
        }
    }

    public void FillEnemyGrid(EnemyGrid enemyGrid, Enemy[] array, Vector2Int arrayOffset, int colsCount, int rowsCount, bool usePrevRowsCounts)
    {
        int[] rowsCounts = usePrevRowsCounts && _rowsCounts != null ? _rowsCounts : (_rowsCounts = GetEnemyRowsCounts(rowsCount));
        int rowOffset = 0;
        int yOffset = 0;
        for (int i = 0; i < _prefabs.Length; i++)
        {
            int prefabI = _prefabs.Length - 1 - i;
            Enemy prefab = _prefabs[prefabI];
            Transform container = _containers[prefabI];
            int prefabRowsCount = rowsCounts[i];
            for (int y = 0; y < prefabRowsCount; y++)
            {
                int yWithOffset = y + yOffset;
                for (int x = 0; x < colsCount; x++)
                {
                    Enemy enemy = GetEnemyFromPool(prefabI);
                    enemy.SetCellPosition(enemyGrid, new Vector2Int(x - arrayOffset.x, arrayOffset.y - yWithOffset));
                    array[x + rowOffset] = enemy;
                }
                rowOffset += colsCount;
            }
            yOffset += prefabRowsCount;
        }
    }

    private int[] GetEnemyRowsCounts(int enemyRowsTotal)
    {
        int rowsCount = 0;
        int[] rowsCounts = new int[_prefabs.Length];
        int rowsLeft = enemyRowsTotal;
        int rowsMin = enemyRowsTotal > 6 ? 2 : 1;
        int rowsHalfTotal = enemyRowsTotal >> 1;
        for (int i = 0; i < rowsCounts.Length - 1; i++)
        {
            int rowsMax = Mathf.Min(rowsLeft - (rowsCounts.Length - 1 - i) * rowsMin, rowsHalfTotal);
            rowsCounts[i] = Random.Range(rowsMin, 1 + rowsMax);
            rowsCount += rowsCounts[i];
            rowsLeft = enemyRowsTotal - rowsCount;
        }
        rowsCounts[rowsCounts.Length - 1] = rowsLeft;
        rowsCount += rowsLeft;

        List<int> rowsCountsList = rowsCounts.ToList();
        rowsCountsList.Sort();
        return rowsCountsList.ToArray();
    }

    public void ClearContainers()
    {
        for (int i = 0; i < _containers.Length; i++)
        {
            Transform pool = _pools[i];
            Transform container = _containers[i];
            Transform[] children = new Transform[container.childCount];
            for (int j = 0; j < container.childCount; j++)
            {
                children[j] = container.GetChild(j);
            }
            foreach (Transform child in children)
            {
                child.SetParent(pool, false);
                child.gameObject.SetActive(false);
            }
        }
    }
}
