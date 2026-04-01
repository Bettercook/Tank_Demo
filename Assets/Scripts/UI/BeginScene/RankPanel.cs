using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankPanel : BasePanel
{
    public Button btnClose;

    private List<Text> labPM = new List<Text>();
    private List<Text> labName = new List<Text>();
    private List<Text> labScore = new List<Text>();
    private List<Text> labTime = new List<Text>();

    public override void Init()
    {
        for (int i = 1; i <= 5; i++)
        {
            //可以通过斜杠来区分父子关系
            labPM.Add(transform.Find("PM/labPM" + i).GetComponent<Text>());
            labName.Add(transform.Find("Name/labName" + i).GetComponent<Text>());
            labScore.Add(transform.Find("Score/labScore" + i).GetComponent<Text>());
            labTime.Add(transform.Find("Time/labTime" + i).GetComponent<Text>());
        }

        btnClose.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<RankPanel>();
        });
    }

    /// <summary>
    /// 根据排行榜数据 更新面板
    /// </summary>
    public void UpdatePanelInfo()
    {
        //得数据
        List<RankInfo> list = GameDataMgr.Instance.rankData.list;

        for (int i = 0; i < list.Count; i++)
        {
            //名字
            labName[i].text = list[i].name;
            //分数
            labScore[i].text = list[i].score.ToString();
            //时间 存储的单位是秒
            int time = (int)list[i].time;
            labTime[i].text = "";

            if (time / 3600 > 0)
            {
                labTime[i].text += time / 3600 + "时";
            }
            if (time % 3600 / 60 > 0 || labTime[i].text != "")
            {
                labTime[i].text += time % 3600 / 60 + "分";
            }
            labTime[i].text += time % 60 + "秒";
        }


    }

    public override void ShowMe()
    {
        base.ShowMe();
        UpdatePanelInfo();
    }
}
