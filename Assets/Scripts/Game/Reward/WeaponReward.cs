using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReward : MonoBehaviour
{
    //多个用于随机的武器对象
    public GameObject[] weaponObj;

    //获取特效
    public GameObject getEff;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //让玩家切换武器
            int index = Random.Range(0, weaponObj.Length);
            //获取玩家对象 命令其切换武器
            PlayerObj player = other.GetComponent<PlayerObj>();
            player.ChangeWeapon(weaponObj[index]);

            //播放奖励特效
            GameObject eff = Instantiate(getEff, this.transform.position, this.transform.rotation);
            //改音效音量和开启状态
            AudioSource audioS = eff.GetComponent<AudioSource>();
            audioS.volume = GameDataMgr.Instance.musicData.soundValue;
            audioS.mute = !GameDataMgr.Instance.musicData.isOpenSound;

            //获取到自己后 移除自己
            Destroy(this.gameObject);
        }
    }
}
