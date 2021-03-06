﻿using System.Collections;
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

    protected override void OnAppAwake(bool active)
    {
        _player.setPlayerHeathText = _gameScreen.SetPlayerHealthText;

        AlertPopup.SetQuitWarningActions(() => {
            GameManager.Instance.SetGameActive(false);
        }, () => {
            GameManager.Instance.SetGameActive(true);
        });

        GameManager.Instance.SetResultCallbacks(ShowSuccessPopup, ShowFailPopup);

        ShowScreen(_splashScreen, false, false);
    }

    public void ShowFailPopup(Action onRestartClick, int level)
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

    private void SetGameViewSize()
    {
        _enemySwarm.SetViewSize(_camera, _backgroundSprite);
        _player.ViewSize = _enemySwarm.ViewSize;
        _gameScreen.SetAnchors(_camera, _enemySwarm.ViewSize);
    }
}
