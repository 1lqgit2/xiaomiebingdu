using UnityEngine;

namespace orangesGame
{
	public class VenomBlasterExplo : MonoBehaviour
	{
		protected const string TAG_PLAYER = "Player";

		private void OnTriggerEnter2D(Collider2D col)
		{
			if (col.gameObject.CompareTag("Player") && !GameGlobalData.IsDie && !GameGlobalData.IsHurt && GameGlobalData.GameState == GameState.Gaming)
			{
				GameGlobalData.IsDie = true;
				Singleton<SdkUtils>.Instance.Shake(GameGlobalData.ShakeFlag);
				ApplicationFacade.Instance.SendNotification("GAME_FAIL");
			}
		}
	}
}
