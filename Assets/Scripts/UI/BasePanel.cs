using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    //专门用于控制面板透明度的组件
    private CanvasGroup canvasGroup;
    //淡入淡出的速度
    private float alphaSpeed = 10;

    //当前的显隐状态
    public bool isShow = false;
    
    //当隐藏完毕后 想做的事情
    private UnityAction hideCallBack = null;

    protected virtual void Awake()
    {
        //一开始就获取面板上挂载的 组件
        canvasGroup = GetComponent<CanvasGroup>();
        //如果忘记添加这样一个脚本
        if (canvasGroup == null )
            canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Init();
    }

    /// <summary>
    /// 注册控件事件的方法 所有的子面板都需要去注册一些控件事件
    /// 所以写成抽象方法 让子类必须去实现
    /// </summary>
    public abstract void Init();

    ///// <summary>
    ///// 显示自己时的逻辑
    ///// </summary>
    //public virtual void ShowMe()
    //{
    //    canvasGroup.alpha = 0;
    //    isShow = true;
    //}

    ///// <summary>
    ///// 隐藏自己时的逻辑
    ///// </summary>
    //public virtual void HideMe(UnityAction callBack)
    //{
    //    canvasGroup.alpha = 1;
    //    isShow = false;

    //    hideCallBack = callBack;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    Debug.Log("Update执行了：" + gameObject.name);

    //    //处于显示状态，且透明度不为1
    //    //则不断加透明度 直到1，才会停止
    //    //淡入
    //    if (isShow && canvasGroup.alpha != 1)
    //    {
    //        canvasGroup.alpha += alphaSpeed * Time.deltaTime;
    //        if (canvasGroup.alpha >= 1)
    //            canvasGroup.alpha = 1;
    //    }
    //    //淡出
    //    else if (!isShow && canvasGroup.alpha != 0)
    //    {
    //        canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
    //        if (canvasGroup.alpha <= 0)
    //        {
    //            canvasGroup.alpha = 0;
    //            //面板淡出完毕后 再去执行一些逻辑
    //            hideCallBack?.Invoke();
    //        }
    //    }
    //}
    // 核心修复：用协程淡入，不依赖Update！
    public virtual void ShowMe()
    {
        isShow = true;
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    // 核心修复：用协程淡出，不依赖Update！
    public virtual void HideMe(UnityAction callBack)
    {
        isShow = false;
        hideCallBack = callBack;
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    // 淡入协程（场景切换也能正常执行）
    private IEnumerator FadeIn()
    {
        canvasGroup.alpha = 0;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;
    }

    // 淡出协程
    private IEnumerator FadeOut()
    {
        canvasGroup.alpha = 1;
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= alphaSpeed * Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0;
        hideCallBack?.Invoke();
    }
}
