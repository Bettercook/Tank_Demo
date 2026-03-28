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
    //private float alphaSpeed = 35;
    [Header("淡入淡出时长（秒），推荐0.2~0.3")]
    public float fadeDuration = 0.2f; // 核心：固定动画时长，0.2秒最丝滑

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
    public virtual void ShowMe()
    {
        isShow = true;
        StopAllCoroutines(); // 停止旧协程，防止动画冲突
        StartCoroutine(SmoothFadeIn()); // 新的平滑淡入
    }

    public virtual void HideMe(UnityAction callBack)
    {
        isShow = false;
        hideCallBack = callBack;
        StopAllCoroutines(); // 停止旧协程，防止动画冲突
        StartCoroutine(SmoothFadeOut()); // 新的平滑淡出
    }

    /// <summary>
    /// 平滑淡入（Mathf.Lerp插值，0卡顿）
    /// </summary>
    private IEnumerator SmoothFadeIn()
    {
        float currentAlpha = canvasGroup.alpha;
        float elapsedTime = 0; // 已执行的动画时间

        // 插值渐变：从当前透明度到1，耗时fadeDuration
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            // 归一化进度：0→1，保证动画速度不随帧率变化
            float progress = Mathf.Clamp01(elapsedTime / fadeDuration);
            // 平滑插值alpha，比直接累加更加丝滑
            canvasGroup.alpha = Mathf.Lerp(currentAlpha, 1, progress);
            yield return null;
        }

        canvasGroup.alpha = 1; // 最终强制拉满，避免浮点数精度问题
    }

    /// <summary>
    /// 平滑淡出（Mathf.Lerp插值，0卡顿）
    /// </summary>
    private IEnumerator SmoothFadeOut()
    {
        float currentAlpha = canvasGroup.alpha;
        float elapsedTime = 0; // 已执行的动画时间

        // 插值渐变：从当前透明度到0，耗时fadeDuration
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / fadeDuration);
            canvasGroup.alpha = Mathf.Lerp(currentAlpha, 0, progress);
            yield return null;
        }

        canvasGroup.alpha = 0; // 最终强制拉到0
        hideCallBack?.Invoke(); // 执行隐藏回调（销毁/移除）
    }
}
