using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace orangesGame
{
	public class UIItemIconRenderer : MonoBehaviour
	{
		public LineRenderer CountDown;

		public Transform CountDownPosTran;

		public Image Icon;

		private int _posCount;

		private ItemBase _item;

		private void UpdateCountDown(float time, float duration)
		{
			int num = _posCount - 1;
			float num2 = 1f - Mathf.Clamp01(time / duration);
			float num3 = 1f / (float)num;
			num2 *= (float)num;
			int num4 = Mathf.FloorToInt(num2);
			num2 = ((!(num2 >= (float)num)) ? (num2 - (float)num4) : 1f);
			if (num4 >= num)
			{
				num4--;
			}
			CountDown.positionCount = num4 + 2;
			Vector3 position = Vector3.Lerp(CountDownPosTran.GetChild(num4).transform.position, CountDownPosTran.GetChild(num4 + 1).transform.position, num2);
			position.z = -1f;
			CountDown.SetPosition(num4 + 1, position);
			for (int i = 0; i <= num4; i++)
			{
				Vector3 position2 = CountDownPosTran.GetChild(i).transform.position;
				position2.z = ((i % 2 != 0) ? (-0.5f) : 0f);
				CountDown.SetPosition(i, position2);
			}
		}

		private void ItemTimeOut()
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}

		public void SetItem(ItemBase item)
		{
			_item = item;
			_posCount = CountDownPosTran.childCount;
			item.EventItemCountDownUpdate.AddListener(UpdateCountDown);
			item.EventItemTimeOut.AddListener(ItemTimeOut);
			Icon.sprite = item.ItemIconSprite;
			LineRenderer countDown = CountDown;
			Color cDLineColor = item.CDLineColor;
			CountDown.endColor = cDLineColor;
			countDown.startColor = cDLineColor;
		}

		public void SetBattleItem(BattleItemBase item)
		{
			_posCount = CountDownPosTran.childCount;
			item.ActionItemCountDownUpdate = UpdateCountDown;
			item.ActionItemTimeOut = ItemTimeOut;
			SpriteAtlas spriteAtlas = Resources.Load<SpriteAtlas>("GameAtlas/BattleItemUIIconAtlas");
			Icon.sprite = spriteAtlas.GetSprite(item.ItemIconName);
			LineRenderer countDown = CountDown;
			Color color = new Color(253f / 255f, 1f, 11f / 51f, 1f);
			CountDown.endColor = color;
			countDown.startColor = color;
		}
	}
}
