using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    //ÃÑ¾ËÀÇ ÆÄ±«·Â
    public float damage = 20.0f;
    //ÃÑ¾Ë ¹ß»ç ¼Óµµ
    public float speed = 1000.0f;
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
