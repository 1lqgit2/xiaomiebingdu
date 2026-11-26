using UnityEngine;

namespace orangesGame
{
	public interface IEffectAction
	{
		void Spawn(Vector3 pos);

		void Spawn(Vector3 pos, Vector3 scale);

		void Spawn(Vector3 pos, Vector3 scale, int index);

		void Clean();
	}
}
