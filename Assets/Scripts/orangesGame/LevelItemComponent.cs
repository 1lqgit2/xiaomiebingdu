using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class LevelItemComponent : MonoBehaviour
	{
		public enum State
		{
			NoPass,
			CurLevel,
			Pass
		}

		public Button btnClick;

		public Text txtContent;

		private LevelUnLockComponent _unLockComponent;

		public int data;

		public int index;

		public RectTransform tran;

		private void Awake()
		{
			tran = (base.transform as RectTransform);
		}

		public void SetDate(int data, int index)
		{
			this.data = data;
			this.index = index;
			txtContent.text = data.ToString();
		}

		public void SetState(State state)
		{
			switch (state)
			{
			}
		}

		public void SetUnLock(bool isActive)
		{
			if (isActive)
			{
				if (_unLockComponent == null)
				{
					_unLockComponent = UnityEngine.Object.Instantiate(Resources.Load<LevelUnLockComponent>("GameUI/LevelView/ui_levelview_unlock"), tran);
				}
				_unLockComponent.gameObject.SetActive(value: true);
			}
			else if (_unLockComponent != null)
			{
				_unLockComponent.gameObject.SetActive(value: false);
			}
		}
	}
}
