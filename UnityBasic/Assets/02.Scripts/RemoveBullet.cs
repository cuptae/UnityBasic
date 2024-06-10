using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    //����ũ �������� ������ ����
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
        //�浹 ������ ������ ����
        ContactPoint contact = coll.contacts[0];
        //���� ���Ͱ� �̷�� ȸ�������� ����
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);
        //����ũ ȿ���� ����
        GameObject spark = Instantiate(sparkEffect, contact.point, rot);
        spark.transform.SetParent(this.transform);
    }
}
