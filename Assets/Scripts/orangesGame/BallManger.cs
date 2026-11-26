using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class BallManger : Singleton<BallManger>
	{
		private LevelCoomon _levelCoomon;

		private LevelBase _curLevel;

		private BallManger()
		{
			_levelCoomon = new LevelCoomon();
		}

		public void Init(int curLevel, LevelStruct level)
		{
			switch (level.Type)
			{
			case 0:
			case 2:
				_curLevel = _levelCoomon;
				break;
			}
			_curLevel.SetLevelData(curLevel, level);
		}

		public void Start()
		{
			_curLevel.SetTimer();
		}

		public float GetCurProgress()
		{
			return _curLevel.GetCurProgress();
		}

		public Transform GetBarrierTran()
		{
			return _curLevel.GetBarrierTran();
		}

		public Transform[] GetBarrierTrans()
		{
			return _curLevel.GetBarrierTrans();
		}

		public int GetProgress()
		{
			if (_curLevel == null)
			{
				return 0;
			}
			return _curLevel.LevelProgress;
		}

		public void Clean()
		{
			if (_curLevel != null)
			{
				_curLevel.CleanAllBallList();
			}
		}

		public void Destroy()
		{
			if (_curLevel != null)
			{
				_curLevel.Destroy();
			}
		}

		public void DestroyWithOutType(List<int> typeList)
		{
			if (_curLevel != null)
			{
				_curLevel.DestroyWithOutType(typeList);
			}
		}

		public void OnIce(bool isActive)
		{
			_curLevel.OnIce(isActive);
		}

		public void Pause()
		{
			_curLevel.Pause();
		}

		public void Stop()
		{
			_curLevel.Stop();
		}

		public void Continue()
		{
			_curLevel.Continue();
		}

		public void Divide(Vector3 pos, BallVO ballVO)
		{
			_curLevel.Divide(pos, ballVO);
		}

		public BarrierBase CreateOne(Vector3 pos, int venomID, float hitCount, int childFlag)
		{
			return _curLevel.CreateOneBall(pos, venomID, hitCount, childFlag);
		}

		public void Invading()
		{
			_levelCoomon.Invading();
		}
	}
}
