using DG.Tweening;
using DragonBones;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class DialogueViewMediator : Mediator, IMediator
	{
		private DialogueVO[] _dialogueVOs;

		private GameObject[] _npcObjs;

		private int _index;

		private float _npsPosY;

		private const float AUTO_CLOSE_TIME = 3f;

		private Timer _autoCloseTimer;

		private DialogueView View => (DialogueView)ViewComponent;

		public DialogueViewMediator(string name, DialogueView viewComponent)
			: base(name, viewComponent)
		{
		}

		public override void OnRegister()
		{
			base.OnRegister();
			View.btnClick.onClick.AddListener(OnClick);
			View.btnClick.gameObject.SetActive(value: false);
			View.tranDialogue.gameObject.SetActive(value: false);
			View.objBg.SetActive(value: false);
			Vector2 anchoredPosition = View.imgDialogueBg.rectTransform.anchoredPosition;
			_npsPosY = anchoredPosition.y + View.imgDialogueBg.rectTransform.rect.height / 2f;
			View.continueTxt.text = LanguageConfig.clickAndContinues;
			View.continueTxt.gameObject.SetActive(value: true);
			View.txtName.text = LanguageConfig.npcName;
		}

		public override IList<string> ListNotificationInterests()
		{
			List<string> list = new List<string>();
			list.Add("SHOW_DIALOGUE");
			list.Add("SHOW_DIALOGUE_TIP");
			return list;
		}

		public override void HandleNotification(INotification notification)
		{
			string name = notification.Name;
			if (name == null)
			{
				return;
			}
			if (!(name == "SHOW_DIALOGUE"))
			{
				if (name == "SHOW_DIALOGUE_TIP" && notification.Body != null)
				{
					ShowTip(notification.Body as DialogueVO);
					View.objStar.gameObject.SetActive(value: false);
				}
				return;
			}
			_index = 0;
			_dialogueVOs = null;
			_dialogueVOs = (notification.Body as DialogueVO[]);
			if (_dialogueVOs == null || _dialogueVOs.Length == 0)
			{
				UnityEngine.Debug.LogError("DialogueVO is null");
				return;
			}
			CreateNPC(_dialogueVOs);
			if (_dialogueVOs[0].type == DialogueType.GameBegin)
			{
				Time.timeScale = 1f;
				Singleton<BulletManager>.Instance.Clean();
				Singleton<BallManger>.Instance.Pause();
				Singleton<PlayerManager>.Instance.PauseLeader();
				Singleton<GameItemManager>.Instance.PauseAllItems();
				View.tranDialogue.gameObject.SetActive(value: true);
				View.objBg.SetActive(value: true);
				Show();
			}
			else
			{
				View.tranDialogue.gameObject.SetActive(value: true);
				View.objBg.SetActive(value: true);
				Singleton<UIViewManager>.Instance.HideView("RESULT_VIEW");
				Show();
			}
		}

		private void ShowTip(DialogueVO vo)
		{
			_dialogueVOs = new DialogueVO[1]
			{
				vo
			};
			CreateNPC(_dialogueVOs);
			float duration = 3f;
			if (vo.firstClickStart)
			{
				duration = 100000f;
			}
			View.btnClick.gameObject.SetActive(value: false);
			View.tranDialogue.gameObject.SetActive(value: true);
			View.objBg.SetActive(value: true);
			Show(vo.content, 0.5f, vo.moveSpeed);
			_autoCloseTimer = Timer.Register(duration, OnClick);
		}

		private void CreateNPC(DialogueVO[] voArry)
		{
			if (_npcObjs != null)
			{
				for (int i = 0; i < _npcObjs.Length; i++)
				{
					UnityEngine.Object.Destroy(_npcObjs[i]);
				}
				_npcObjs = null;
			}
			_npcObjs = new GameObject[voArry.Length];
			for (int j = 0; j < _npcObjs.Length; j++)
			{
				_npcObjs[j] = (Object.Instantiate(Resources.Load("GameUI/DialogueView/img_npc_" + j), View.tranNpc) as GameObject);
				UnityArmatureComponent component = _npcObjs[j].GetComponent<UnityArmatureComponent>();
				_npcObjs[j].SetActive(value: false);
			}
		}

		private void Show()
		{
			View.objStar.gameObject.SetActive(value: true);
			Show(_dialogueVOs[_index].content, 0.5f);
		}

		private void Show(string content, float delay, float moveSpeed = 0.5f)
		{
			RectTransform rectTransform = _npcObjs[0].transform as RectTransform;
			rectTransform.anchoredPosition += new Vector2(-500f, 0f);
			rectTransform.DOAnchorPosX(0f, moveSpeed).SetEase(Ease.InQuart);
			_npcObjs[0].SetActive(value: true);
			View.canvasGroup.alpha = 0f;
			View.canvasGroup.DOFade(1f, 0.5f).SetDelay(0.4f);
			View.txtContent.text = content;
			Timer.Register(delay, delegate
			{
				View.btnClick.gameObject.SetActive(value: true);
			});
		}

		private void OnClick()
		{
			if (_dialogueVOs[0].type == DialogueType.GameTip)
			{
				if (_autoCloseTimer != null)
				{
					_autoCloseTimer.Cancel();
				}
				HideView();
				if (_dialogueVOs[0].callback != null)
				{
					_dialogueVOs[0].callback();
				}
				return;
			}
			_index++;
			if (_dialogueVOs == null || _index >= _dialogueVOs.Length)
			{
				HideView();
				if (_dialogueVOs[0].type == DialogueType.GameBegin)
				{
					Singleton<BallManger>.Instance.Continue();
					Singleton<PlayerManager>.Instance.ContinueLeader(protect: false, isShoot: false);
					Singleton<GameItemManager>.Instance.ContinueAllItems();
					_dialogueVOs[0].callback();
				}
				else if (_dialogueVOs[0].type == DialogueType.GameResult)
				{
					Singleton<UIViewManager>.Instance.ShowView("RESULT_VIEW");
				}
			}
			else
			{
				Show();
			}
		}

		private void HideView()
		{
			View.btnClick.gameObject.SetActive(value: false);
			View.tranDialogue.gameObject.SetActive(value: false);
			View.objBg.SetActive(value: false);
			Singleton<UIViewManager>.Instance.HideView("DIALOGUE_VIEW");
		}
	}
}
