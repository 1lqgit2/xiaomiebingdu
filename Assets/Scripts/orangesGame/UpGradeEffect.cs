using UnityEngine;

namespace orangesGame
{
	public class UpGradeEffect : IEffectAction
	{
		private string _order = "eff_player_upgarde";

		private string _resPath = "GameEffect/Player/UpGrade/eff_player_upgarde";

		public void Spawn(Vector3 pos)
		{
		}

		public void Spawn(Vector3 pos, Vector3 scale)
		{
			EffectBehaviour effectBehaviour = Singleton<EffectManager>.Instance.GetEffect(_order);
			if (effectBehaviour == null)
			{
				GameObject original = Resources.Load(_resPath) as GameObject;
				GameObject gameObject = Object.Instantiate(original);
				effectBehaviour = gameObject.GetComponent<EffectBehaviour>();
				effectBehaviour.SetParent(Singleton<UIViewManager>.Instance.GetLayerTran(40));
			}
			else
			{
				effectBehaviour.gameObject.SetActive(value: true);
			}
			effectBehaviour.tran.localPosition = pos;
			effectBehaviour.tran.localScale = scale;
		}

		public void Spawn(Vector3 pos, Vector3 scale, int index)
		{
		}

		public void Clean()
		{
		}
	}
}
