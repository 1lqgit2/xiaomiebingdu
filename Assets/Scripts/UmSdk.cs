using System.Collections.Generic;
using Umeng;
using UnityEngine;

public class UmSdk
{
	public bool IsDebug;

	public UmSdk(bool isDebug)
	{
		if (isDebug)
		{
			UnityEngine.Debug.Log("untiy um init :");
		}
		Analytics.Start();
		SetDebugLog(isDebug);
	}

	public void SetDebugLog(bool islog)
	{
		IsDebug = islog;
		Analytics.SetLogEnabled(islog);
	}

	public void OnEvent(string eventID)
	{
		Analytics.Event(eventID);
		if (IsDebug)
		{
			UnityEngine.Debug.Log("unity um OnEvent：" + eventID);
		}
	}

	public void OnEvent(string eventID, string label)
	{
		Analytics.Event(eventID, label);
		if (IsDebug)
		{
			UnityEngine.Debug.Log("unity um OnEvent：" + eventID + " label：" + label);
		}
	}

	public void OnEvent(string eventID, string[] args, char splitChar)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		for (int i = 0; i < args.Length; i++)
		{
			string[] array = args[i].Split(splitChar);
			if (array.Length == 2)
			{
				dictionary.Add(array[0], array[1]);
			}
		}
		Analytics.Event(eventID, dictionary);
		if (IsDebug)
		{
			string text = string.Empty;
			for (int j = 0; j < args.Length; j++)
			{
				text = text + args[j] + "##";
			}
			UnityEngine.Debug.Log("unity um OnEvent：" + eventID + " args：" + text);
		}
	}

	public void OnEventValue(string eventID, string[] args, char splitChar, int value)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		for (int i = 0; i < args.Length; i++)
		{
			string[] array = args[i].Split(splitChar);
			if (array.Length == 2)
			{
				dictionary.Add(array[0], array[1]);
			}
		}
		Analytics.Event(eventID, dictionary, value);
		if (IsDebug)
		{
			string text = string.Empty;
			for (int j = 0; j < args.Length; j++)
			{
				text = text + args[j] + "##";
			}
			UnityEngine.Debug.Log("unity um OnEvent：" + eventID + " args：" + text + " value：" + value);
		}
	}

	public void StartLevel(string levelID)
	{
		GA.StartLevel(levelID);
		if (IsDebug)
		{
			UnityEngine.Debug.Log("unity um StartLevel：" + levelID);
		}
	}

	public void FinishLevel(string levelID)
	{
		GA.FinishLevel(levelID);
		if (IsDebug)
		{
			UnityEngine.Debug.Log("unity um FinishLevel：" + levelID);
		}
	}

	public void FailLevel(string levelID)
	{
		GA.FailLevel(levelID);
		if (IsDebug)
		{
			UnityEngine.Debug.Log("unity um FailLevel：" + levelID);
		}
	}
}
