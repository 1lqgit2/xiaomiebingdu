using UnityEngine;
using UnityEngine.UI;

public class RawImageWidget : MonoBehaviour
{
	public RawImage rimgBg;

	public float speed;

	private bool _isPause = true;

	private void Update()
	{
		if (!_isPause)
		{
			Move();
		}
	}

	public void SetSprite(Texture texture)
	{
		rimgBg.texture = texture;
	}

	public void Play()
	{
		_isPause = false;
	}

	public void Pause()
	{
		_isPause = true;
	}

	public void Stop()
	{
		Pause();
		Rect uvRect = rimgBg.uvRect;
		uvRect.y = 0f;
		rimgBg.uvRect = uvRect;
	}

	public void Move()
	{
		float num = speed * Time.deltaTime;
		Rect uvRect = rimgBg.uvRect;
		uvRect.y += num;
		rimgBg.uvRect = uvRect;
	}
}
