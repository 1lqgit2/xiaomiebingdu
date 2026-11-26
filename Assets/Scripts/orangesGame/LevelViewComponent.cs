using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class LevelViewComponent : MonoBehaviour
	{
		public GameObject subObj;

		public GameObject lineObj;

		public RectTransform tranConetent;

		public ScrollRect scrollRect;

		public LevelBackGroundComponent bgComponent;

		private float _height;

		private float _height_half;

		private int _lineIndex;

		private int _dataStart;

		private float _startOffSet = 200f;

		private float _space = 300f;

		private List<RectTransform> _lineList;

		private LinkedList<LevelItemComponent> _linkedList;

		private int[] _datas;

		private float _dir;

		private Dictionary<int, WeaponStruct> _weaponJson;

		private void Start()
		{
			Singleton<GameJson>.Instance.Init();
			Init(0, new int[20]);
			scrollRect.onValueChanged.AddListener(OnValueChanged);
		}

		public void Init(int curLevel, int[] data)
		{
			for (int i = 0; i < data.Length; i++)
			{
				data[i] = i;
			}
			_weaponJson = new Dictionary<int, WeaponStruct>();
			WeaponStruct[] weaponJson = Singleton<GameJson>.Instance.WeaponJson;
			for (int j = 0; j < weaponJson.Length; j++)
			{
				_weaponJson.Add(weaponJson[j].UnLockLevel, weaponJson[j]);
			}
			_datas = data;
			tranConetent.sizeDelta = new Vector2(0f, 1800 * (_datas.Length / 6));
			_height = tranConetent.rect.height;
			_height_half = _height / 2f;
			_lineList = new List<RectTransform>();
			_linkedList = new LinkedList<LevelItemComponent>();
			_dataStart = curLevel / 10 * 10;
			CreateSubLevel(_dataStart);
			for (int k = 1; k < 9; k++)
			{
				CreateSubLevel(_dataStart + k);
				CreateSubLine();
			}
			RollTarget(0);
		}

		private void LateUpdate()
		{
			foreach (LevelItemComponent linked in _linkedList)
			{
				if (linked != null)
				{
					Vector3 position = linked.tran.position;
					if (position.y < -15f)
					{
						UpdateNextNode();
						break;
					}
				}
				if (linked != null)
				{
					Vector3 position2 = linked.tran.position;
					if (position2.y > 15f)
					{
						UpdatePreNode();
						break;
					}
				}
			}
		}

		private void OnValueChanged(Vector2 vector)
		{
			if (vector.y < _dir)
			{
				bgComponent.OnUpdate((vector.y - _dir) * 4000f);
			}
			else
			{
				bgComponent.OnUpdate((vector.y - _dir) * 4000f);
			}
			_dir = vector.y;
		}

		private void RollTarget(int targetLevel)
		{
			LinkedListNode<LevelItemComponent> first = _linkedList.First;
			LevelItemComponent value = first.Value;
			RectTransform rectTransform = tranConetent;
			float num = 0f - _height_half;
			Vector2 anchoredPosition = value.tran.anchoredPosition;
			rectTransform.anchoredPosition = new Vector2(0f, num + anchoredPosition.y + _startOffSet);
		}

		private void CreateSubLine()
		{
			RectTransform rectTransform = UnityEngine.Object.Instantiate(lineObj, tranConetent).transform as RectTransform;
			LinkedListNode<LevelItemComponent> last = _linkedList.Last;
			LinkedListNode<LevelItemComponent> previous = last.Previous;
			float y = Vector2.Distance(last.Value.tran.anchoredPosition, previous.Value.tran.anchoredPosition);
			RectTransform rectTransform2 = rectTransform;
			Vector2 sizeDelta = rectTransform.sizeDelta;
			rectTransform2.sizeDelta = new Vector2(sizeDelta.x, y);
			rectTransform.anchoredPosition = previous.Value.tran.anchoredPosition;
			Vector2 v = last.Value.tran.localPosition - previous.Value.tran.localPosition;
			rectTransform.localRotation = Quaternion.FromToRotation(Vector2.down, v);
			_lineList.Add(rectTransform);
		}

		private void CreateSubLevel(int index)
		{
			LevelItemComponent component = UnityEngine.Object.Instantiate(subObj, tranConetent).GetComponent<LevelItemComponent>();
			component.tran.anchoredPosition = new Vector2(UnityEngine.Random.Range(-450, 450), _height_half - _startOffSet - _space * (float)index);
			component.name = index.ToString();
			component.SetDate(_datas[index], index);
			component.SetUnLock(_weaponJson.ContainsKey(_datas[index]));
			component.btnClick.onClick.AddListener(OnClickCurLevel);
			_linkedList.AddLast(component);
		}

		private void UpdateNextNode()
		{
			LinkedListNode<LevelItemComponent> first = _linkedList.First;
			LinkedListNode<LevelItemComponent> last = _linkedList.Last;
			int num = last.Value.index + 1;
			if (num < _datas.Length)
			{
				_linkedList.Remove(first);
				_linkedList.AddLast(first);
				first.Value.index = num;
				first.Value.data = _datas[num];
				first.Value.SetUnLock(_weaponJson.ContainsKey(_datas[num]));
				first.Value.tran.anchoredPosition = new Vector2(UnityEngine.Random.Range(-450, 450), _height_half - 200f - (float)(300 * first.Value.index));
				SetNextLinePos(_lineIndex, first);
				_lineIndex = (_lineIndex + 1) % _lineList.Count;
			}
		}

		private void UpdatePreNode()
		{
			LinkedListNode<LevelItemComponent> first = _linkedList.First;
			LinkedListNode<LevelItemComponent> last = _linkedList.Last;
			int num = first.Value.index - 1;
			if (num >= 0)
			{
				_linkedList.Remove(last);
				_linkedList.AddFirst(last);
				last.Value.index = num;
				last.Value.data = _datas[num];
				last.Value.SetUnLock(_weaponJson.ContainsKey(_datas[num]));
				last.Value.tran.anchoredPosition = new Vector2(UnityEngine.Random.Range(-450, 450), _height_half - 200f - (float)(300 * last.Value.index));
				_lineIndex = (_lineIndex - 1) % _lineList.Count;
				if (_lineIndex < 0)
				{
					_lineIndex = _lineList.Count - 1;
				}
				SetPreLinePos(_lineIndex, last);
			}
		}

		private void SetNextLinePos(int index, LinkedListNode<LevelItemComponent> node)
		{
			RectTransform rectTransform = _lineList[index];
			LinkedListNode<LevelItemComponent> previous = node.Previous;
			float y = Vector2.Distance(node.Value.tran.anchoredPosition, previous.Value.tran.anchoredPosition);
			RectTransform rectTransform2 = rectTransform;
			Vector2 sizeDelta = rectTransform.sizeDelta;
			rectTransform2.sizeDelta = new Vector2(sizeDelta.x, y);
			rectTransform.anchoredPosition = previous.Value.tran.anchoredPosition;
			Vector2 v = node.Value.tran.localPosition - previous.Value.tran.localPosition;
			rectTransform.localRotation = Quaternion.FromToRotation(Vector2.down, v);
		}

		private void SetPreLinePos(int index, LinkedListNode<LevelItemComponent> node)
		{
			RectTransform rectTransform = _lineList[index];
			LinkedListNode<LevelItemComponent> next = node.Next;
			float y = Vector2.Distance(next.Value.tran.anchoredPosition, node.Value.tran.anchoredPosition);
			RectTransform rectTransform2 = rectTransform;
			Vector2 sizeDelta = rectTransform.sizeDelta;
			rectTransform2.sizeDelta = new Vector2(sizeDelta.x, y);
			rectTransform.anchoredPosition = node.Value.tran.anchoredPosition;
			Vector2 v = next.Value.tran.localPosition - node.Value.tran.localPosition;
			rectTransform.localRotation = Quaternion.FromToRotation(Vector2.down, v);
		}

		private void OnClickCurLevel()
		{
			UnityEngine.Debug.Log("@@@@@@@@@@");
			Singleton<UIViewManager>.Instance.ShowView("DIALOG_VIEW");
			DialogData dialogData = new DialogData();
			dialogData.content = LanguageConfig.dialog_content;
			dialogData.callback = delegate
			{
				Singleton<UIViewManager>.Instance.HideView("LEVEL_VIEW");
				ApplicationFacade.Instance.SendNotification("MAIN_START_GAME");
			};
			ApplicationFacade.Instance.SendNotification("DIALOG_COMMON", dialogData);
		}
	}
}
