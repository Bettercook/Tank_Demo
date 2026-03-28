using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKMusic : MonoBehaviour
{
    private static BKMusic instance;

    public static BKMusic Instance => instance;

    private AudioSource audioSource;

    void Awake()
    {
        instance = this;
        //得到音频源脚本
        audioSource = GetComponent<AudioSource>();
        //初始化就根据数据进行设置
        ChangeValue(GameDataMgr.Instance.musicData.bkValue);
        ChangeOpen(GameDataMgr.Instance.musicData.isOpenBK);
    }

    /// <summary>
    /// 改变背景音乐大小
    /// </summary>
    /// <param name="value"></param>
    public void ChangeValue(float value)
    {
        audioSource.volume = value;
    }

    /// <summary>
    /// 开关背景音乐
    /// </summary>
    /// <param name="isOpen"></param>
    public void ChangeOpen(bool isOpen)
    {
        audioSource.mute = !isOpen;
    }
}
