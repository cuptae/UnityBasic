using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    //스파크 프리팹을 저장할 변수
    public GameObject sparkEffect;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "BULLET")
        {
            ShowEffect(collision);
            Destroy(collision.gameObject);
        }
    }
    void ShowEffect(Collision coll)
    {
        //충돌 지점의 정보를 추출
        ContactPoint contact = coll.contacts[0];
        //법선 벡터가 이루는 회전각도를 추출
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);
        //스파크 효과를 생성
        GameObject spark = Instantiate(sparkEffect, contact.point, rot);
        spark.transform.SetParent(this.transform);
    }
}
