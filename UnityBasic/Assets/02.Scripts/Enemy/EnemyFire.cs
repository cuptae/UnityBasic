using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    //AudioSource ������Ʈ�� ������ ����
    private AudioSource audio;
    //Animator ������Ʈ�� ������ ����
    private Animator animator;
    //���ΰ� ĳ������ Transform ������Ʈ
    private Transform playerTr;
    //�� ĳ������ Transform ������Ʈ
    private Transform enemyTr;

    //�ִϸ����� ��Ʈ�ѷ��� ������ �Ķ������ �ؽð��� �̸� ����
    private readonly int hashFire = Animator.StringToHash("Fire");
    private readonly int hashReload = Animator.StringToHash("Reload");

    //���� �߻��� �ð� ���� ����
    private float nextFire = 0.0f;
    //�Ѿ� �߻� ����
    private readonly float fireRate = 0.1f;
    //���ΰ��� ���� ȸ���� �ӵ� ���
    private readonly float damping = 10.0f;

    private readonly float reloadTime = 2.0f;   //������ �ð�
    private readonly int maxBullet = 10;        //źâ�� �ִ� �Ѿ� ��
    private int currBullet = 10;                //�ʱ� �Ѿ� ��
    private bool isReload = false;              //������ ����
    //������ �ð� ���� ��ٸ� ���� ����
    private WaitForSeconds wsReload;

    //�� �߻� ���θ� �Ǵ��� ����
    public bool isFire = false;
    //�� �߻� ���带 ������ ����
    public AudioClip fireSfx;
    //������ ���带 ������ ����
    public AudioClip reloadSfx;

    //�� ĳ������ �Ѿ� ������
    public GameObject Bullet;
    //�Ѿ��� �߻� ��ġ ����
    public Transform firePos;
    //MuzzleFlash�� MeshRenderer ������Ʈ�� ������ ����
    public MeshRenderer muzzleFlash;

    void Start()
    {
        //������Ʈ ���� �� ���� ����
        playerTr = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();
        enemyTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();

        wsReload = new WaitForSeconds(reloadTime);

        //MuzzleFlash�� ��Ȱ��ȭ
        muzzleFlash.enabled= false;
    }

    void Update()
    {
        if (!isReload&&isFire)
        {
            //���� �ð��� ���� �߻� �ð����� ū���� Ȯ��
            if (Time.time >= nextFire)
            {
                Fire();
                //���� �߻� �ð� ���
                nextFire = Time.time + fireRate + Random.Range(0.0f, 0.3f);
            }
            //���ΰ��� �ִ� ��ġ������ ȸ�� ���� ���
            Quaternion rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);
            //�����Լ��� ����� ���������� ȸ����Ŵ
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }
    }
    void Fire()
    {
        animator.SetTrigger(hashFire);
        audio.PlayOneShot(fireSfx, 1.0f);
        //�ѱ� ȭ�� ȿ�� �ڷ�ƾ ȣ��
        StartCoroutine(ShowMuzzleFlash());

        //�Ѿ��� ����
        GameObject _bullet = Instantiate(Bullet, firePos.position, firePos.rotation);
        //���� �ð��� ���� �� ����
        Destroy(_bullet, 3.0f);

        //���� �Ѿ˷� ������ ���θ� ���
        isReload = (--currBullet % maxBullet == 0);
        if(isReload)
        {
            StartCoroutine(Reloading());
        }
    }

    IEnumerator ShowMuzzleFlash()
    {
        //MuzzleFlash Ȱ��ȭ
        muzzleFlash.enabled= true;

        //�ұ�Ģ�� ȸ�� ������ ���
        Quaternion rot = Quaternion.Euler(Vector3.forward * Random.Range(0, 360));
        //MuzzleFlash�� Z�� �������� ȸ��
        muzzleFlash.transform.localRotation= rot;
        //MuzzleFlash�� �������� �ұ�Ģ�ϰ� ����
        muzzleFlash.transform.localScale = Vector3.one * Random.Range(1.0f, 2.0f);

        //�ؽ�ó�� Offset �Ӽ��� ������ �ұ�Ģ�� ���� ����
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;
        //MuzzleFlash�� ��Ƽ������ Offset ���� ����
        muzzleFlash.material.SetTextureOffset("_MainTex", offset);

        //MuzzleFlash�� ���� ���� ��� ���
        yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        //MuzzleFlash�� �ٽ� ��Ȱ��ȭ
        muzzleFlash.enabled= false;
    }

    IEnumerator Reloading()
    {
        //MuzzleFlash�� ��Ȱ��ȭ
        muzzleFlash.enabled= false;
        //������ �ִϸ��̼� ����
        animator.SetTrigger(hashReload);
        //������ ���� �߻�
        audio.PlayOneShot(reloadSfx, 1.0f);

        //������ �ð���ŭ ����ϴ� ���� ����� �纾
        yield return wsReload;

        //�Ѿ��� ������ �ʱ�ȭ
        currBullet = maxBullet;
        isReload = false;
    }

}
