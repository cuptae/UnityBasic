using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private const string bulletTag = "BULLET";
    //���� ������
    private float hp = 100f;
    //�ǰ� �� ����� ���� ȿ��
    private GameObject bloodEffect;


    // Start is called before the first frame update
    void Start()
    {
        bloodEffect = Resources.Load<GameObject>("BulletImpactFleshBigEffect");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == bulletTag)
        {
            //���� ȿ���� �����ϴ� �Լ� ȣ��
            ShowBloodEffect(collision);
            //�Ѿ� ����
            Destroy(collision.gameObject);
            //���� ������ ����
            hp -= collision.gameObject.GetComponent<BulletCtrl>().damage;
            if(hp <= 0)
            {
                //�� ĳ������ ���¸� DIE�� ����
                GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
            }
        }
    }

    private void ShowBloodEffect(Collision collision)
    {
        //�Ѿ��� �浹�� ���� ����
        Vector3 pos = collision.contacts[0].point;
        //�Ѿ��� �浹���� ���� ���� ����
        Vector3 _normal = collision.contacts[0].normal;
        //�Ѿ��� �浹 �� ���� ������ ȸ���� �Ի�
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward,_normal);

        //���� ȿ�� ����
        GameObject blood = Instantiate(bloodEffect, pos, rot);
        Destroy(blood, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
