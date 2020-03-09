using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using MustHave.Utilities;
using System.IO;

[Serializable]
public class EnemyParams
{
    private static readonly string FolderPath = Application.persistentDataPath + "/EnemySpecs/";

    [SerializeField]
    private int _healthPoints = default;
    [SerializeField]
    private int _missileDamageHP = 1;
    [SerializeField]
    private float _missileSpeed = default;
    [SerializeField]
    private float _shotIntevalMin = default;
    [SerializeField]
    private float _shotIntevalMax = default;

    public int HealthPoints { get => _healthPoints; }
    public int MissileDamageHP { get => _missileDamageHP; }
    public float MissileSpeed { get => _missileSpeed; }
    public float ShotIntevalMin { get => _shotIntevalMin; }
    public float ShotIntevalMax { get => _shotIntevalMax; }

    public EnemyParams(Enemy enemy)
    {
        _healthPoints = enemy.HealthPoints;
        _missileDamageHP = enemy.MissileDamageHP;
        _missileSpeed = enemy.MissileSpeed;
        _shotIntevalMin = enemy.ShotIntevalMin;
        _shotIntevalMax = enemy.ShotIntevalMax;
    }

    public static string GetPath(string enemyName)
    {
        return FolderPath + enemyName + "Specs.json";
    }

    public static void SaveToJson(Enemy enemy)
    {
        if (!Directory.Exists(FolderPath))
        {
            Directory.CreateDirectory(FolderPath);
        }
        EnemyParams enemyParams = new EnemyParams(enemy);
        JsonUtils.SaveToJson(enemyParams, GetPath(enemy.name));
    }

    public static EnemyParams LoadFromJson(string enemyName)
    {
        return JsonUtils.LoadFromJson<EnemyParams>(GetPath(enemyName));
    }
}
