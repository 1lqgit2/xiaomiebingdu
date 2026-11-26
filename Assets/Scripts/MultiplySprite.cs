using System.Collections.Generic;
using UnityEngine;

public class MultiplySprite : MonoBehaviour
{
	public Dictionary<string, Sprite> SpriteDic;

	public string[] Keys;

	public Sprite[] Sprites;

	private void Awake()
	{
	}

	public void Init()
	{
		SpriteDic = new Dictionary<string, Sprite>();
		for (int i = 0; i < Keys.Length; i++)
		{
			SpriteDic.Add(Keys[i], Sprites[i]);
		}
	}
}
