using UnityEngine;

static class MathUtils
{
    public static Vector3 GetNearestPointOnSegment(Vector3 a, Vector3 b, Vector3 target)
    {
        var AC = Vector3.Normalize(target - a);
        var normalAB = Vector3.Normalize(b - a);
        var finalScalar = Vector3.Dot(AC, normalAB);
        finalScalar = Mathf.Clamp(finalScalar, 0 , Vector3.Distance(a,b));

        var returnValue = a + normalAB * finalScalar;
        return returnValue;
    }
}
