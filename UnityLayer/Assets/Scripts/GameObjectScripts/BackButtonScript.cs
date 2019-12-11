using UnityEngine;

public class BackButtonScript : MonoBehaviour
{
	public static BackButtonScript instance;

	void Awake()
	{
		if (null == instance)
		{
			instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
	
	public void Back()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			{
				Application.Quit();
				System.Diagnostics.Process.GetCurrentProcess().Kill();
				AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
				activity.Call<bool>("moveTaskToBack", true);
			}
		}
	}
}