using UnityEngine;
using System.Collections;

public class Zhstar_MathUtil : MonoBehaviour
{
	public static bool EqualV3 (Vector3 a, Vector3 b)
	{
		if (a.x == b.x && a.y == b.y && a.z == b.z)
			return true;
		else
			return false;
	}
}
