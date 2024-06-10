using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BarrelCtrl : MonoBehaviour
{
    // ���� ȿ�� �������� ������ ����
    public GameObject expEffect;
    //��׷��� �巳���� �޽��� ������ �迭
    public Mesh[] meshes;
    //�巳���� �ؽ�ó�� ������ �迭
    public Texture[] textures;

    //�Ѿ��� ���� Ƚ��
    private int hitCount = 0;
    //Rigidbody ������Ʈ�� ������ ����
    private Rigidbody rb;
    //MeshFilter ������Ʈ�� ������ ����
    private MeshFilter meshFilter;
    //MeshRenderer ������Ʈ�� ������ ����
    private MeshRenderer _renderer;

    //���� �ݰ�
    public float expRadius = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody ������Ʈ�� ������ ����
        rb = GetComponent<Rigidbody>();
        
        //MeshFilter ������Ʈ�� ������ ����
        meshFilter = GetComponent<MeshFilter>();
        
        //MeshRenderer ������Ʈ�� ������ ����
        _renderer= GetComponent<MeshRenderer>();

        //������ �߻����� �ұ�Ģ���� �ؽ�ó�� ����
        _renderer.material.mainTexture = textures[Random.Range(0, textures.Length)];
    }
    //�浹�� �߻����� �� �� �� ȣ��Ǵ� �ݹ� �Լ�
    private void OnCollisionEnter(Collision collision)
    {
        //�浹�� ���ӿ�����Ʈ�� �±׸� ��
        if(collision.collider.CompareTag("BULLET"))
        {
            if(++hitCount == 3)
            {
                ExpBarrel();
            }
        }
    }

    private void ExpBarrel()
    {
        //���� ȿ�� �������� �������� ����
        GameObject effect = Instantiate(expEffect, transform.transform.position, Quaternion.identity);
        Destroy(effect,2.0f);
        
        //Rigidbody ������Ʈ�� mass�� 1.0���� ������ ���Ը� ������ ��
        //rb.mass = 1.0f;
        //���� �ڱ�ġ�� ���� ����
        //rb.AddForce(Vector3.up * 1000.0f);

        //���߷� ����
        IndirectDamage(transform.position);

        //������ �߻�
        int idx = UnityEngine.Random.Range(0, meshes.Length);
        //��׷��� �޽��� ����
        meshFilter.sharedMesh = meshes[idx];
    }

    private void IndirectDamage(Vector3 position)
    {
        //�ֺ��� �ִ� �巳���� ��� ����
        Collider[] colls = Physics.OverlapSphere(position, expRadius, 1 << 8);
        foreach(var coll in colls)
        {
            //���� ������ ���Ե� �巳���� Ridgidbody ������Ʈ ����
            var _rb = coll.GetComponent<Rigidbody>();
            //�巳���� ���Ը� ������ ��
            _rb.mass = 1.0f;
            //���߷��� ����
            _rb.AddExplosionForce(1200.0f, position, expRadius, 1000.0f);
        }
    }
}
