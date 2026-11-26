using DG.Tweening;
using System;
using UnityEngine;

namespace orangesGame
{
	public class UIView : IView
	{
		private bool _IsInit;

		private bool _IsLoad;

		private bool _IsShow;

		private GameObject _Root;

		public bool IsInit => _IsInit;

		public bool IsLoad => _IsLoad;

		public bool IsShow => _IsShow;

		public GameObject Root => _Root;

		public virtual void OnInit()
		{
			_IsInit = true;
		}

		public virtual void OnLoad()
		{
			_IsLoad = true;
		}

		public virtual void OnShow(Action callback = null)
		{
			if (!_IsLoad)
			{
				OnLoad();
			}
			if (!_IsInit)
			{
				OnInit();
			}
			if (!_IsShow)
			{
				_IsShow = true;
				_Root.SetActive(_IsShow);
			}
		}

		public virtual void OnHide(Action callback = null)
		{
			if (_IsShow)
			{
				_IsShow = false;
				_Root.SetActive(_IsShow);
			}
		}

		public void Show(Transform tran, Vector3 to)
		{
			tran.localScale = Vector3.zero;
			tran.DOScale(to, 0.3f).SetAutoKill(autoKillOnCompletion: true).SetEase(Ease.OutBack);
		}

		public void Hide(Transform tran, Action callback = null)
		{
			tran.DOScale(Vector3.zero, 0.3f).SetAutoKill(autoKillOnCompletion: true).SetEase(Ease.InOutBack)
				.OnComplete(delegate
				{
					if (_IsShow)
					{
						_IsShow = false;
						_Root.SetActive(_IsShow);
					}
					if (callback != null)
					{
						callback();
					}
				});
		}

		public virtual void OnDestory()
		{
		}

		public virtual void OnRegister()
		{
		}

		public virtual void OnRemove()
		{
		}

		protected void CreateView(string path, Transform parent = null)
		{
            Debug.Log("Create View : " + path);
            if (parent != null)
            {
                Debug.Log(parent);
            }

			_Root = (UnityEngine.Object.Instantiate(Resources.Load(path)) as GameObject);
			Transform transform = _Root.transform;
			if (parent != null)
			{
				transform.SetParent(parent);
			}
			GameUtils.SetTranOrigin(transform);
			GameUtils.SetUIStretch(transform);
		}
	}
}
