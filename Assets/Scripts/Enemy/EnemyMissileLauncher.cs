using MustHave.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissileLauncher : MissileLauncher
{
    private Coroutine _shootingRoutine = default;

    public void StartShooting(MissileHandler handler, Enemy enemy, float intervalScale)
    {
        StopShooting();
        _shootingRoutine = StartCoroutine(ShootingRoutine(handler, enemy, intervalScale));
    }

    public void StopShooting()
    {
        if (_shootingRoutine != null)
        {
            StopCoroutine(_shootingRoutine);
            _shootingRoutine = null;
        }
    }

    private IEnumerator ShootingRoutine(MissileHandler handler, Enemy enemy, float intervalScale)
    {
        yield return new WaitForSeconds(intervalScale * Random.Range(0f, enemy.ShotIntevalMax));
        while (true)
        {
            Shoot(handler, -Mathf.Abs(enemy.MissileSpeed), enemy.MissileDamageHP);
            yield return new WaitForSeconds(intervalScale * Random.Range(enemy.ShotIntevalMin, enemy.ShotIntevalMax));
        }
    }
}
