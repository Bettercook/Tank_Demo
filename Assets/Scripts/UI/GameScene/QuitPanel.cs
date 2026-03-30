using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuitPanel : BasePanel
{
    public Button btnQuit;
    public Button btnGoOn;
    public Button btnClose;

    public override void Init()
    {
        //回到主界面
        btnQuit.onClick.AddListener(()=>
        {
            UIManager.Instance.HidePanel<QuitPanel>(false);
            UIManager.Instance.HidePanel<GamePanel>(false);
            Time.timeScale = 1;
            //回到开始界面
            SceneManager.LoadScene("BeginScene");

        });

        //都是关闭当前面板
        btnGoOn.onClick.AddListener(()=>
        {
            UIManager.Instance.HidePanel<QuitPanel>();
        });

        btnClose.onClick.AddListener(()=>
        {
            UIManager.Instance.HidePanel<QuitPanel>();
        });
    }

    public override void HideMe(UnityAction callBack)
    {
        base.HideMe(callBack);
        Time.timeScale = 1;
    }
}
