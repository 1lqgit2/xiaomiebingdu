using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	internal class LuckyDrawManager : Singleton<LuckyDrawManager>
	{
		private int IsLuckyBox;

		private List<LuckyDrawVo> list = new List<LuckyDrawVo>();

		public void InitLuckyBoxTime(int draw)
		{
			IsLuckyBox = draw;
		}

		public void SetLuckyBoxTime(bool draw)
		{
			IsLuckyBox = (draw ? 1 : 0);
			PlayerPrefs.SetInt("IsLuckyBox", IsLuckyBox);
		}

		public void InitList()
		{
			LuckyDrawStruct[] luckyDrawJson = Singleton<GameJson>.Instance.LuckyDrawJson;
			for (int i = 0; i < luckyDrawJson.Length; i++)
			{
				LuckyDrawStruct luckyDrawStruct = luckyDrawJson[i];
				LuckyDrawVo item = new LuckyDrawVo(luckyDrawStruct.Type, luckyDrawStruct.ItemId, (float)luckyDrawStruct.AddNum, (float)luckyDrawStruct.Rate);
				list.Add(item);
			}
		}

		public bool CanLuckyDraw()
		{
			if (IsLuckyBox == 0)
			{
				return true;
			}
			return false;
		}

		public LuckyDrawVo RandomVo()
		{
			float num = 0f;
			for (int i = 0; i < list.Count; i++)
			{
				LuckyDrawVo luckyDrawVo = list[i];
				num += luckyDrawVo.GetRate();
			}
			float num2 = Random.Range(0f, num);
			float num3 = 0f;
			for (int j = 0; j < list.Count; j++)
			{
				LuckyDrawVo luckyDrawVo = list[j];
				num3 = luckyDrawVo.GetRate();
				if (num2 <= num3)
				{
					return luckyDrawVo;
				}
				num2 -= num3;
			}
			UnityEngine.Debug.LogError("[LuckyDrawManager]RandomVo , random luckydraw vo failed.");
			return null;
		}

		public LuckyDrawVo LuckyDraw()
		{
			if (!CanLuckyDraw())
			{
				return null;
			}
			SetLuckyBoxTime(draw: true);
			return RandomVo();
		}

		public void AddLuckyDraw(LuckyDrawVo vo)
		{
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(110);
			if (vo.GetItemId() > 0)
			{
				Singleton<CoinManager>.Instance.FloatByCircle<FloatProp>("GameObject/Item/FloatProp", 2, Vector2.zero, GameGlobalData.MainViewPropBtnPos, 0.25f, delegate
				{
					Singleton<BattleItemManager>.Instance.SetBattleItemNumber(vo.GetItemId(), (int)vo.GetAddNum());
				}, layerTran);
			}
			else
			{
				switch (vo.GetDrawType())
				{
				case 6:
				case 7:
					Singleton<CoinManager>.Instance.FloatByCircle<FloatGold>("GameObject/Item/FloatCoin", 20, Vector2.zero, GameGlobalData.CoinRecoverTopLeftPos, 0.25f, delegate
					{
						float fireCost = GameGlobalData.GetFireCost(GameGlobalData.GetFirePowerData());
						GameGlobalData.AddCurCoin(vo.GetAddNum() * fireCost);
					}, layerTran);
					break;
				case 8:
					Singleton<CoinManager>.Instance.FloatByCircle<FloatDiamond>("GameObject/Item/FloatDiamond", 20, Vector3.zero, GameGlobalData.DiamondRecoverTopLeftPos, 0.25f, delegate
					{
						Singleton<DiamondManager>.Instance.VipFreeDiamond();
					}, layerTran);
					break;
				}
			}
			GameAnalysisData.DrawItem(vo.GetDrawType().ToString(), vo.GetAddNum().ToString());
		}
	}
}
