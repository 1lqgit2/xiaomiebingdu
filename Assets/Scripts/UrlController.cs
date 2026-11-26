using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UrlController : MonoBehaviour
{
	public static UrlController Instance;

	private Dictionary<string, string> _postData = new Dictionary<string, string>();

	private void Awake()
	{
	}

	public void Init()
	{
		Instance = this;
	}

	public void SetGameUser()
	{
		_postData.Clear();
		_postData["gameName"] = Application.productName;
		_postData["mac"] = SystemInfo.deviceUniqueIdentifier;
		_postData["version"] = Application.version;
		_postData["platform"] = "release";
		_postData["gaid"] = GameGlobalData.GetGaid;
	}

	public void GetConfig(string packName, Action<string> action = null)
	{
		if (SkipLegacyRemoteApi("GetConfig"))
		{
			return;
		}

		string url = "https://sdk.ihippogame.com/api/v1/getConfig";
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["pkg_name"] = packName;
		dictionary["ver_code"] = "1";
		dictionary["ver_name"] = "1";
		dictionary["os"] = "android";
		StartCoroutine(Post(url, dictionary, action));
	}

	public void SaveData(string packName, string gaId, string userData, Action<string> action = null)
	{
		if (SkipLegacyRemoteApi("SaveData"))
		{
			return;
		}

		string url = "https://sdk.ihippogame.com/api/v1/saveData";
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["pkg_name"] = packName;
		dictionary["ver_code"] = "1";
		dictionary["ver_name"] = "1";
		dictionary["os"] = "android";
		dictionary["user_id"] = gaId;
		dictionary["user_data"] = userData;
		StartCoroutine(Post(url, dictionary, action));
	}

	public void GetData(string packName, string gaId, Action<string> action = null)
	{
		if (SkipLegacyRemoteApi("GetData"))
		{
			return;
		}

		string url = "https://sdk.ihippogame.com/api/v1/getData";
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary["pkg_name"] = packName;
		dictionary["ver_code"] = "1";
		dictionary["ver_name"] = "1";
		dictionary["os"] = "android";
		dictionary["user_id"] = gaId;
		StartCoroutine(Post(url, dictionary, action));
	}

	/// <summary>
	/// 抖音小游戏（WebGL 打包版）里不请求旧发行平台 sdk.ihippogame.com：
	/// 该域名不在小游戏后台白名单里，请求必然 errNo 21100 / HTTP 500，
	/// 只会刷屏；这条链路本来是旧渠道的远程配置和云存档，抖音包用不到。
	/// 编辑器与其它平台保持原逻辑，方便继续排查。
	/// </summary>
	private static bool SkipLegacyRemoteApi(string api)
	{
#if UNITY_WEBGL && !UNITY_EDITOR
		UnityEngine.Debug.Log("[UrlController] 跳过旧平台接口 " + api + "（抖音小游戏无域名白名单）");
		return true;
#else
		return false;
#endif
	}

	private IEnumerator PostUrl(string url, string eventName, Dictionary<string, string> postData, Action<string> action)
	{
		url = url + "/" + eventName;
		UnityWebRequest request = UnityWebRequest.Post(url, postData);
		yield return request.SendWebRequest();
		if (request.isHttpError)
		{
			UnityEngine.Debug.Log(request.error);
		}
		else if (request.responseCode == 200)
		{
			string text = request.downloadHandler.text;
		}
	}

	private IEnumerator Post(string url, Dictionary<string, string> postData, Action<string> action)
	{
		UnityWebRequest request = UnityWebRequest.Post(url, postData);
		yield return request.SendWebRequest();
		if (request.isHttpError)
		{
			UnityEngine.Debug.Log("[UrlController]request.responseCode = " + request.error);
		}
		else if (request.responseCode == 200)
		{
			string text = request.downloadHandler.text;
			if (action != null)
			{
				action(text);
			}
		}
		else
		{
			UnityEngine.Debug.Log("[UrlController]request.responseCode = " + request.responseCode);
		}
	}

	private IEnumerator GetUrl(string url, Action<string> action)
	{
		UnityWebRequest request = UnityWebRequest.Get(url);
		yield return request.SendWebRequest();
		if (request.isHttpError)
		{
			UnityEngine.Debug.Log(request.error);
		}
		else if (request.responseCode == 200)
		{
			string text = request.downloadHandler.text;
		}
	}
}
