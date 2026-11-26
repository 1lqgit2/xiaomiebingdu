using orangesGame;
using System;
using UnityEngine;

public class VipManager : Singleton<VipManager>
{
	public const int OPEN_MEMBER_VIEW_LEVEL = 30;

	private bool _vipStatus;

	private bool _vipShowView;

	private bool _isGotFreeDiamond;

	private bool _isCanFree;

	private string _vipDate;

	public bool VipCanFree
	{
		get
		{
			return _isCanFree;
		}
		set
		{
			if (_isCanFree)
			{
				PlayerPrefs.SetInt("IsCanFree", value ? 1 : 0);
				_isCanFree = value;
			}
		}
	}

	public string VipDate
	{
		get
		{
			return _vipDate;
		}
		set
		{
			_vipDate = value;
		}
	}

	public bool VipStatus
	{
		get
		{
			return _vipStatus;
		}
		set
		{
			_vipStatus = value;
		}
	}

	public bool IsShowVipView
	{
		get
		{
			return _vipShowView;
		}
		set
		{
			PlayerPrefs.SetInt("IsShowVIPView", value ? 1 : 0);
			_vipShowView = value;
		}
	}

	public bool IsGotFreeDiamond
	{
		get
		{
			return _isGotFreeDiamond;
		}
		set
		{
			PlayerPrefs.SetInt("IsGotFreeDiamond", value ? 1 : 0);
			_isGotFreeDiamond = value;
		}
	}

	public void Init()
	{
		int @int = PlayerPrefs.GetInt("IsShowVIPView");
		_vipShowView = (@int == 1);
		int int2 = PlayerPrefs.GetInt("IsGotFreeDiamond");
		_isGotFreeDiamond = (int2 == 1);
		int int3 = PlayerPrefs.GetInt("IsCanFree", 1);
		_isCanFree = (int3 == 1);
		_vipDate = PlayerPrefs.GetString("VipDate");
	}

	public void GetFreeDiamond()
	{
		if (!IsGotFreeDiamond)
		{
			IsGotFreeDiamond = true;
			ShopView view = Singleton<UIViewManager>.Instance.GetView<ShopView>("SHOP_VIEW");
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(110);
			Singleton<CoinManager>.Instance.FloatByCircle<FloatDiamond>("GameObject/Item/FloatDiamond", 20, Vector3.zero, view.tranDiamond.position, 0.25f, delegate
			{
				Singleton<DiamondManager>.Instance.VipFreeDiamond();
			}, layerTran);
			Singleton<AudioManager>.Instance.PlaySound("coin", 0.8f, 4, 0.2f);
		}
	}

	public string GetTimeUnixDate(string timeStamp, int addSpanDay = 7)
	{
		long num = long.Parse(timeStamp);
		return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(num).ToLocalTime().AddDays(addSpanDay)
			.ToString("yyyy-MM-dd");
	}
}
