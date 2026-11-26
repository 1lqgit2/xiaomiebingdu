using Umeng;
using UnityEngine;

public class UmengManager : MonoBehaviour
{
	private void Awake()
	{
		Object.DontDestroyOnLoad(base.transform.gameObject);
	}

	private void OnApplicationPause(bool isPause)
	{
		if (isPause)
		{
			UnityEngine.Debug.Log("Umeng:----onPause");
			Analytics.onPause();
		}
		else
		{
			UnityEngine.Debug.Log("Umeng:----onResume");
			Analytics.onResume();
		}
	}

	private void OnApplicationQuit()
	{
		Analytics.onKillProcess();
	}
}
