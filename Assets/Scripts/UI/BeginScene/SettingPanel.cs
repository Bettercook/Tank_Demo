using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    public Button btnClose;
    public Toggle togMusic;
    public Toggle togSound;
    public Slider sliderMusic;
    public Slider sliderSound;

    public override void Init()
    {
        //处理音乐
        sliderMusic.onValueChanged.AddListener((v)=>
        {
            GameDataMgr.Instance.ChangeBKValue(v);
        });

        //处理音效
        sliderSound.onValueChanged.AddListener((v)=>
        {
            GameDataMgr.Instance.ChangeSoundValue(v);
        });

        togMusic.onValueChanged.AddListener((v) =>
        {
            GameDataMgr.Instance.OpenOrCloseBKMusic(v);
        });

        togSound.onValueChanged.AddListener((v)=>
        {
            GameDataMgr.Instance.OpenOrCloseSound(v);   
        });

        btnClose.onClick.AddListener(()=>
        {
            //隐藏自己
            UIManager.Instance.HidePanel<SettingPanel>();
        });
    }

    public void UpdatePanelInfo()
    {
        //面板上的数据都是根据 音效数据 更新的
        MusicData data = GameDataMgr.Instance.musicData;

        //设置面板内容
        sliderMusic.value = data.bkValue;
        sliderSound.value = data.soundValue;

        togMusic.isOn = data.isOpenBK;
        togSound.isOn = data.isOpenSound;
    }

    public override void ShowMe()
    {
        base.ShowMe();
        //每次显示面板时顺便把面板上的内容也更新了
        UpdatePanelInfo();
    }

    public override void HideMe(UnityAction callBack)
    {
        base.HideMe(callBack);
        Time.timeScale = 1;
    }
}
