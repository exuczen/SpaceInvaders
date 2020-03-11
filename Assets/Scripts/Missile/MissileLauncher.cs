using MustHave.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    [SerializeField]
    private Color _missileColor = Color.white;

    public Color MissileColor { get => _missileColor; }

    private void Awake()
    {
        transform.DestroyAllChildren();
    }

    public void Shoot(MissileHandler _handler, float speed, int damageHP)
    {
        Missile missile = _handler.GetObjectFromPool();
        missile.SetSpriteColor(_missileColor);
        missile.Launch(transform.position, speed, damageHP);
    }
}
