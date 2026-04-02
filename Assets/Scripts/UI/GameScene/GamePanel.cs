using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    //分数
    public Text labScore;
    //时间
    public Text labTime;
    //退出按钮
    public Button btnQuit;
    //设置
    public Button btnSetting;
    //血量图
    public Image texHP;

    //血条控件的宽
    public float hpW = 450;

    //记录玩家当前的分数
    [HideInInspector]
    public int nowScore = 0;

    [HideInInspector]
    public float nowTime = 0;

    private int time;

    public override void Init()
    {
        // 游戏启动时 → 隐藏鼠标
        Cursor.visible = false;
        // 同时锁定鼠标位置
        Cursor.lockState = CursorLockMode.Locked;

        //打开设置面板
        btnSetting.onClick.AddListener(()=>
        {
            UIManager.Instance.ShowPanel<SettingPanel>();

            //改变时间缩放值 为0 即时间停止
            Time.timeScale = 0;
        });
        //退出 弹出确定退出的按钮
        btnQuit.onClick.AddListener(()=>
        {
            UIManager.Instance.ShowPanel<QuitPanel>();

            //改变时间缩放值 为0 即时间停止
            Time.timeScale = 0;
        });

    }

    // Update is called once per frame
    void Update()
    {
        //通过帧间隔时间进行累加 会比较准确
        nowTime += Time.deltaTime;

        time = (int)nowTime;
        labTime.text = "";

        if (time / 3600 > 0)
        {
            labTime.text += time / 3600 + "时";
        }
        if (time % 3600 / 60 > 0 || labTime.text != "")
        {
            labTime.text += time % 3600 / 60 + "分";
        }
        labTime.text += time % 60 + "秒";

        // 核心：判断是否打开了 除GamePanel外的其他面板
        bool showPanel = UIManager.Instance.GetPanel<SettingPanel>() != null
                      || UIManager.Instance.GetPanel<QuitPanel>() != null
                      || UIManager.Instance.GetPanel<WinPanel>() != null
                      || UIManager.Instance.GetPanel<LosePanel>() != null;

        // 有其他面板打开 → 显示鼠标；否则 → Alt控制
        if (showPanel)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            bool pressAlt = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
            Cursor.visible = pressAlt;
            Cursor.lockState = pressAlt ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    /// <summary>
    /// 提供给外部的加分方法
    /// </summary>
    /// <param name="score"></param>
    public void AddScore(int score)
    {
        nowScore += score;
        //更新界面的分数显示
        labScore.text = nowScore.ToString();
    }

    /// <summary>
    /// 更新血条的方法
    /// </summary>
    /// <param name="maxHP"></param>
    /// <param name="HP"></param>
    public void UpdateHP(int maxHP, int HP)
    {
        (texHP.transform as RectTransform).sizeDelta = new Vector2((float)HP / maxHP * hpW, 50);
    }
}
