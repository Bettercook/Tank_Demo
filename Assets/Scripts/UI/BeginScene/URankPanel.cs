using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class URankPanel : BasePanel
{
    public Button btnClose;

    [Header("排行榜滚动面板")]
    public ScrollRect rankScrollRect; // 拖拽RankPanel下的 ScrollRect
    [Header("滚动内容容器")]
    public RectTransform content;     // 拖拽ScrollRect下的 Content
    [Header("排行榜条目预制体")]
    public RankItem rankItemPrefab;   // 拖拽制作好的 RankItem 预制体

    // 最多显示前5名
    private int maxShowCount = 5;

    public override void Init()
    {
        // 关闭按钮逻辑（完全保留）
        btnClose.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<URankPanel>();
        });
    }

    /// <summary>
    /// 根据数据 自动排序 + 自动创建条目
    /// </summary>
    public void UpdatePanelInfo()
    {
        // 1. 获取原始数据
        List<RankInfo> originList = GameDataMgr.Instance.rankData.list;

        // 2. 【核心】自动排序：分数从高到低 → 分数相同 时间更短排前
        var sortedList = originList
            .OrderByDescending(info => info.score)
            .ThenBy(info => info.time)
            .ToList();

        // 3. 清空旧的排行榜条目
        ClearOldItems();

        // 4. 动态创建条目（最多显示5条，和你原来一致）
        int showCount = Mathf.Min(sortedList.Count, maxShowCount);
        for (int i = 0; i < showCount; i++)
        {
            CreateSingleItem(sortedList[i], i + 1); // i+1 = 自动排名
        }
    }

    /// <summary>
    /// 创建单条排行榜UI
    /// </summary>
    private void CreateSingleItem(RankInfo info, int rank)
    {
        // 实例化预制体 → 放到Content下
        RankItem item = Instantiate(rankItemPrefab, content);
        item.transform.localScale = Vector3.one; // UGUI 必须重置缩放

        // 赋值数据
        item.txtRank.text = rank.ToString();
        item.txtName.text = info.name;
        item.txtScore.text = info.score.ToString();
        item.txtTime.text = FormatTime(info.time); // 时间格式化
    }

    /// <summary>
    /// 清空旧UI
    /// </summary>
    private void ClearOldItems()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// 时间格式化逻辑
    /// </summary>
    private string FormatTime(float time)
    {
        int totalSec = (int)time;
        string result = "";

        if (totalSec / 3600 > 0)
            result += totalSec / 3600 + "时";
        if (totalSec % 3600 / 60 > 0 || result != "")
            result += totalSec % 3600 / 60 + "分";
        result += totalSec % 60 + "秒";

        return result;
    }

    public override void ShowMe()
    {
        base.ShowMe();
        UpdatePanelInfo(); // 显示面板时刷新排行榜
    }
}
