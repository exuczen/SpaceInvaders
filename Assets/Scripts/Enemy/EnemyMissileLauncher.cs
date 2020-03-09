using MustHave.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissileLauncher : MissileLauncher
{
    private Coroutine _shootingRoutine = default;

    public void StartShooting(MissileHandler handler, float intervalScale, float intervalMin, float intervalMax, float speed, int damageHP)
    {
        StopShooting();
        _shootingRoutine = StartCoroutine(ShootingRoutine(handler, intervalScale, intervalMin, intervalMax, speed, damageHP));
    }

    public void StopShooting()
    {
        if (_shootingRoutine != null)
        {
            StopCoroutine(_shootingRoutine);
            _shootingRoutine = null;
        }
    }

    private IEnumerator ShootingRoutine(MissileHandler handler, float intervalScale, float intervalMin, float intervalMax, float speed, int damageHP)
    {
        yield return new WaitForSeconds(intervalScale * Random.Range(0f, intervalMax));
        while (true)
        {
            Shoot(handler, -Mathf.Abs(speed), damageHP);
            yield return new WaitForSeconds(intervalScale * Random.Range(intervalMin, intervalMax));
        }
    }
}
