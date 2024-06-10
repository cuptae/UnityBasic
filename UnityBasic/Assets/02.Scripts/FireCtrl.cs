using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    //�Ѿ� ������
    public GameObject bullet;
    //�Ѿ� �߻� ��ǥ
    public Transform firePos;
    //ź�� ���� ��ƼŬ
    public ParticleSystem cartridge;
    //�ѱ� ȭ�� ��ƼŬ
    private ParticleSystem muzzleFlash;
    // Start is called before the first frame update
    void Start()
    {
        muzzleFlash = firePos.GetComponentInChildren<ParticleSystem>();
    }
    // Update is called once per frame
    void Update()
    {
        //���콺 ���� ��ư�� Ŭ�� ���� �� Fire �Լ� ȣ��
        if(Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    private void Fire()
    {
        //Bullet �������� �������� ����
        Instantiate(bullet,firePos.position,firePos.rotation);
        cartridge.Play();
        muzzleFlash.Play();
    }
}
