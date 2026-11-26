using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;

namespace orangesGame
{
	public class GradeViewMediator : Mediator, IMediator
	{
		private int _index = 5;

		private bool _isStarView = true;

		private GradeView View => (GradeView)ViewComponent;

		public GradeViewMediator(string name, GradeView viewComponent)
			: base(name, viewComponent)
		{
		}

		public override void OnRegister()
		{
			base.OnRegister();
			View.btnConfirm.onClick.AddListener(OnClickConfirm);
			View.btnExit.onClick.AddListener(OnClickExit);
			for (int i = 0; i < View.btnStars.Length; i++)
			{
				int temp = i;
				View.btnStars[i].onClick.AddListener(delegate
				{
					OnClickStar(temp);
				});
			}
		}

		private void OnClickStar(int index)
		{
			int num = View.btnStars.Length;
			int num2 = index + 1;
			for (int i = 0; i < num; i++)
			{
				if (i < num2)
				{
					View.imgStars[i].gameObject.SetActive(value: true);
				}
				else
				{
					View.imgStars[i].gameObject.SetActive(value: false);
				}
			}
			_index = num2;
		}

		private void OnExit()
		{
			GameGlobalData.SetGradeState(1);
			Singleton<UIViewManager>.Instance.HideView("GRADE_VIEW");
		}

		private void OnClickExit()
		{
			OnExit();
			GameGlobalData.ActualLevelReject = GameGlobalData.ActualLevel;
			GameGlobalData.SetActualLevelReject();
			GameAnalysisData.UserEvaluation(0, 0, string.Empty);
		}

		private void OnClickConfirm()
		{
			if (_isStarView)
			{
				if (_index >= View.btnStars.Length - 1)
				{
					OnExit();
					Application.OpenURL(GameGlobalData.GARDE_URL);
					GameGlobalData.IsEvaluate = 1;
					GameGlobalData.SetIsEvaluate();
					GameAnalysisData.UserEvaluation(1, _index, string.Empty);
				}
				else
				{
					_isStarView = false;
					View.tranStar.gameObject.SetActive(value: false);
					View.tranContent.gameObject.SetActive(value: true);
				}
			}
			else
			{
				OnExit();
				GameGlobalData.IsEvaluate = 1;
				GameGlobalData.SetIsEvaluate();
				GameAnalysisData.UserEvaluation(1, _index, View.txtContent.text);
			}
		}
	}
}
