using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeObject : MonoBehaviour
{
    public GameObject[] rewardObjects;

    //死亡特效
    public GameObject deadEff;

    private void OnTriggerEnter(Collider other)
    {
        //子弹逻辑中已经处理过打中Cube销毁自己的逻辑
        //打到自己 处理随机创建奖励的逻辑

        //随机一个数来获取奖励
        int rangeInt = Random.Range(0, 100);
        //50%的几率 创建一个奖励
        if (rangeInt < 50)
        {
            //随机在当前位置创建一个奖励预设体
            rangeInt = Random.Range(0, rewardObjects.Length);
            //放在当前箱子所在位置 即可
            Instantiate(rewardObjects[rangeInt], this.transform.position,this.transform.rotation);
        }
        //播放奖励特效
        GameObject eff = Instantiate(deadEff, this.transform.position, this.transform.rotation);
        //改音效音量和开启状态
        AudioSource audioS = eff.GetComponent<AudioSource>();
        audioS.volume = GameDataMgr.Instance.musicData.soundValue;
        audioS.mute = !GameDataMgr.Instance.musicData.isOpenSound;

        Destroy(gameObject);
    }
}
