using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 旧版本（2019 年前）保存的 AnimatorController 在 Unity 2022 下运行时可能丢失动画状态，
/// 表现为 Animator.Play 时报 “Invalid Layer Index '-1'”“GotoState: State could not be found”，
/// 导致通关/失败结算依赖的动画结束回调不触发，流程卡在战斗界面。
///
/// 执行本菜单会把 Assets/AnimatorController 下的控制器按当前 Unity 版本重新序列化一次，
/// 一般可以彻底修复动画状态丢失的问题（每次升级大版本 Unity 后都建议执行一次）。
/// </summary>
public static class AnimatorControllerFix
{
	[MenuItem("工具/修复动画控制器（重新序列化）")]
	public static void Reserialize()
	{
		string[] guids = AssetDatabase.FindAssets("t:AnimatorController", new string[1] { "Assets/AnimatorController" });
		if (guids == null || guids.Length == 0)
		{
			Debug.LogWarning("[Animator] Assets/AnimatorController 下没有找到 AnimatorController");
			return;
		}

		List<string> paths = new List<string>(guids.Length);
		for (int i = 0; i < guids.Length; i++)
		{
			paths.Add(AssetDatabase.GUIDToAssetPath(guids[i]));
		}

		AssetDatabase.ForceReserializeAssets(paths, ForceReserializeAssetsOptions.ReserializeAssetsAndMetadata);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		Debug.Log("[Animator] 已重新序列化 " + paths.Count + " 个 AnimatorController，请重新运行游戏验证通关/失败流程");
	}
}
