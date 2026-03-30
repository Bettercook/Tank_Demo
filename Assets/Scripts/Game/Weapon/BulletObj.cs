using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObj : MonoBehaviour
{
    //移动速度
    public float moveSpeed = 50;
    //谁发射的我
    public TankBaseObj fatherObj;
    //特效对象
    public GameObject effObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    //碰撞触发
    private void OnTriggerEnter(Collider other)
    {
        //子弹射击到立方体会爆炸
        //子弹射击到不同阵营的对象也应该爆炸
        if (other.CompareTag("Cube") ||
            other.CompareTag("Player") && fatherObj.CompareTag("Monster") ||
            other.CompareTag("Monster") && fatherObj.CompareTag("Player"))
        {
            //判断是否受伤
            //得到碰撞体身上是否有坦克相关脚本，使用里式替换原则 通过父类获取
            TankBaseObj obj = other.GetComponent<TankBaseObj>();
            if (obj != null)
                obj.Wound(fatherObj);

            //子弹销毁时 创建爆炸特效
            if (effObj != null)
            {
                GameObject eff = Instantiate(effObj, this.transform.position, this.transform.rotation);
                //改音效音量和开启状态
                AudioSource audioS = eff.GetComponent<AudioSource>();
                audioS.volume = GameDataMgr.Instance.musicData.soundValue;
                audioS.mute = !GameDataMgr.Instance.musicData.isOpenSound;

            }
            Destroy(gameObject);
        }
    }

    //设置拥有者
    public void SetFather(TankBaseObj obj)
    {
        fatherObj = obj;
    }
}
