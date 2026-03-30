using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTower : TankBaseObj
{
    //间隔时间
    public float fireOffsetTime = 1;
    //记录累加时间 用于设置攻速阈值
    private float nowTime = 0;

    //发射位置
    public Transform[] shootPos;

    //子弹预设体关联
    public GameObject bulletObj;

    // Update is called once per frame
    void Update()
    {
        //不停累加时间
        nowTime += Time.deltaTime;
        //时间超过间隔时间时 就开火
        if (nowTime > fireOffsetTime)
        {
            Fire();
            nowTime = 0;
        }
        
    }

    public override void Fire()
    {
        for (int i = 0; i < shootPos.Length; i++)
        {
            //实例化几个子弹
            GameObject obj = Instantiate(bulletObj, shootPos[i].position, shootPos[i].rotation);
            //设置子弹拥有者，方便进行属性计算
            BulletObj bullet = obj.GetComponent<BulletObj>();
            bullet.SetFather(this);
        }
    }

    public override void Wound(TankBaseObj other)
    {
        //什么内容都不写
        //让这个固定不动的坦克不被伤害 永远不死
    }
}
