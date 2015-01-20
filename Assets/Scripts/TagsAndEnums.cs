using UnityEngine;
using System.Collections;

public class TagsAndEnums : MonoBehaviour {
    public const string player = "playerShip";
    public const string ignore = "ignore";
    public const string enemy = "enemy";
    public const string terrain = "terrain";
    public const string projectile = "projectile";

    public enum ProjectileType
    {
        missile
    };
}
