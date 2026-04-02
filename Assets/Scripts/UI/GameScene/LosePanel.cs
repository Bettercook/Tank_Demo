using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LosePanel : BasePanel
{
    public Button btnBack;
    public Button btnGoOn;

    public override void Init()
    {
        btnBack.onClick.AddListener(()=>
        {
            UIManager.Instance.HidePanel<GamePanel>(false);
            UIManager.Instance.HidePanel<LosePanel>(false);
            //取消暂停
            Time.timeScale = 1;
            //切换场景
            SceneManager.LoadScene("BeginScene");
        });

        btnGoOn.onClick.AddListener(()=>
        {
            UIManager.Instance.HidePanel<GamePanel>(false);
            UIManager.Instance.HidePanel<LosePanel>(false);
            //取消暂停
            Time.timeScale = 1;
            //再次切换到游戏场景就可以 达到所有内容重新加载 从头开始的效果
            SceneManager.LoadScene("GameScene");
        });
    }
}
