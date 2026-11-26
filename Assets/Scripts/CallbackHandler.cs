using orangesGame;
using UnityEngine;

public class CallbackHandler : MonoBehaviour
{
	public void ShareSuc(string context)
	{
		ApplicationFacade.Instance.SendNotification("SHARE_SUCCESS");
	}

	public void ShareFail(string context)
	{
		ApplicationFacade.Instance.SendNotification("SHARE_FAIL");
	}

	public void GooglePaySuccess(string msg)
	{
		string[] array = msg.Split('|');
		GameAnalysisData.BuyFromShopEnd(array[0], array[1]);
		if (string.IsNullOrEmpty(array[1]))
		{
			GameAnalysisData.GooglePaySuccessNullOrderId(array[0]);
		}
		else if (GameGlobalData.GoogleSkuIdDic.ContainsKey(array[0]))
		{
			switch (array[0])
			{
			case "diamond_1":
				ApplicationFacade.Instance.SendNotification("SHOP_DIAMOND_BUY", 0);
				break;
			case "diamond_2":
				ApplicationFacade.Instance.SendNotification("SHOP_DIAMOND_BUY", 1);
				break;
			case "diamond_3":
				ApplicationFacade.Instance.SendNotification("SHOP_DIAMOND_BUY", 2);
				break;
			case "diamond_4":
				ApplicationFacade.Instance.SendNotification("SHOP_DIAMOND_BUY", 3);
				break;
			case "diamond_5":
				ApplicationFacade.Instance.SendNotification("SHOP_DIAMOND_BUY", 4);
				break;
			case "diamond_6":
				ApplicationFacade.Instance.SendNotification("SHOP_DIAMOND_BUY", 5);
				break;
			case "no_ads":
				ApplicationFacade.Instance.SendNotification("SHOP_NOADS_BUY");
				break;
			case "time_1":
			case "time_2":
			case "time_3":
			case "time_4":
			case "time_5":
				ApplicationFacade.Instance.SendNotification("SHOP_TIMEGIFT_BUY", array[0]);
				break;
			case "item_1":
			case "item_2":
			case "item_3":
				ApplicationFacade.Instance.SendNotification("SHOP_ITEMGIFT_BUY", array[0]);
				break;
			case "hold_up_vessel":
				ApplicationFacade.Instance.SendNotification("LUCKYBOX_BUY_SUCCESS");
				break;
			}
		}
		else
		{
			UnityEngine.Debug.LogError("GooglePaySuccess msg not contain :" + msg);
		}
	}

	public void GoogleVipSuccess(string msg)
	{
		string[] array = msg.Split('|');
		if (array[0] == "diamond_membership")
		{
			Singleton<VipManager>.Instance.VipDate = array[1];
			Singleton<VipManager>.Instance.VipStatus = true;
			Singleton<VipManager>.Instance.VipCanFree = false;
			Singleton<PlayerManager>.Instance.LeaderSetVip();
			ApplicationFacade.Instance.SendNotification("VIP_STATE_UPDATE_SUCCESS", array[1]);
		}
		if (array.Length == 3)
		{
			GameAnalysisData.BuyFromSubscribeEnd(array[2]);
		}
	}

	public void GooglePayFail(string msg)
	{
		switch (msg)
		{
		case "diamond_1":
		case "diamond_2":
		case "diamond_3":
		case "diamond_4":
		case "diamond_5":
		case "diamond_6":
		case "no_ads":
		case "time_1":
		case "time_2":
		case "time_3":
		case "time_4":
		case "time_5":
			ApplicationFacade.Instance.SendNotification("SHOP_BUY_FAIL", msg);
			break;
		case "hold_up_vessel":
			ApplicationFacade.Instance.SendNotification("LUCKYBOX_BUY_FAIL");
			break;
		}
	}

	public void Init(string context)
	{
	}
}
