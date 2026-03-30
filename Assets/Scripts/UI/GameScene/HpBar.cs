using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HpBar : MonoBehaviour
{
    [Header("头顶偏移")]
    public Vector3 headOffset = new Vector3(0, 1.5f, 0);
    private Slider hpSlider;
    private CanvasGroup canvasGroup;
    private Transform owner; // 血条归属者（谁的血条就跟随谁）
    private TankBaseObj ownerTank; // 归属者的坦克脚本

    void Awake()
    {
        hpSlider = GetComponentInChildren<Slider>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0; // 初始隐藏

    }

    // 绑定血条归属者（谁的血条）
    public void BindOwner(Transform target)
    {
        owner = target;
        if (target != null)
        {
            ownerTank = target.GetComponent<TankBaseObj>();
        }
    }

    // 外部调用：显示血条1秒
    public void Show()
    {
        StopAllCoroutines();
        StartCoroutine(AutoHide());
    }

    // 1秒后自动隐藏
    IEnumerator AutoHide()
    {
        canvasGroup.alpha = 1;
        yield return new WaitForSeconds(1f);
        canvasGroup.alpha = 0;
    }

    void Update()
    {
        // 主人死亡，直接销毁血条
        if (owner == null)
        {
            Destroy(gameObject);
            return;
        }

        // 跟随主人头顶
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, owner.position + headOffset);
        GetComponent<RectTransform>().position = screenPos;

        // 实时更新主人血量（100%准确）
        if (hpSlider != null && ownerTank != null)
        {
            hpSlider.value = (float)ownerTank.hp / ownerTank.maxHp;
        }
            
    }
}