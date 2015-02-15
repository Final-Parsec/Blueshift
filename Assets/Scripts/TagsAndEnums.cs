using UnityEngine;
using System.Collections;

public class TagsAndEnums : MonoBehaviour {
    public const string player = "playerShip";
    public const string ignore = "ignore";
    public const string enemy = "enemy";
    public const string terrain = "terrain";
    public const string projectile = "projectile";

    public static float GetSqrDistance(Vector3 a, Vector3 b)
    {
        return (a-b).sqrMagnitude;
    }

    public enum ProjectileType
    {
        missile,
		plasmaSphere,
		homerMissile
    };

	public enum AimingDirection
	{
		forward,
		back,
		left,
		right,
		up,
		down
	};
}
