using UnityEngine;

public static class Vector2Extension
{
    public static Vector2 Vector2FromAngle(this Vector2 vector, float angle)
    {
        angle *= Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    public static Vector2 SetMagnitude(this Vector2 vector, float magnitude)
    {
        return vector.normalized * magnitude;
    }

    public static Vector2 ChangeMagnitude(this Vector2 vector, float deltaMagnitude)
    {
        var magnitude = vector.magnitude + deltaMagnitude;
        magnitude = Mathf.Max(magnitude, 0);
        return vector.normalized * magnitude;
    }
    
    public static Vector3 ToVector3XY(this Vector2 vector, float z = default)
    {
        return new Vector3(vector.x, vector.y, z);
    }
}