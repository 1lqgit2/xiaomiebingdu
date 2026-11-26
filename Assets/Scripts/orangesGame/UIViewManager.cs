using DG.Tweening;
using DG.Tweening.Core;
using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class UIViewManager : Singleton<UIViewManager>
	{
		private enum Type
		{
			UI,
			Sprite
		}

		private class GameLayer
		{
			public int Layer;

			public Transform Tran;

			public GameLayer(int layer, Transform tran)
			{
				Layer = layer;
				Tran = tran;
			}
		}

		public static float Screen_WF;

		public const int LAYER_LONG = 1;

		public const int LAYER_SPRITE = 40;

		public const int LAYER_MID = 60;

		public const int LAYER_SHORT = 80;

		public const int LAYER_SHORTEST = 90;

		public const int LAYER_TOP = 100;

		public const int LAYER_TOP_TIP = 120;

		public const int LAYER_UI_130 = 130;

		public const int LAYER_SPRITE_SHORT = 110;

		public float uiHeight;

		public float screenHeight;

		public float screenWide;

		public float screenScale;

		public float originalHeight;

		private Camera _mainCamera;

		private Canvas _mainCanvas;

		private Dictionary<int, GameLayer> _layerDic;

		private Dictionary<string, UIView> _viewDic;

		private IFacade _facade;

		private bool isPropertyViewShow;

		[CompilerGenerated]
		private static DOGetter<Vector3> _003C_003Ef__mg_0024cache0;

		public float uiHeightHalf => uiHeight / 2f;

		public float screenHeightHalf => screenHeight / 2f;

		public float screenWideHalf => screenWide / 2f;

		public float originalHeightHalf => originalHeight / 2f;

		public Camera mainCamera => _mainCamera;

		public Canvas mainCanvas => _mainCanvas;

		private UIViewManager()
		{
			_layerDic = new Dictionary<int, GameLayer>();
			_viewDic = new Dictionary<string, UIView>();
			_facade = ApplicationFacade.Instance;
		}

		public void Init()
		{
			_mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
			_mainCanvas = GameObject.Find("MainCanvas").GetComponent<Canvas>();
			Vector3 localScale = _mainCanvas.transform.localScale;
			screenScale = localScale.x;
			SetLayer(1, Type.UI);
			SetLayer(60, Type.UI);
			SetLayer(80, Type.UI);
			SetLayer(90, Type.UI);
			SetLayer(100, Type.UI);
			SetLayer(120, Type.UI);
			SetLayer(40, Type.Sprite);
			SetLayer(110, Type.Sprite);
			SetLayer(130, Type.UI);
			screenHeight = (float)Screen.height / 100f;
			screenWide = (float)Screen.width / 100f;
			if (Screen.width <= 720 || Screen.height > 1440)
			{
			}
			uiHeight = (float)Screen.height * screenScale / 0.01f;
			Vector2 referenceResolution = _mainCanvas.GetComponent<CanvasScaler>().referenceResolution;
			originalHeight = referenceResolution.y / 100f;
			Screen_WF = (float)Screen.width * screenScale / 0.01f / 100f / 2f;
		}

		public float GetScreenWide()
		{
			return (float)Screen.width * screenScale / 0.01f;
		}

		public void LayerShake(int key, float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool fadeOut = true)
		{
			Transform tran = GetLayerTran(key);
			DOTween.Shake(() => Vector2.zero, delegate(Vector3 x)
			{
				tran.localPosition = x;
			}, duration, strength, vibrato, randomness, fadeOut);
		}

		public void LayerShake(int key, float duration, Vector3 strength, int vibrato = 10, float randomness = 90f, bool snapping = false, bool fadeOut = true, TweenCallback callback = null)
		{
			Transform tran = GetLayerTran(key);
			tran.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut).OnComplete(delegate
			{
				tran.localPosition = Vector2.zero;
				if (callback != null)
				{
					callback();
				}
			});
		}

		public void SetLayerPos(int key, Vector3 pos)
		{
			Transform layerTran = GetLayerTran(key);
			layerTran.position = Vector3.zero;
		}

		public Transform GetLayerTran(int key)
		{
			if (_layerDic.ContainsKey(key))
			{
				return _layerDic[key].Tran;
			}
			UnityEngine.Debug.LogError("GetLayerTran Key is not contains:" + key);
			return new GameObject("ErrorObj").transform;
		}

		public int GetLayerOrder(int key)
		{
			if (_layerDic.ContainsKey(key))
			{
				return _layerDic[key].Layer;
			}
			UnityEngine.Debug.LogError("GetLayerOrder Key is not contains:" + key);
			return 1;
		}

		public void RegisterView(string viewName, UIView view)
		{
			if (!_viewDic.ContainsKey(viewName))
			{
				view.OnRegister();
				_viewDic.Add(viewName, view);
			}
			else
			{
				UnityEngine.Debug.LogError("View register is contain:" + viewName);
			}
		}

		public void RemoveView(string viewName)
		{
			if (_viewDic.ContainsKey(viewName))
			{
				_viewDic[viewName].OnRemove();
				_viewDic.Remove(viewName);
			}
			else
			{
				UnityEngine.Debug.LogError("View register is null:" + viewName);
			}
		}

		public void ShowView(string viewName, Action callback = null)
		{
			if (_viewDic.ContainsKey(viewName))
			{
				_viewDic[viewName].OnShow(callback);
				GameAnalysisData.ClickAndShowView(viewName);
			}
			else
			{
				UnityEngine.Debug.LogError("ShowView is null:" + viewName);
			}
		}

		public void ShowView(string viewName, string notificationName, object body, Action callback = null)
		{
			if (_viewDic.ContainsKey(viewName))
			{
				_viewDic[viewName].OnShow(callback);
				GameAnalysisData.ClickAndShowView(viewName);
			}
			else
			{
				UnityEngine.Debug.LogError("ShowView is null:" + viewName);
			}
			_facade.SendNotification(notificationName, body);
		}

		public void ShowView(string viewName, string notificationName, Action callback = null)
		{
			if (_viewDic.ContainsKey(viewName))
			{
				_viewDic[viewName].OnShow(callback);
				GameAnalysisData.ClickAndShowView(viewName);
			}
			else
			{
				UnityEngine.Debug.LogError("ShowView is null:" + viewName);
			}
			_facade.SendNotification(notificationName);
		}

		public void ShowView(params string[] viewName)
		{
			for (int i = 0; i < viewName.Length; i++)
			{
				ShowView(viewName[i]);
			}
		}

		public void HideView(string viewName, Action callback = null)
		{
			if (_viewDic.ContainsKey(viewName))
			{
				_viewDic[viewName].OnHide(callback);
			}
			else
			{
				UnityEngine.Debug.LogError("HideView is null:" + viewName);
			}
		}

		public void HideView(string viewName, string notificationName, Action callback = null)
		{
			if (_viewDic.ContainsKey(viewName))
			{
				_viewDic[viewName].OnHide(callback);
			}
			else
			{
				UnityEngine.Debug.LogError("HideView is null:" + viewName);
			}
			_facade.SendNotification(notificationName);
		}

		public void HideViews(params string[] viewName)
		{
			for (int i = 0; i < viewName.Length; i++)
			{
				HideView(viewName[i]);
			}
		}

		public T GetView<T>(string viewName) where T : UIView
		{
			if (_viewDic.ContainsKey(viewName))
			{
				return _viewDic[viewName] as T;
			}
			UnityEngine.Debug.LogError("GetView is null:" + viewName);
			return new UIView() as T;
		}

		private void SetLayer(int key, Type type)
		{
			switch (type)
			{
			case Type.UI:
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load("GameUI/UILayer")) as GameObject;
				gameObject2.AddComponent<GraphicRaycaster>();
				Transform transform = gameObject2.transform;
				transform.SetParent(_mainCanvas.transform);
				GameUtils.SetTranOrigin(transform);
				GameUtils.SetUIStretch(transform);
				Canvas component = gameObject2.GetComponent<Canvas>();
				component.name = "LayerUI_" + key;
				component.sortingOrder = key;
				_layerDic.Add(key, new GameLayer(key, gameObject2.transform));
				break;
			}
			case Type.Sprite:
			{
				GameObject gameObject = new GameObject("LayerUI_" + key);
				gameObject.transform.localPosition = new Vector3(0f, 0f, key);
				_layerDic.Add(key, new GameLayer(key, gameObject.transform));
				break;
			}
			}
		}

		public void SetPropertyViewShow(bool _isPropertyViewShow)
		{
			isPropertyViewShow = _isPropertyViewShow;
		}

		public bool GetPropertyViewShow()
		{
			return isPropertyViewShow;
		}
	}
}
