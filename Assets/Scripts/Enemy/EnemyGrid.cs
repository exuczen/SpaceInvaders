using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MustHave.Utilities;

public class EnemyGrid : MonoBehaviour
{
    [SerializeField]
    protected Grid _grid = default;
    [SerializeField, Range(5, 8)]
    protected int _enemyRowsMax = 8;
    [SerializeField, Range(3, 5)]
    protected int _enemyRowsMin = 5;
    [SerializeField, Range(8, 12)]
    protected int _enemyCols = 12;
    [SerializeField, Range(16, 20)]
    protected int _viewRows = 18;
    [SerializeField, Range(14, 18)]
    protected int _viewCols = 16;

    protected Vector2 _viewSize = default;

    public Grid Grid { get => _grid; }
    public Vector2 ViewSize { get => _viewSize; }

    private void SetViewSize()
    {
        Vector2 cellSize = _grid.cellSize;
        _viewSize = new Vector2(cellSize.x * _viewCols, cellSize.y * _viewRows);
    }

    public void SetViewSize(Camera camera, SpriteRenderer background)
    {
#if UNITY_EDITOR
        if (this && (Application.isPlaying || !UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode))
#endif
        {
            SetViewSize();
            SetCameraSize(camera);
            SetBackgroundSpriteScale(background);
        }
    }

    public Vector3 GetCellWorldPosition(Vector2Int cell)
    {
        Vector3 position = _grid.GetCellCenterWorld(new Vector3Int(cell.x, cell.y, 0));
        position.z = 0f;
        return position;
    }

    private void SetBackgroundSpriteScale(SpriteRenderer backgroundSprite)
    {
        Vector2 bgSpriteSize = backgroundSprite.sprite.bounds.size;
        backgroundSprite.transform.localScale = _viewSize / bgSpriteSize;
    }

    private void SetCameraSize(Camera camera)
    {
        float screenAspect = 1f * Screen.width / Screen.height;
        float viewGridAspect = 1f * _viewCols / _viewRows;
        Vector2 cellSize = _grid.cellSize;
        if (screenAspect > viewGridAspect)
        {
            float cameraHalfHeight = _viewRows * cellSize.y / 2f;
            camera.orthographicSize = cameraHalfHeight;
        }
        else
        {
            float cameraHalfWidth = _viewCols * cellSize.x / 2f;
            camera.orthographicSize = cameraHalfWidth / camera.aspect;
        }
    }
}