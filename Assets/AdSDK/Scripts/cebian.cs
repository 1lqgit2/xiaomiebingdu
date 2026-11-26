using UnityEngine;
using UnityEngine.UI;
using TTSDK;
using TTSDK.UNBridgeLib.LitJson;

/// <summary>
/// 侧边栏引导弹窗（挂在 cebianPanel 根节点上）。
///   1. 打开时把自己抬到独立 Canvas（overrideSorting + 高 sortingOrder），
///      避免被主界面上层的奖励按钮 / 进度条等界面盖住；
///   2. 把「前往侧边栏」按钮接到 TT.NavigateToScene(scene=sidebar)，跳转后自动关闭；
///   3. 「关闭」按钮由预制体实例上的持久化回调负责 SetActive(false)，这里再兜一次。
/// </summary>
public class cebian : MonoBehaviour
{
    /// <summary>「前往侧边栏」按钮</summary>
    public Button btn_Go;

    /// <summary>关闭按钮（可为空，实例上已有 SetActive(false) 的持久化回调）</summary>
    public Button btn_Close;

    /// <summary>弹窗提升后的排序值，高于项目里所有 UI 层（最高 130）</summary>
    private const int TopSortingOrder = 200;

    private void Awake()
    {
        EnsureTopCanvas();

        if (btn_Go != null)
        {
            btn_Go.onClick.RemoveListener(NavigateToSideBar);
            btn_Go.onClick.AddListener(NavigateToSideBar);
        }
        else
        {
            Debug.LogWarning("[侧边栏] 前往侧边栏按钮未绑定，点击将无反应");
        }

        if (btn_Close != null)
        {
            btn_Close.onClick.RemoveListener(ClosePanel);
            btn_Close.onClick.AddListener(ClosePanel);
        }
    }

    /// <summary>
    /// 给弹窗根节点挂一个覆盖排序的 Canvas，让它永远画在游戏内所有界面之上。
    /// 嵌套 Canvas 必须自带 GraphicRaycaster，否则里面的按钮点不到。
    /// </summary>
    private void EnsureTopCanvas()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas == null)
        {
            canvas = gameObject.AddComponent<Canvas>();
        }
        canvas.overrideSorting = true;
        canvas.sortingOrder = TopSortingOrder;

        if (GetComponent<GraphicRaycaster>() == null)
        {
            gameObject.AddComponent<GraphicRaycaster>();
        }
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    /// <summary>跳转到抖音侧边栏</summary>
    public void NavigateToSideBar()
    {
        try
        {
            JsonData data = new JsonData();
            data["scene"] = "sidebar";
            TT.NavigateToScene(data, () =>
            {
                Debug.Log("navigate to scene success");
                ClosePanel();
            }, () =>
            {
                Debug.Log("navigate to scene complete");
                ClosePanel();
            }, (errCode, errMsg) =>
            {
                Debug.Log($"navigate to scene error, errCode:{errCode}, errMsg:{errMsg}");
            });
        }
        catch (System.Exception e)
        {
            Debug.LogError("跳转到侧边栏异常: " + e.Message);
        }
    }
}
