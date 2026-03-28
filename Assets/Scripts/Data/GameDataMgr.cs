using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一个单例的游戏数据管理类
/// </summary>
public class GameDataMgr
{
    private static GameDataMgr instance = new GameDataMgr();

    public static GameDataMgr Instance => instance;

    //音效数据对象
    public MusicData musicData;
    //排行榜数据对象
    public RankList rankData;

    private GameDataMgr()
    {
        //初始化数据
        musicData = PlayerPrefsDataMgr.Instance.LoadData(typeof(MusicData), "Music") as MusicData;
        //如果第一次进入游戏，没有音效数据
        if (!musicData.notFirst)
        {
            musicData.notFirst = true;
            musicData.isOpenBK = true;
            musicData.isOpenSound = true;
            musicData.bkValue = 1;
            musicData.soundValue = 1;
            PlayerPrefsDataMgr.Instance.SaveData(musicData, "Music");
        }

        //初始化读取排行榜数据
        rankData = PlayerPrefsDataMgr.Instance.LoadData(typeof(RankList), "Rank") as RankList;
    }

    //提供一些API给外部 方便数据的改变存储

    /// <summary>
    /// 在排行榜中添加数据的方法
    /// </summary>
    /// <param name="name"></param>
    /// <param name="score"></param>
    /// <param name="time"></param>
    public void AddRankInfo(string name, int score, float time)
    {
        rankData.list.Add(new RankInfo(name, score, time));
        //排序
        rankData.list.Sort((a, b) => a.time < b.time ? -1 : 1);
        //从尾部往前遍历，移除10条以外的数据
        for (int i = rankData.list.Count - 1; i >= 10; i--)
        {
            rankData.list.RemoveAt(i);
        }
        //存储
        PlayerPrefsDataMgr.Instance.SaveData(rankData, "Rank");
    }

    /// <summary>
    /// 开启或者关闭背景音乐
    /// </summary>
    /// <param name="isOpen"></param>
    public void OpenOrCloseBKMusic(bool isOpen)
    {
        musicData.isOpenBK = isOpen;

        //背景音乐开关
        BKMusic.Instance.ChangeOpen(isOpen);

        //存储改变后的数据
        PlayerPrefsDataMgr.Instance.SaveData(musicData, "Music");
    }

    /// <summary>
    /// 开启或者关闭音效
    /// </summary>
    /// <param name="isOpen"></param>
    public void OpenOrCloseSound(bool isOpen)
    {
        musicData.isOpenSound = isOpen;
        //存储改变后的数据
        PlayerPrefsDataMgr.Instance.SaveData(musicData, "Music");
    }

    /// <summary>
    /// 改变背景音乐的大小
    /// </summary>
    /// <param name="value"></param>
    public void ChangeBKValue(float value)
    {
        musicData.bkValue = value;

        //背景音乐大小
        BKMusic.Instance.ChangeValue(value);

        PlayerPrefsDataMgr.Instance.SaveData(musicData, "Music");
    }


    /// <summary>
    /// 改变音效的大小
    /// </summary>
    /// <param name="value"></param>
    public void ChangeSoundValue(float value)
    {
        musicData.soundValue = value;

        PlayerPrefsDataMgr.Instance.SaveData(musicData, "Music");
    }
}
