using System;
using UnityEngine;

namespace orangesGame
{
	public class AnimatorWidget : MonoBehaviour
	{
		public Animator Animer;

		public bool IsDestory;

		private Action finishCallback;

		private int _LayerIndex;

		private bool _IsPause;

		/// <summary>
		/// 动画回调兜底。
		/// 旧版本 AnimatorController 在 Unity 2022 下可能出现 controller 失效 / 状态找不到
		/// （日志表现为 “Invalid Layer Index '-1'”“Animator.GotoState: State could not be found”），
		/// 此时 normalizedTime 永远是 0，Update 里的回调永远不会触发，
		/// 结算流程会卡在战斗界面出不来。这里加一个超时兜底，保证回调一定会执行。
		/// </summary>
		private const float FALLBACK_DURATION = 3f;

		private int _fallbackToken;

		private void Awake()
		{
			if (Animer == null)
			{
				Animer = GetComponent<Animator>();
			}
			_IsPause = false;
			_LayerIndex = 0;
		}

		private void Update()
		{
			if (!_IsPause && Animer.GetCurrentAnimatorStateInfo(_LayerIndex).normalizedTime >= 1f)
			{
				if (finishCallback != null)
				{
					InvokeFinishCallback();
				}
				if (IsDestory)
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
				_IsPause = true;
			}
		}

		public void Play(string name)
		{
			if (!Animer.enabled)
			{
				Animer.enabled = true;
			}
			if (base.gameObject.activeInHierarchy)
			{
				Animer.Play(name);
				_IsPause = false;
			}
		}

		public void Play(string name, Action callback)
		{
			SetCallback(callback);
			Play(name);

			// Controller 整个丢失时动画永远不会结束（layerCount = 0），直接回调，避免流程卡住
			if (Animer == null || Animer.runtimeAnimatorController == null || Animer.layerCount <= 0)
			{
				UnityEngine.Debug.LogWarning("[动画] " + name + "：Animator 没有加载到 Controller，跳过动画直接继续流程");
				InvokeFinishCallback();
				return;
			}

			int stateHash = Animator.StringToHash(name);
			if (!HasPlayableState(name))
			{
				UnityEngine.Debug.LogWarning("[动画] " + name + "：Animator 中找不到该状态，若动画没有开始播放将强制继续流程");
			}

			StartFallback(name, stateHash);
		}

		/// <summary>
		/// 判断该状态名在 Animator 的任意一层里是否存在。
		/// 旧版本 Controller 在 Unity 2022 下可能失效，此时 layerCount 为 0 或状态丢失，
		/// Animator.Play 会打印 “Invalid Layer Index '-1' / State could not be found”。
		/// </summary>
		private bool HasPlayableState(string name)
		{
			if (Animer == null || Animer.runtimeAnimatorController == null || Animer.layerCount <= 0)
			{
				return false;
			}
			int stateHash = Animator.StringToHash(name);
			for (int i = 0; i < Animer.layerCount; i++)
			{
				if (Animer.HasState(i, stateHash))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 超时兜底：只处理「动画根本没开始播放」的情况（Controller/状态失效）。
		/// 动画正常播放时（哪怕被 timeScale 放慢）当前状态哈希或 normalizedTime 会对上，
		/// 这里直接放手，由 Update 负责回调。用 token 防止上一段动画的兜底误触发新回调。
		/// </summary>
		private void StartFallback(string name, int stateHash)
		{
			if (finishCallback == null)
			{
				return;
			}
			int token = ++_fallbackToken;
			Timer.Register(FALLBACK_DURATION, delegate
			{
				if (token != _fallbackToken || finishCallback == null)
				{
					return;
				}
				AnimatorStateInfo info = Animer.GetCurrentAnimatorStateInfo(_LayerIndex);
				if (info.shortNameHash == stateHash || info.normalizedTime > 0.01f)
				{
					// 动画确实在播放，交给 Update 等它自然结束
					return;
				}
				UnityEngine.Debug.LogWarning("[动画] " + name + " 没有开始播放（Controller 或动画状态失效），强制触发回调，避免流程卡死");
				InvokeFinishCallback();
			}, null, false, true);
		}

		private void InvokeFinishCallback()
		{
			Action action = finishCallback;
			finishCallback = null;
			_IsPause = true;
			if (action != null)
			{
				action();
			}
		}

		public void Recover()
		{
			Animer.enabled = true;
		}

		public void Pause()
		{
			Animer.enabled = false;
		}

		public void AddStateInfo(int layerIndex)
		{
			if (layerIndex <= Animer.layerCount)
			{
				_LayerIndex = layerIndex;
				return;
			}
			_LayerIndex = 0;
			UnityEngine.Debug.LogError("动画层数错误：" + layerIndex);
		}

		public void SetCallback(Action callback)
		{
			finishCallback = callback;
		}
	}
}
