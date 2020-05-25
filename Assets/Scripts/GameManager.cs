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
    private int _level = 1;

    private Action<Action, int> _onFail = default;
    private Action<Action, int> _onSuccess = default;

    public int Level { get => _level; }

    protected override void OnAwake()
    {
        _enemySwarm.Initialize();
        _enemySwarm.ResetSwarm(true, _level);
        SetGameActive(false);
    }

    public void SetResultCallbacks(Action<Action, int> onSuccess, Action<Action, int> onFail)
    {
        _onSuccess = onSuccess;
        _onFail = onFail;
    }

    private IEnumerator FailRoutine()
    {
        SetGameActive(false);
        yield return new WaitForEndOfFrame();
        _onFail?.Invoke(() => {
            _enemySwarm.ResetSwarm(false, _level);
            _player.Restart();
            SetGameActive(true);
        }, _level);
    }

    private IEnumerator SuccessRoutine()
    {
        SetGameActive(false);
        yield return new WaitForEndOfFrame();
        _level++;
        _onSuccess?.Invoke(() => {
            _enemySwarm.ResetSwarm(true, _level);
            _player.Restart();
            SetGameActive(true);
        }, _level);
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
        _enemySwarm.MissileHandler.SetMissilesSimulated(active);
        _player.enabled = active;
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
