using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterObj : TankBaseObj
{
    private Transform targetPos;

    //随机用的点 外面关联
    public Transform[] randomPos;

    //一直盯着自己的目标
    public Transform lookAtTarget;

    //开火距离 小于该距离时就会主动攻击
    public float fireDis = 5;

    //攻击间隔时间
    public float fireOffsetTime = 1;
    private float nowTime = 0;

    //开火点
    public Transform[] shootPos;
    //子弹预设体
    public GameObject bulletObj;

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        RandomPos();
    }

    // Update is called once per frame
    void Update()
    {
        //看向自己的目标点
        this.transform.LookAt(targetPos);
        //不停地朝自己的面朝向位移
        this.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        //距离过小时，认为到达了目的地，重新随机
        if (Vector3.Distance(this.transform.position, targetPos.transform.position) < 0.05f)
        {
            RandomPos();
        }

        if (lookAtTarget != null)
        {
            tankHead.LookAt(lookAtTarget);
            //自己和目标距离小于预设的开火距离时 就开火
            if (Vector3.Distance(this.transform.position, lookAtTarget.position) <= fireDis)
            {
                nowTime += Time.deltaTime;
                if (nowTime >= fireOffsetTime)
                {
                    Fire();
                    nowTime = 0;
                }
            }
        }
    }

    private void RandomPos()
    {
        if (randomPos.Length == 0)
            return;
        targetPos = randomPos[Random.Range(0, randomPos.Length)];
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

    public override void Dead()
    {
        base.Dead();
        //加分
        UIManager.Instance.GetPanel<GamePanel>().AddScore(10);
    }

    
}
