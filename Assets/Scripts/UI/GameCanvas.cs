using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using MustHave.Utilities;
using UnityEditor;
using MustHave.UI;
using System;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GameCanvas : CanvasScript
{
    [SerializeField]
    private SplashScreen _splashScreen = default;
    [SerializeField]
    private GameScreen _gameScreen = default;
    [SerializeField]
    private Player _player = default;
    [SerializeField]
    private EnemySwarm _enemySwarm = default;
    [SerializeField]
    private Camera _camera = default;
    [SerializeField]
    private SpriteRenderer _backgroundSprite = default;

    public GameScreen GameScreen { get => _gameScreen; }

    protected override void Start()
    {
        SetGameViewSize();
    }

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();
#if UNITY_EDITOR
        if (this && _camera && _backgroundSprite)
#endif
        {
            SetGameViewSize();
        }
    }

    private void SetGameViewSize()
    {
        _enemySwarm.SetViewSize(_camera, _backgroundSprite);
        _player.ViewSize = _enemySwarm.ViewSize;
        SetGameScreenAnchors(_enemySwarm.ViewSize);
    }

    private void SetGameScreenAnchors(Vector2 viewSize)
    {
        Vector2 cameraSize = new Vector2(2f * _camera.orthographicSize * _camera.aspect, 2f * _camera.orthographicSize);
        Vector2 normalizedViewSize = viewSize / cameraSize;
        RectTransform screenRect = _gameScreen.transform as RectTransform;
        Vector2 anchorOffset = (Vector2.one - normalizedViewSize) / 2f;
        screenRect.anchorMin = anchorOffset;
        screenRect.anchorMax = Vector2.one - anchorOffset;
    }

    protected override void OnAppAwake(bool active)
    {
        _player.setPlayerHeathText = _gameScreen.SetPlayerHealthText;

        ShowScreen(_splashScreen, false, false);

        AlertPopup.SetQuitWarningActions(() => { GameManager.Instance.SetGameActive(false); }, () => { GameManager.Instance.SetGameActive(true); });
    }

    public void ShowFailPopup(Action onRestartClick)
    {
        AlertPopup.SetText("You failed. Restart the game.").
            SetButtons(ActionWithText.Create("Restart", onRestartClick)
        ).Show();
    }

    public void ShowSuccessPopup(Action onNextLevelClick, int nextLevel)
    {
        AlertPopup.SetText("You won!!!").
            SetButtons(ActionWithText.Create("Next Level", () => {
                _gameScreen.SetLevelText(nextLevel);
                onNextLevelClick();
            })
        ).Show();
    }
}
