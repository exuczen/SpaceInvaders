using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MustHave.UI;
using MustHave.DesignPatterns;
using System;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private EnemySwarm _enemySwarm = default;
    [SerializeField]
    private Player _player = default;
    [SerializeField]
    private GameCanvas _gameCanvas = default;
    [SerializeField]
    private int _level = 1;

    protected override void OnAwake()
    {
        _player.setPlayerHeathText = _gameCanvas.GameScreen.SetPlayerHealthText;
    }

    public void InitGame()
    {
        _gameCanvas.AlertPopup.SetQuitWarningActions(() => { SetGameActive(false); }, () => { SetGameActive(true); });
        _gameCanvas.GameScreen.SetLevelText(_level);
        _enemySwarm.Initialize();
        _enemySwarm.ResetSwarm(true, _level);
        SetGameActive(false);
        _gameCanvas.AlertPopup.ShowWithConfirmButton("Start the game!", () => {
            SetGameActive(true);
        });
    }

    public void PauseGame(Action onResume)
    {
        SetGameActive(false);
        _gameCanvas.AlertPopup.ShowWithConfirmButton("Press OK to resume the game", () => {
            SetGameActive(true);
            onResume?.Invoke();
        });
    }

    private IEnumerator FailRoutine()
    {
        SetGameActive(false);
        yield return new WaitForEndOfFrame();
        _gameCanvas.ShowFailPopup(() => {
            _enemySwarm.ResetSwarm(false, _level);
            _player.Restart();
            SetGameActive(true);
        });
    }

    private IEnumerator SuccessRoutine()
    {
        SetGameActive(false);
        yield return new WaitForEndOfFrame();
        _gameCanvas.ShowSuccessPopup(() => {
            _level++;
            _enemySwarm.ResetSwarm(true, _level);
            _player.Restart();
            _gameCanvas.GameScreen.SetLevelText(_level);
            SetGameActive(true);
        });
    }

    public void StartFailRoutine()
    {
        StartCoroutine(FailRoutine());
    }

    public void StartSuccessRoutine()
    {
        StartCoroutine(SuccessRoutine());
    }

    public void SetGameActive(bool active)
    {
        _enemySwarm.enabled = active;
        _player.enabled = active;
        _enemySwarm.MissileHandler.SetMissilesSimulated(active);
        _player.MissileHandler.SetMissilesSimulated(active);
        if (!active)
        {
            _enemySwarm.StopShooting();
        }
        else
        {
            _enemySwarm.StartShooting();
        }
    }
}
