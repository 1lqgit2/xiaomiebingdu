using orangesGame;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;

public class GameProxy : Proxy, IProxy
{
	private BallManger _ballMgr;

	public GameProxy(string NAME)
		: base(NAME)
	{
		_ballMgr = Singleton<BallManger>.Instance;
	}

	public void Start()
	{
		Timer.Register(1f, _ballMgr.Start);
	}

	public void SetLevelData()
	{
		if (GameGlobalData.GameTest == GameTestState.NoTest)
		{
			string aBTestKey = Singleton<AnalyzeSdk>.Instance.GetABTestKey("retry_limited");
			if (!string.IsNullOrEmpty(aBTestKey))
			{
				GameGlobalData.ReviveType = (ReviveState)int.Parse(aBTestKey);
			}
			switch (GameGlobalData.ReviveType)
			{
			default:
				GameGlobalData.ReviveCountMax = 1;
				break;
			case ReviveState.Three:
				GameGlobalData.ReviveCountMax = 3;
				break;
			case ReviveState.UnLimite:
				GameGlobalData.ReviveCountMax = 99999;
				break;
			}
		}
		int curLevel = GameGlobalData.CurLevel;
		LevelStruct[] levelJson = Singleton<GameJson>.Instance.LevelJson;
		if (levelJson.Length <= curLevel)
		{
			UnityEngine.Debug.LogError("levelJson is null:" + curLevel);
			return;
		}
		LevelStruct level = levelJson[curLevel];
		GameGlobalData.LevelType = (LevelType)level.Type;
		_ballMgr.Init(curLevel, level);
	}
}
