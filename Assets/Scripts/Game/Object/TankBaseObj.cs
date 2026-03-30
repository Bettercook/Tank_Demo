using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TankBaseObj : MonoBehaviour
{
    [Header("拖入血条预制体")]
    public GameObject hpBarPrefab;
    private HpBar myHpBar;

    //攻击力
    public int atk;
    //防御力
    public int def;
    //最大血量
    public int maxHp;
    //当前血量
    public int hp;

    //所有坦克 都有炮台相关
    public Transform tankHead;

    //移动旋转速度相关
    public float moveSpeed = 10;
    public float roundSpeed = 100;
    public float headRoundSpeed = 100;

    //死亡特效 关联对应预设体 死亡时动态创建，设置位置即可
    public GameObject deadEff;

    protected virtual void Start()
    {
        // 初始化自己的血量
        //hp = maxHp;
        // 创建【自己的血条】（只属于自己，别人看不到）
        if (hpBarPrefab != null)
        {
            GameObject barObj = Instantiate(hpBarPrefab, UIManager.Instance.canvasTrans);
            myHpBar = barObj.GetComponent<HpBar>();
            myHpBar.BindOwner(transform); // 血条绑定自己
        }
    }

    /// <summary>
    /// 开火抽象方法 子类重写开火行为即可
    /// </summary>
    public abstract void Fire();

    /// <summary>
    /// 我被别人攻击 造成我自己受伤
    /// </summary>
    /// <param name="other"></param>
    public virtual void Wound(TankBaseObj other)
    {
        int dmg = other.atk - this.def;
        if (dmg <= 0)
            return;
        //如果伤害大于0 就应该减血
        this.hp -= dmg;

        if (myHpBar != null)
            myHpBar.Show();

        //判断，如果血量<=0了，就应该死亡
        if (this.hp <= 0)
        {
            this.hp = 0;
            this.Dead();
        }
    }

    /// <summary>
    /// 死亡行为 如果血量小于等于0，就应该死亡
    /// </summary>
    public virtual void Dead()
    {
        // 销毁自己的血条
        if (myHpBar != null) Destroy(myHpBar.gameObject);

        //对象死亡，在场景上移除该对象
        Destroy(this.gameObject);
        //死亡时，所有对象都应该播放一个死亡特效
        if (deadEff != null)
        {
            //实例化对象的同时 设置位置和角度
            GameObject effObj = Instantiate(deadEff, this.transform.position, this.transform.rotation);
            //该特效身上直接关联了音效 故可以在此处控制音效
            AudioSource audioSource = effObj.GetComponent<AudioSource>();
            //根据音乐数据设置
            //音效大小
            audioSource.volume = GameDataMgr.Instance.musicData.soundValue;
            //是否开启
            audioSource.mute = !GameDataMgr.Instance.musicData.isOpenSound;
            //避免没有勾选Play on Awake
            audioSource.Play();
        }
    }
}
