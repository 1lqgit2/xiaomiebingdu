using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace orangesGame
{
	public class UITaskItem : MonoBehaviour
	{
		public Image imgBg1;

		public Image imgBg2;

		public Button btnReward;

		public Text txtCollect;

		public GameObject objStar;

		public GameObject objGet;

		public Text txtNum;

		public Text txtTitle;

		public Text txtProgress;

		public Text txtReward;

		public GameObject objBtnImage;

		private RectTransform _tran;

		private int _index;

		private bool _isSpecial;

		private void Awake()
		{
			_tran = (base.transform as RectTransform);
		}

		public void Init(Transform parent, Vector2 pos)
		{
			_tran.SetParent(parent);
			_tran.anchoredPosition = pos;
			_tran.localScale = Vector3.one;
			btnReward.onClick.AddListener(OnClickReward);
			_isSpecial = false;
			txtCollect.text = LanguageConfig.COLLECT;
		}

		public void SetTask(DailyTaskVO vo, string cost)
		{
			txtNum.text = (vo.index + 1).ToString();
			txtTitle.text = LanguageConfig.TASK_TITLE[(int)(vo.type - 1)];
			txtProgress.text = $"{vo.progress}/{vo.progressMax}";
			txtReward.text = GameUtils.FloatToUnit(vo.rate * GameUtils.UnitToFloat(cost));
			if (vo.rewardState == 0)
			{
				SpriteAtlas spriteAtlas = Resources.Load<SpriteAtlas>("GameAtlas/MainAtlas");
				if (vo.isFinish)
				{
					btnReward.image.sprite = spriteAtlas.GetSprite("anniu_b_1");
				}
				else
				{
					btnReward.image.sprite = spriteAtlas.GetSprite("anniu_b_3");
				}
				btnReward.enabled = vo.isFinish;
				SetBtnRewardActive(isActive: true);
			}
			else
			{
				SetBtnRewardActive(isActive: false);
			}
			_index = vo.index;
		}

		public void SetSpecialTask(DailyTaskVO vo, string cost)
		{
			SetTask(vo, cost);
			txtNum.text = string.Empty;
			objStar.gameObject.SetActive(value: true);
			imgBg1.color = new Color(14f / 15f, 89f / 255f, 233f / 255f);
			imgBg2.color = new Color(63f / 85f, 8f / 85f, 188f / 255f);
			objBtnImage.SetActive(vo.isFinish);
			_isSpecial = true;
		}

		public void SetBtnRewardActive(bool isActive)
		{
			btnReward.gameObject.SetActive(isActive);
		}

		private void OnClickReward()
		{
			if (_isSpecial)
			{
				ApplicationFacade.Instance.SendNotification("TASK_GETREWARD_FINAL", _index);
			}
			else
			{
				ApplicationFacade.Instance.SendNotification("TASK_GETREWARD", _index);
			}
		}
	}
}
