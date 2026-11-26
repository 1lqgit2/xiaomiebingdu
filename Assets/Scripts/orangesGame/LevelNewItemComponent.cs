using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class LevelNewItemComponent : MonoBehaviour
	{
		public Button btnClick;

		public Text txtTitle;

		public Text txtProgress;

		public Text txtContent;

		public Image imgIcon;

		public Image[] imgWeapon;

		public int index;

		public GameObject objClear;

		private int _state = -1;

		private void Start()
		{
		}

		public void SetBtnClick()
		{
			btnClick.onClick.RemoveAllListeners();
			btnClick.onClick.AddListener(OnClick);
		}

		private void OnClick()
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Singleton<UIViewManager>.Instance.HideView("LEVEL_VIEW");
				return;
			}
			ApplicationFacade.Instance.SendNotification("LEVEL_VIEW_ITEM_HIDE");
			Singleton<UIViewManager>.Instance.ShowView("DIALOGUE_VIEW");
			DialogueVO dialogueVO = new DialogueVO();
			dialogueVO.npcID = 0;
			dialogueVO.type = DialogueType.GameTip;
			dialogueVO.content = string.Format(LanguageConfig.level_click_tip, LanguageConfig.constellation_names[index]);
			dialogueVO.callback = delegate
			{
				ApplicationFacade.Instance.SendNotification("LEVEL_VIEW_ITEM_SHOW");
			};
			ApplicationFacade.Instance.SendNotification("SHOW_DIALOGUE_TIP", dialogueVO);
		}

		public void SetImageGray(Material material)
		{
			if (_state != 3 && material != null)
			{
				_state = 3;
				btnClick.enabled = false;
				imgIcon.material = material;
				btnClick.image.material = material;
				for (int i = 0; i < imgWeapon.Length; i++)
				{
					imgWeapon[i].material = material;
				}
				objClear.SetActive(value: false);
			}
		}

		public void SetImageDark()
		{
			if (_state != 1)
			{
				Color color = new Color(40f / 51f, 40f / 51f, 40f / 51f);
				_state = 1;
				btnClick.enabled = true;
				btnClick.image.material = null;
				imgIcon.material = null;
				for (int i = 0; i < imgWeapon.Length; i++)
				{
					imgWeapon[i].material = null;
					imgWeapon[i].color = color;
				}
				btnClick.image.color = color;
				imgIcon.color = color;
				objClear.SetActive(value: true);
			}
		}

		public void SetImageActive()
		{
			if (_state != 2)
			{
				_state = 2;
				btnClick.enabled = true;
				btnClick.image.material = null;
				imgIcon.material = null;
				for (int i = 0; i < imgWeapon.Length; i++)
				{
					imgWeapon[i].material = null;
					imgWeapon[i].color = Color.white;
				}
				btnClick.image.color = Color.white;
				imgIcon.color = Color.white;
				objClear.SetActive(value: false);
			}
		}
	}
}
