using UnityEngine;

public struct Layer
{
    public static readonly int ViewBounds = LayerMask.NameToLayer("ViewBounds");
    public static readonly int Enemy = LayerMask.NameToLayer("Enemy");
    public static readonly int EnemyMissile = LayerMask.NameToLayer("EnemyMissile");
    public static readonly int Player = LayerMask.NameToLayer("Player");
    public static readonly int PlayerMissile = LayerMask.NameToLayer("PlayerMissile");

    public static readonly int ViewBoundsMask = LayerMask.GetMask("ViewBounds");
    public static readonly int EnemyMask = LayerMask.GetMask("Enemy");
    public static readonly int EnemyMissileMask = LayerMask.GetMask("EnemyMissile");
    public static readonly int PlayerMask = LayerMask.GetMask("Player");
    public static readonly int PlayerMissileMask = LayerMask.GetMask("PlayerMissile");
}
