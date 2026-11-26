using System;
using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class UIWeaponItem : MonoBehaviour
	{
		public int ID;

		public Button btnClick;

		public Text TxtContent;

		public Image[] imgWeapons;

		private Action<int> _callback;

		public void SetOnClick(Action<int> callback)
		{
			_callback = callback;
			btnClick.onClick.AddListener(OnCallback);
		}

		public void SetParent(Transform parent)
		{
			base.transform.SetParent(parent);
			base.transform.localScale = Vector3.one;
			base.transform.localPosition = Vector3.zero;
		}

		private void OnCallback()
		{
			_callback(ID);
		}

		public void SetLock(Sprite sprite, int lockLevel)
		{
			Material grayMaterial = GameUtils.GetGrayMaterial();
			for (int i = 0; i < imgWeapons.Length; i++)
			{
				imgWeapons[i].material = grayMaterial;
			}
			btnClick.image.sprite = sprite;
			btnClick.enabled = false;
			TxtContent.text = string.Format(LanguageConfig.UNLOCK, lockLevel);
		}

		public void SetUnLock(Sprite sprite)
		{
			for (int i = 0; i < imgWeapons.Length; i++)
			{
				imgWeapons[i].material = null;
			}
			btnClick.image.sprite = sprite;
			btnClick.enabled = true;
			TxtContent.text = string.Empty;
		}

		public void SetIcon(Sprite sprite)
		{
			for (int i = 0; i < imgWeapons.Length; i++)
			{
				imgWeapons[i].sprite = sprite;
				imgWeapons[i].SetNativeSize();
			}
		}
	}
}
