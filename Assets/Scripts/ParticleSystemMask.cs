using UnityEngine;

public class ParticleSystemMask : MonoBehaviour
{
	[SerializeField]
	private RectTransform _maskRectTran;

	[SerializeField]
	private ParticleSystem[] _particlesSystems;

	private void Start()
	{
		Vector3[] array = new Vector3[4];
		_maskRectTran.GetWorldCorners(array);
		float x = array[0].x;
		float y = array[0].y;
		float x2 = array[2].x;
		float y2 = array[2].y;
		ParticleSystem[] particlesSystems = _particlesSystems;
		foreach (ParticleSystem particleSystem in particlesSystems)
		{
			particleSystem.GetComponent<Renderer>().sharedMaterial.SetFloat("_MinX", x);
			particleSystem.GetComponent<Renderer>().sharedMaterial.SetFloat("_MinY", y);
			particleSystem.GetComponent<Renderer>().sharedMaterial.SetFloat("_MaxX", x2);
			particleSystem.GetComponent<Renderer>().sharedMaterial.SetFloat("_MaxY", y2);
		}
	}
}
