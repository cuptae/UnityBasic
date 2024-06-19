using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//������̼� ����� ����ϱ� ���� �߰��ؾ� �ϴ� ���ӽ����̽�
using UnityEngine.AI;

public class MoveAgent : MonoBehaviour
{
    //���� �������� �����ϱ� ���� ListŸ���� ����
    public List<Transform> wayPoints;
    //���� ���� ������ �迭�� Index
    public int nextIdx;

    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;
    //ȸ���� ���� �ӵ��� �����ϴ� ���
    private float damping = 1.0f;

    //NavMeshAgent ������Ʈ�� ������ ����
    private NavMeshAgent agent;
    //�� ĳ������ Transform ������Ʈ�� ������ ����
    private Transform enemyTr;

    //���� ���θ� �Ǵ��ϴ� ����
    private bool _patrolling;
    //patrolling ������Ƽ ����(getter, setter)
    public bool patrolling
    {
        get { return _patrolling; }
        set
        {
            _patrolling = value;
            if (_patrolling)
            {
                agent.speed = patrolSpeed;
                //���� ������ ȸ�����
                damping = 1.0f;
                MoveWayPoint();
            }
        }
    }
    //���� ����� ��ġ�� �����ϴ� ����
    private Vector3 _traceTarget;
    //traceTarget ������Ƽ ����(getter,setter)
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            //���� ������ ȸ�����
            damping = 7.0f;
            TraceTarget(_traceTarget);
        }
    }

    //NavMeshAgent�� �̵� �ӵ��� ���� ������Ƽ ����(getter)
    public float speed
    {
        get { return agent.velocity.magnitude; }
    }

    void Start()
    {
        //�� ĳ������ Transform ������Ʈ ���� �� ������ ����
        enemyTr = GetComponent<Transform>();
        //NavMeshAgent ������Ʈ�� ������ �� ������ ����
        agent = GetComponent<NavMeshAgent>();
        //�������� ����������� �ӵ��� ���̴� �ɼ��� ��Ȱ��ȭ
        agent.autoBraking = false;
        //�ڵ����� ȸ���ϴ� ����� ��Ȱ��ȭ
        agent.updateRotation = false;

        agent.speed = patrolSpeed;

        //���̷�Ű ���� WayPointGroup ���ӿ�����Ʈ�� ����
        var group = GameObject.Find("WayPointGroup");
        if (group != null)
        {
            //WayPointGroup ������ �ִ� ��� Transform ������Ʈ�� ������ ��
            //List Ÿ���� wayPoints �迭�� �߰�
            group.GetComponentsInChildren<Transform>(wayPoints);
            //�迭�� ù ��° �׸� ����
            wayPoints.RemoveAt(0);
        }
        MoveWayPoint();
    }

    //���� ���������� �̵� ����� ������ �Լ�
    void MoveWayPoint()
    {
        //�ִܰŸ� ��� ����� ������ �ʾ����� ������ �������� ����
        if (agent.isPathStale) return;
        //���� �������� wayPoints �迭���� ������ ��ġ�� ���� �������� ����
        agent.destination = wayPoints[nextIdx].position;
        //������̼� ����� Ȱ��ȭ�ؼ� �̵��� ������
        agent.isStopped = false;
    }

    //���ΰ��� ������ �� �̵���Ű�� �Լ�
    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;
        agent.destination = pos;
        agent.isStopped = false;
    }

    //���� �� ������ ������Ű�� �Լ�
    public void Stop()
    {
        agent.isStopped = true;
        //�ٷ� �����ϱ� ���� �ӵ��� 0���� ����
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }

    void Update()
    {
        //�� ĳ���Ͱ� �̵� ���� ���� ȸ��
        if (agent.isStopped == false)
        {
            //NavMeshAgent�� ���� �� ���� ���͸� ���ʹϾ� Ÿ���� ������ ��ȯ
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            //���� �Լ��� ����� ���������� ȸ����Ŵ
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }

        //���� ��尡 �ƴ� ��� ���� ������ �������� ����
        if (!_patrolling) return;

        //NavMeshAgent�� �̵��ϰ� �ְ� �������� �����ߴ��� ���θ� ���
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f
            && agent.remainingDistance <= 0.5f)
        {
            //���� �������� �迭 ÷�ڸ� ���
            nextIdx = ++nextIdx % wayPoints.Count;
            //���� �������� �̵� ����� ����
            MoveWayPoint();
        }
    }
}
