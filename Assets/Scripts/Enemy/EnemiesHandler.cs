using MustHave.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesHandler : ObjectPoolHandler<Enemy>
{
    [SerializeField]
    protected float _normalizedSpriteSize = 1f;

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
            FillPool(i, POOL_INIT_CAPACITY);
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

    public void FillEnemyGrid(EnemyGrid enemyGrid, Enemy[] array, Vector2Int arrayOffset, int colsCount, int rowsCount, bool usePrevRowsCounts)
    {
        int[] rowsCounts = usePrevRowsCounts && _rowsCounts != null ? _rowsCounts : (_rowsCounts = GetEnemyRowsCounts(rowsCount));
        int rowOffset = 0;
        int yOffset = 0;
        for (int i = 0; i < _prefabs.Length; i++)
        {
            int prefabI = _prefabs.Length - 1 - i;
            int prefabRowsCount = rowsCounts[i];
            for (int y = 0; y < prefabRowsCount; y++)
            {
                int yWithOffset = y + yOffset;
                for (int x = 0; x < colsCount; x++)
                {
                    Enemy enemy = GetObjectFromPool(prefabI);
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

    protected override void CreateObjectInstance(Enemy prefab, Transform pool)
    {
        prefab.Create(pool);
    }
}
