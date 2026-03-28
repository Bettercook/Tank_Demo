using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 音效数据类 用于存储音乐相关的信息
/// </summary>
public class MusicData
{
    //背景音乐是否开启
    public bool isOpenBK;
    //音效是否开启
    public bool isOpenSound;
    //背景音乐大小
    public float bkValue;
    //音效大小
    public float soundValue;

    //是否是第一次加载数据的标识
    public bool notFirst;
}
