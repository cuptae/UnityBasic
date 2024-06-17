using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MoveAgent : MonoBehaviour
{
    //���� �������� �����ϱ� ���� List Ÿ�� ����
    public List<Transform> wayPoints;
    //���� ���� ������ �迭�� Index
    public int nextIdx;

    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;

    //NavMeshAgent ������Ʈ�� ������ ����
    private NavMeshAgent agent;

    //���� ���θ� �Ǵ��ϴ� ����
    private bool _patrolling;
    //patrolling ������Ƽ ����(getter, setter)
    public bool patrolling
    {
        get { return _patrolling; }
        set 
        {
            _patrolling = value;
            if(_patrolling)
            {
                agent.speed = patrolSpeed;
                MoveWayPoint();
            }
        }
    }

    //���� ����� ��ġ�� �����ϴ� ����
    private Vector3 _traceTarget;
    //traceTarget ������Ƽ ����(getter, setter)
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            TraceTarget(_traceTarget);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //NavMeshAgent ������Ʈ�� ������ �� ������ ����
        agent = GetComponent<NavMeshAgent>();
        //�������� ����������� �ӵ��� ���̴� �ɼ��� ��Ȱ��ȭ
        agent.autoBraking= false;

        agent.speed = patrolSpeed;

        //���̷�Ű ���� WayPointGroup ���ӿ�����Ʈ�� ����
        var group = GameObject.Find("WayPointGroup");
        if(group != null)
        {
            //WayPointGroup ������ �ִ� ��� Transform ������Ʈ�� ������ ��
            //List Ÿ���� wayPoints �迭�� �߰�
            group.GetComponentsInChildren<Transform>(wayPoints);
            //�迭�� ù ��° �׸� ����
            wayPoints.RemoveAt(0);
        }
        MoveWayPoint();
    }

    private void MoveWayPoint()
    {
        //�ִܰŸ� ��� ����� ������ �ʾ����� ������ �������� ����
        if (agent.isPathStale) return;

        //���� �������� wayPoints �迭���� ������ ��ġ�� ���� �������� ����
        agent.destination = wayPoints[nextIdx].position;
        //�׺���̼� ����� Ȱ��ȭ�ؼ� �̵��� ������
        agent.isStopped = false;
    }

    //���ΰ��� ������ �� �̵���Ű�� �Լ�
    void TraceTarget(Vector3 pos)
    {
        if(agent.isPathStale) return;

        agent.destination = pos;
        agent.isStopped = false;
    }

    //���� �� ������ ������Ű�� �Լ�
    public void Stop()
    {
        agent.isStopped= true;
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_patrolling) return;

        //NavMeshAgent�� �̵��ϰ� �ְ� �������� �����ߴ��� ���θ� ���
        if (agent.velocity.sqrMagnitude > 0.2f * 0.2f
            && agent.remainingDistance<= 0.5f)
        {
            //���� �������� �迭 ÷�ڸ� ���
            nextIdx = ++nextIdx % wayPoints.Count;
            //���� �������� �̵� ����� ����
            MoveWayPoint();
        }
    }
}
