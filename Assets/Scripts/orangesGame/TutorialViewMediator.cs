using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class TutorialViewMediator : Mediator, IMediator
	{
		private float _waitTime = 4f;

		private float _waitTimeBtn = 6.5f;

		private Timer _timer;

		private TutorialView View => (TutorialView)ViewComponent;

		public TutorialViewMediator(string name, TutorialView viewComponent)
			: base(name, viewComponent)
		{
		}

		public override void OnRegister()
		{
			base.OnRegister();
			View.btnComfirn.onClick.AddListener(OnClickComfirn);
		}

		public override void OnRemove()
		{
			base.OnRemove();
		}

		public override IList<string> ListNotificationInterests()
		{
			List<string> list = new List<string>();
			list.Add("TUTORIAL_GAMING_TIP");
			list.Add("TUTORIAL_OPEN_UPGAREDVIEW");
			list.Add("TUTORIAL_UPGARED_POWER");
			return list;
		}

		public override void HandleNotification(INotification notification)
		{
			string name = notification.Name;
			if (name == null)
			{
				return;
			}
			if (!(name == "TUTORIAL_GAMING_TIP"))
			{
				if (!(name == "TUTORIAL_OPEN_UPGAREDVIEW"))
				{
					if (name == "TUTORIAL_UPGARED_POWER")
					{
						View.rectTranFinger.gameObject.SetActive(value: true);
						View.rectTranFingerRow.gameObject.SetActive(value: false);
						Button button = notification.Body as Button;
						Vector3 position = button.transform.position;
						UpGardePower(button);
						SetTip(LanguageConfig.TUTORIAL_UPGRADE_POWER, position + new Vector3(-2.35f, 5f, 0f));
						View.rectTranFinger.position = position + new Vector3(0f, 2f, 0f);
					}
				}
				else
				{
					View.rectTranBg.gameObject.SetActive(value: true);
					View.rectTranTip.gameObject.SetActive(value: true);
					View.rectTranFingerRow.gameObject.SetActive(value: true);
					object[] array = notification.Body as object[];
					Button btn = array[0] as Button;
					Vector3 vector = (Vector3)array[1];
					OpenUpGardeView(btn, vector);
					SetTip(LanguageConfig.TUTORIAL_UPGARDEVIEW, vector + new Vector3(-5.3f, 1.5f, 0f));
					View.rectTranFingerRow.position = vector + new Vector3(-4f, 0f, 0f);
				}
			}
			else
			{
				GameGlobalData.SetTutorialStat(TutorialType.OpenUpGradeView);
			}
		}

		private void OpenGamingTip()
		{
		}

		private void OpenUpGardeView(Button btn, Vector3 pos)
		{
			Button copy = Object.Instantiate(btn);
			copy.transform.SetParent(View.Root.transform);
			copy.transform.position = pos;
			copy.onClick.AddListener(btn.onClick.Invoke);
			copy.onClick.AddListener(delegate
			{
				UnityEngine.Object.DestroyImmediate(copy.gameObject);
				btn.gameObject.SetActive(value: true);
				GameGlobalData.TutorialState = 2;
				SendNotification("TUTORIAL_SET_STEP");
			});
			copy.transform.Find("ImgTip").gameObject.SetActive(value: true);
			btn.gameObject.SetActive(value: false);
            copy.transform.localScale = Vector3.one;
		}

		private void UpGardePower(Button btn)
		{
			Button copy = Object.Instantiate(btn);
			copy.transform.SetParent(View.Root.transform);
			copy.transform.position = btn.transform.position;
			copy.onClick.AddListener(btn.onClick.Invoke);
			copy.onClick.AddListener(delegate
			{
				UnityEngine.Object.DestroyImmediate(copy.gameObject);
				btn.gameObject.SetActive(value: true);
				GameGlobalData.SetTutorialStat(TutorialType.Finish);
				Singleton<UIViewManager>.Instance.HideView("TUTORIAL_VIEW");
			});
			btn.gameObject.SetActive(value: false);
            copy.transform.localScale = Vector3.one;
        }

		private void SetTip(string tip, Vector2 pos)
		{
			View.txtTip.text = tip;
			View.txtTip.transform.parent.position = pos;
		}

		private void OnClickComfirn()
		{
			Singleton<BallManger>.Instance.Continue();
			Singleton<PlayerManager>.Instance.ContinueLeader(protect: false, isShoot: false);
			Singleton<GameItemManager>.Instance.ContinueAllItems();
			View.tranGamingTip.gameObject.SetActive(value: false);
			View.rectTranBg.gameObject.SetActive(value: false);
		}
	}
}
