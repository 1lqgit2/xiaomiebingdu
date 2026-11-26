using System;
using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class DialogView : UIView
	{
		public Button dialogBlackGround;

		public Transform tranReward;

		public Button btnLeft;

		public Button btnRight;

		public Button btnClose;

		public Text txtTitle;

		public Text txtLeftCollect;

		public Text txtRightCollect;

		public Transform tranDialog;

		public Text txtDialogContent;

		public Button btnDialogLeft;

		public Button btnDialogRight;

		public Text txtDialogLeft;

		public Text txtDialogRight;

		public Transform tranDownload;

		public Button btnDonwload;

		public Text txtDownLoad;

		public override void OnLoad()
		{
			base.OnLoad();
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(90);
			CreateView("GameUI/DialogView/ui_dialogview", layerTran);
		}

		public override void OnInit()
		{
			base.OnInit();
			Transform transform = base.Root.transform;
			dialogBlackGround = transform.Find("ImgBg_0").GetComponent<Button>();
			tranReward = transform.Find("TranGetReward");
			txtTitle = tranReward.Find("TxtTitle").GetComponent<Text>();
			btnLeft = tranReward.Find("BtnLeft").GetComponent<Button>();
			txtLeftCollect = btnLeft.transform.Find("TxtCost").GetComponent<Text>();
			btnRight = tranReward.Find("BtnRight").GetComponent<Button>();
			txtRightCollect = btnRight.transform.Find("TxtCost").GetComponent<Text>();
			btnClose = tranReward.Find("BtnClose").GetComponent<Button>();
			tranDialog = transform.Find("root_dialog");
			txtDialogContent = tranDialog.Find("txt_content").GetComponent<Text>();
			btnDialogLeft = tranDialog.Find("btn_left").GetComponent<Button>();
			btnDialogRight = tranDialog.Find("btn_right").GetComponent<Button>();
			txtDialogLeft = tranDialog.Find("btn_left/txt_content").GetComponent<Text>();
			txtDialogRight = tranDialog.Find("btn_right/txt_content").GetComponent<Text>();
			tranDownload = transform.Find("root_download");
			btnDonwload = tranDownload.Find("btn_download").GetComponent<Button>();
			txtDownLoad = tranDownload.Find("txt_content").GetComponent<Text>();
			ApplicationFacade.Instance.RegisterMediator(new DialogViewMediator("DIALOG", this));
		}

		public override void OnHide(Action callback = null)
		{
			base.OnHide(callback);
			tranDialog.gameObject.SetActive(value: false);
			tranReward.gameObject.SetActive(value: false);
			tranDownload.gameObject.SetActive(value: false);
		}

		public override void OnRegister()
		{
			base.OnRegister();
		}

		public override void OnRemove()
		{
			base.OnRemove();
			ApplicationFacade.Instance.RemoveMediator("DIALOG");
		}
	}
}
