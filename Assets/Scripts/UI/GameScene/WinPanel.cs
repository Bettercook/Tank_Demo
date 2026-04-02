using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinPanel : BasePanel
{
    public InputField inputInfo;
    public Button btnSure;

    public override void Init()
    {

        btnSure.onClick.AddListener(()=>
        {
            //把数据记录到排行榜中，并且回到主场景中
            GameDataMgr.Instance.AddRankInfo(inputInfo.text, 
                UIManager.Instance.GetPanel<GamePanel>().nowScore,
                UIManager.Instance.GetPanel<GamePanel>().nowTime);

            //返回开始界面即可
            UIManager.Instance.HidePanel<GamePanel>(false);
            UIManager.Instance.HidePanel<WinPanel>(false);
            Time.timeScale = 1;
            //切换游戏场景
            SceneManager.LoadScene("BeginScene");
        });
    }
}
