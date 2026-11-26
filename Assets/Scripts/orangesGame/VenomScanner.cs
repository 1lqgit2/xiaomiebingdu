using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class VenomScanner : MonoBehaviour
	{
		public float Radius;

		public LayerMask LayerMask;

		public EventHealerScannerTargetStay EventOnTargetStay = new EventHealerScannerTargetStay();

		public EventHealerScannerRemoveTarget EventOnRemoveTarget = new EventHealerScannerRemoveTarget();

		public EventHealerScannerProcessTarget EventOnProcessTarget = new EventHealerScannerProcessTarget();

		private Collider2D[] _scannerResults = new Collider2D[25];

		private List<GameObject> _venomGameObjects = new List<GameObject>();

		private List<GameObject> _cacheVenomGameObjects = new List<GameObject>();

		private float _tempSinceTime;

		private float _delay;

		private bool _isUpdate;

		protected void onTargetFind(int count)
		{
			_venomGameObjects.Clear();
			for (int i = 0; i < count; i++)
			{
				if (_scannerResults[i] != null)
				{
					_venomGameObjects.Add(_scannerResults[i].gameObject);
				}
			}
			for (int j = 0; j < _cacheVenomGameObjects.Count; j++)
			{
				if (!_venomGameObjects.Contains(_cacheVenomGameObjects[j]) && _cacheVenomGameObjects[j] != null)
				{
					EventOnRemoveTarget.Invoke(_cacheVenomGameObjects[j]);
				}
			}
			_cacheVenomGameObjects.Clear();
			for (int k = 0; k < _venomGameObjects.Count; k++)
			{
				if (_venomGameObjects[k] != null)
				{
					EventOnTargetStay.Invoke(_venomGameObjects[k]);
					_cacheVenomGameObjects.Add(_venomGameObjects[k]);
				}
			}
		}

		private void LateUpdate()
		{
			_tempSinceTime += Time.deltaTime;
			if (_tempSinceTime > _delay)
			{
				int num = Physics2D.OverlapCircleNonAlloc(base.transform.position, Radius, _scannerResults, LayerMask);
				if (num > 0)
				{
					_isUpdate = false;
					onTargetFind(num);
				}
				else if (!_isUpdate)
				{
					_isUpdate = true;
					onTargetFind(num);
				}
				_tempSinceTime = 0f;
			}
			EventOnProcessTarget.Invoke();
		}
	}
}
