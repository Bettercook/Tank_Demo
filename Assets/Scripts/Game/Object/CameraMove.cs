using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //摄像机跟随的目标
    public Transform targetPlayer;
    public float H = 10;

    private Vector3 pos;

    // Update is called once per frame
    void LateUpdate()
    {
        if (targetPlayer == null)
            return;
        //x和z同玩家一样
        pos.x = targetPlayer.position.x;
        pos.z = targetPlayer.position.z;
        //外部调整摄像机高度
        pos.y = H;
        this.transform.position = pos;
    }
}
