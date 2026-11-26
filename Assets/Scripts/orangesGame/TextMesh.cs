using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace orangesGame
{
	public class TextMesh : MonoBehaviour
	{
		public SpriteAtlas atlas;

		public SpriteRenderer target;

		public Transform parent;

		public float padding = 0.3f;

		private int fontSize;

		private List<TextMeshData> _list;

		private Transform _tran;

		private int _order;

		private string strValue = string.Empty;

		private float[] sizeFloats = new float[5]
		{
			0.5f,
			0.4f,
			0.3f,
			0.23f,
			0.2f
		};

		private void Awake()
		{
			_list = new List<TextMeshData>();
			_tran = base.transform;
		}

		public void SetSortingOrder(int order)
		{
			_order = order;
			target.sortingOrder += _order;
		}

		public void Parse(string str)
		{
			strValue = str;
			char[] array = str.ToCharArray();
			fontSize = array.Length;
			if (array.Length <= _list.Count)
			{
				for (int i = 0; i < array.Length; i++)
				{
					ChangeTextMesh(array[i], i);
				}
				for (int j = array.Length; j < _list.Count; j++)
				{
					_list[j].renderer.gameObject.SetActive(value: false);
				}
			}
			else
			{
				for (int k = 0; k < _list.Count; k++)
				{
					ChangeTextMesh(array[k], k);
				}
				for (int l = _list.Count; l < array.Length; l++)
				{
					TextMeshData textMeshData = new TextMeshData();
					textMeshData.renderer = GetAtlasSprite(array[l].ToString());
					SetTextMeshData(array[l], textMeshData, GetAtlasSprite(array[l].ToString(), textMeshData.renderer));
					_list.Add(textMeshData);
				}
			}
			int num = (!str.Contains(".")) ? (fontSize - 1) : (fontSize - 2);
			num = ((num >= sizeFloats.Length) ? (sizeFloats.Length - 1) : num);
			float d = sizeFloats[num];
			base.transform.localScale = Vector3.one * d;
			SortList();
		}

		private void ChangeTextMesh(char value, int index)
		{
			if (value.ToString() == _list[index].name)
			{
				if (!_list[index].renderer.gameObject.activeInHierarchy)
				{
					_list[index].renderer.gameObject.gameObject.SetActive(value: true);
				}
			}
			else
			{
				SetTextMeshData(value, _list[index], GetAtlasSprite(value.ToString(), _list[index].renderer));
			}
		}

		private void SetTextMeshData(char value, TextMeshData meshData, SpriteRenderer renderer)
		{
			meshData.name = value.ToString();
			meshData.renderer = renderer;
			float num = meshData.renderer.sprite.rect.width / 100f;
			Vector3 localScale = meshData.renderer.transform.localScale;
			meshData.wide = num * localScale.x - padding;
		}

		private SpriteRenderer GetAtlasSprite(string str)
		{
			SpriteRenderer spriteRenderer = UnityEngine.Object.Instantiate(target);
			spriteRenderer.transform.SetParent(parent);
			spriteRenderer.transform.localPosition = Vector3.zero;
			spriteRenderer.transform.localScale = target.transform.localScale;
			return GetAtlasSprite(str, spriteRenderer);
		}

		private SpriteRenderer GetAtlasSprite(string str, SpriteRenderer renderer)
		{
			Sprite sprite2 = renderer.sprite = atlas.GetSprite("bingdu_" + str);
			renderer.gameObject.SetActive(value: true);
			return renderer;
		}

		private void SortList()
		{
			float num = 0f;
			float num2 = 0f;
			for (int i = 1; i < _list.Count; i++)
			{
				if (_list[i].renderer.gameObject.activeSelf)
				{
					num2 = _list[i - 1].wideHalf + _list[i].wideHalf;
					num += num2;
					_list[i].renderer.transform.localPosition = new Vector2(num, 0f);
				}
			}
			parent.localPosition = new Vector2((0f - num) / 2f, 0f);
		}
	}
}
