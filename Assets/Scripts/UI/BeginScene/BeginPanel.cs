using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    public Button btnBegin;
    public Button btnSetting;
    public Button btnQuit;
    public Button btnRank;

    public override void Init()
    {
        btnBegin.onClick.AddListener(()=>
        {
            UIManager.Instance.HidePanel<BeginPanel>(false);
            //切换游戏场景
            SceneManager.LoadScene("GameScene");
            UIManager.Instance.ShowPanel<GamePanel>();
            //AsyncOperation async = SceneManager.LoadSceneAsync("GameScene");
            //async.completed += (op) => UIManager.Instance.ShowPanel<GamePanel>();
        });

        btnSetting.onClick.AddListener(()=>
        {
            //显示设置面板
            UIManager.Instance.ShowPanel<SettingPanel>();
        });

        btnQuit.onClick.AddListener(()=>
        {
            //退出
            Application.Quit();
        });

        btnRank.onClick.AddListener(()=>
        {
            UIManager.Instance.ShowPanel<RankPanel>();
        });
    }
}
