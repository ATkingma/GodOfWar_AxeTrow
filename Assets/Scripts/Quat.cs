using UnityEngine;
using System.Collections;
public static class Quat
{
	public static Vector3 GetQuadraticCurvePoint(float f, Vector3 p0, Vector3 p1, Vector3 p2)
	{
		float i = 1 - f;
		float ff = f * f;
		float ii = i * i;
		return (ii * p0) + (2 * i * f * p1) + (ff * p2);
	}
}