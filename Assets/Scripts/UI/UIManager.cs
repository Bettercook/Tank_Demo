using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private static UIManager instance = new UIManager();

    public static UIManager Instance => instance;

    //用于存储显示着的面板 每显示一个面板，就会存入这个字典
    //隐藏面板时直接获取字典中的面板进行隐藏
    private Dictionary<string, BasePanel> panlDic = new Dictionary<string, BasePanel>();

    //暴露全局Canvas的Transform，方便血条挂载
    public Transform canvasTrans;

    private UIManager()
    {
        //得到场景中的Canvas对象
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        canvasTrans = canvas.transform;
        //通过过场景不移除该对象，保证这个游戏过程中 只有一个Canvas对象
        GameObject.DontDestroyOnLoad(canvas);
    }

    //显示面板
    public T ShowPanel<T>() where T : BasePanel
    {
        //只需要保证 泛型T的类型 和面板预设体名字一致 定一个这样的规则
        string panelName = typeof(T).Name;

        //判断字典中是否已经显示了这个面板
        if(panlDic.ContainsKey(panelName))
            return panlDic[panelName] as T;

        //根据面板名字 动态创建预设体 设置父对象
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + panelName));
        //把对象放到场景中的Canvas下面
        panelObj.transform.SetParent(canvasTrans, false);

        //执行面板上的显示逻辑 并且保存起来
        T panel = panelObj.GetComponent<T>();
        //把这个面板脚本存储到字典中 方便后面的获取和隐藏
        panlDic.Add(panelName, panel);
        //调用显示逻辑
        //panel.ShowMe();
        panel.StartCoroutine(ShowPanelDelay(panel));

        return panel;
    }
    private IEnumerator ShowPanelDelay(BasePanel panel)
    {
        yield return null; // 等待1帧，这一帧会执行Start() → Init()
        panel.ShowMe();    // 此时UI已经初始化完成，绝对安全！
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <typeparam name="T">面板类名</typeparam>
    /// <param name="isFade">是否淡出完毕后再删除面板 默认是true</param>
    public void HidePanel<T>(bool isFade = true) where T: BasePanel
    {
        string panelName = typeof(T).Name;
        //判断当前显示的面板有没有要隐藏的
        if (panlDic.ContainsKey(panelName))
        {
            if (isFade)
            {
                //让面板淡出完毕后再删除
                panlDic[panelName].HideMe(()=>
                {
                    //删除对象
                    GameObject.Destroy(panlDic[panelName].gameObject);
                    //删除字典里的脚本 否则会有内存泄露
                    panlDic.Remove(panelName);
                });
            }
            else
            {
                //删除对象
                GameObject.Destroy(panlDic[panelName].gameObject);
                //删除字典里的脚本 否则会有内存泄露
                panlDic.Remove(panelName);
            }
        }
    }

    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (panlDic.ContainsKey(panelName))
            return panlDic[panelName] as T;
        //若没有对应面板显示，返回空
        return null;
    }
}
