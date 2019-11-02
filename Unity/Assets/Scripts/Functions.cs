using UnityEngine;

public class Functions
{
	public static float AngleBetween(float originLat, float originLong, float destinationLat, float destinationLong)
	{
		float lat1 = DegreeToRadian(originLat);
		float lat2 = DegreeToRadian(destinationLat);
		float longDiff = DegreeToRadian(destinationLong - originLong);
		float y = Mathf.Sin(longDiff) * Mathf.Cos(lat2);
		float x = Mathf.Cos(lat1) * Mathf.Sin(lat2) - Mathf.Sin(lat1) * Mathf.Cos(lat2) * Mathf.Cos(longDiff);

		return (RadianToDegree(Mathf.Atan2(y, x)) + 360) % 360;
	}

	public static float RadianToDegree(float angle)
	{
		return angle * (180.0f / Mathf.PI);
	}

	public static float DistanceBetween(float originLat, float originLong, float destinationLat, float destinationLong)
	{
		const int R = 6371; // Radius of the earth

		float latDistance = DegreeToRadian(destinationLat - originLat);
		float lonDistance = DegreeToRadian(destinationLong - originLong);
		float a = Mathf.Sin(latDistance / 2) * Mathf.Sin(latDistance / 2)
				+ Mathf.Cos(DegreeToRadian(originLat)) * Mathf.Cos(DegreeToRadian(destinationLat))
				* Mathf.Sin(lonDistance / 2) * Mathf.Sin(lonDistance / 2);
		float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
		float distance = R * c * 1000;

		float height = 1 - 1;

		distance = Mathf.Pow(distance, 2) + Mathf.Pow(height, 2);

		return Mathf.Sqrt(distance);
	}

	public static float DegreeToRadian(float angle)
	{
		return Mathf.PI * angle / 180.0f;
	}
}
