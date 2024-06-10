using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAnim
{
    public AnimationClip idle;
    public AnimationClip runF;
    public AnimationClip runB;
    public AnimationClip runL;
    public AnimationClip runR;
}
public class PlayerCtrl : MonoBehaviour
{
    private float h = 0.0f;
    private float v = 0.0f;
    private float r = 0.0f;

    //�����ؾ� �ϴ� ������Ʈ�� �ݵ�� ������ �Ҵ��� �� ���
    private Transform tr;

    //�̵� �ӵ� ����(public���� ����Ǿ� Inspector�� �����)
    public float moveSpeed = 10.0f;
    //ȸ�� �ӵ� ����
    public float rotSpeed = 80.0f;

    //�ν����� �信 ǥ���� �ִϸ��̼� Ŭ���� ����
    public PlayerAnim playerAnim;
    //Animation ������Ʈ�� �����ϱ� ���� ����
    public Animation anim;
    // Start is called before the first frame update
    void Start()
    {
        //��ũ��Ʈ�� ����� �� ó�� ����Ǵ� Start �Լ����� Transform ������Ʈ�� �Ҵ�
        tr = GetComponent<Transform>();
        //Animation ������Ʈ�� ������ �Ҵ�
        anim = GetComponent<Animation>();
        //Animation ������Ʈ�� �ִϸ��̼� Ŭ���� �����ϰ� ����
        anim.clip = playerAnim.idle;
        anim.Play();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        // �����¿� �̵� ���� ���� ���
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        //Translate(�̵� ����* �ӵ� * ������ * Time.deltaTime, ������ǥ)
        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self);

        //Vector3.up ���� �������� rotSpeed��ŭ�� �ӵ��� ȸ��
        tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r);

        //Ű���� �Է°��� �������� ������ �ִϸ��̼� ����
        if(v >= 0.1f)
        {
            anim.CrossFade(playerAnim.runF.name, 0.3f); //���� �ִϸ��̼�
        }
        else if(v<=-0.1f)
        {
            anim.CrossFade(playerAnim.runB.name, 0.3f);//���� �ִϸ��̼�
        }
        else if (h >= 0.1f)
        {
            anim.CrossFade(playerAnim.runR.name, 0.3f);//������ �̵� �ִϸ��̼�
        }
        else if (h <= -0.1f)
        {
            anim.CrossFade(playerAnim.runL.name, 0.3f);//���� �̵� �ִϸ��̼�
        }
        else
        {
            anim.CrossFade(playerAnim.idle.name, 0.3f);//����  �� Idle �ִϸ��̼�
        }
    }
}
