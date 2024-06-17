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
        //�̵��� �����ϴ� MoveAgent Ŭ������ ����
        moveAgent= GetComponent<MoveAgent>();

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
        while(isDie)
        {
            yield return ws;
            //���¿� ���� �б� ó��
            switch (state)
            {
                case State.PATROL:
                    //���� ��带 Ȱ��ȭ
                    moveAgent.patrolling = true;
                    break;

                case State.TRACE:
                    //���ΰ��� ��ġ�� �Ѱ� ���� ���� ����
                    moveAgent.traceTarget = playerTr.position;
                    break;

                case State.ATTACK:
                    //���� �� ������ ����
                    moveAgent.Stop();
                    break;

                case State.DIE:
                    //���� �� ������ ����
                    moveAgent.Stop();
                    break;
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
