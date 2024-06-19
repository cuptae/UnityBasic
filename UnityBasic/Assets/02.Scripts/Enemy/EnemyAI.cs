using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    //�� ĳ������ ���¸� ǥ���ϱ� ���� ������ ���� ����
    public enum State
    {
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }
    //���¸� ������ ����
    public State state = State.PATROL;

    //���ΰ��� ��ġ�� ������ ����
    private Transform playerTr;
    //�� ĳ������ ��ġ�� ������ ����
    private Transform enemyTr;
    //Animator ������Ʈ�� ������ ����
    private Animator animator;

    //���� �����Ÿ�
    public float attackDist = 5.0f;
    //���� �����Ÿ�
    public float traceDist = 10.0f;

    //��� ���θ� �Ǵ��� ����
    public bool isDie = false;

    //�ڷ�ƾ���� ����� �����ð� ����
    public WaitForSeconds ws;
    //�̵��� �����ϴ� MoveAgent Ŭ������ ������ ����
    private MoveAgent moveAgent;
    //�Ѿ� �߻縦 �����ϴ� EnemyFire Ŭ������ ������ ������
    private EnemyFire enemyFire;

    //�ִϸ����� ��Ʈ�ѷ��� ������ �Ķ������ �ؽð��� �̸� ����
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashDieIdx = Animator.StringToHash("DieIdx");

    private void Awake()
    {
        //���ΰ� ���ӿ�����Ʈ ����
        var player = GameObject.FindGameObjectWithTag("PLAYER");
        //���ΰ��� Transform ������Ʈ ����
        if(player != null)
        {
            playerTr = player.GetComponent<Transform>();
        }

        //�� ĳ������ Transform ������Ʈ ����
        enemyTr = GetComponent<Transform>();
        //Animator ������Ʈ ����
        animator = GetComponent<Animator>();
        //�̵��� �����ϴ� MoveAgent Ŭ������ ����
        moveAgent= GetComponent<MoveAgent>();
        //�Ѿ� �߻縦 �����ϴ� EnemyFire Ŭ������ ����
        enemyFire= GetComponent<EnemyFire>();

        //�ڷ�ƾ�� �����ð� ����
        ws = new WaitForSeconds(0.3f);
    }

    private void OnEnable()
    {
        //CheckState �ڷ�ƾ �Լ� ����
        StartCoroutine(CheckState());
        //Action �ڷ�ƾ �Լ� ����
        StartCoroutine(Action());
    }

    //�� ĳ������ ���¸� �˻��ϴ� �ڷ�ƾ �Լ�
    IEnumerator CheckState()
    {
        //�� ĳ���Ͱ� ����ϱ� ������ ���� ���ѷ���
        while(!isDie)
        {
            //���°� ����̸� �ڷ�ƾ �Լ��� �����Ŵ
            if (state == State.DIE) yield break;

            //���ΰ��� �� ĳ���� ���� �Ÿ��� ���
            float dist = (playerTr.position - enemyTr.position).sqrMagnitude;
            //float dist = Vector3.Distance(playerTr.position, enemyTr.position);

            //���� �����Ÿ� �̳��� ���
            if(dist <= attackDist)
            {
                state = State.ATTACK;
            }
            else if(dist <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.PATROL;
            }
            //0.3�� ���� ����ϴ� ���� ������� �纸
            yield return ws;
        }
    }

    //���¿� ���� �� ĳ������ �ൿ�� ó���ϴ� �ڷ�ƾ �Լ�
    IEnumerator Action()
    {
        while(!isDie)
        {
            yield return ws;
            //���¿� ���� �б� ó��
            switch (state)
            {
                case State.PATROL:
                    //�Ѿ� �߻� ����
                    enemyFire.isFire = false;
                    //���� ��带 Ȱ��ȭ
                    moveAgent.patrolling = true;
                    animator.SetBool(hashMove, true);
                    break;

                case State.TRACE:
                    //�Ѿ� �߻� ����
                    enemyFire.isFire = false;
                    //���ΰ��� ��ġ�� �Ѱ� ���� ���� ����
                    moveAgent.traceTarget = playerTr.position;
                    animator.SetBool(hashMove, true);
                    break;

                case State.ATTACK:
                    //���� �� ������ ����
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);

                    //�Ѿ� �߻� ����
                    if(enemyFire.isFire == false)
                    {
                        enemyFire.isFire = true;
                    }
                    break;

                case State.DIE:
                    isDie = true;
                    enemyFire.isFire = false;
                    //���� �� ������ ����
                    moveAgent.Stop();
                    //��� �ִϸ��̼��� ������ ����
                    animator.SetInteger(hashDieIdx, UnityEngine.Random.Range(0, 3));
                    //��� �ִϸ��̼� ����
                    animator.SetTrigger(hashDie);
                    //Capsule Collider ������Ʈ�� ��Ȱ��ȭ
                    GetComponent<CapsuleCollider>().enabled = false;
                    break;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        animator.SetFloat(hashSpeed, moveAgent.speed);
    }
}
