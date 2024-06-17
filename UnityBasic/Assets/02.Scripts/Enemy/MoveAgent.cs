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

    //NavMeshAgent ������Ʈ�� ������ ����
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        //NavMeshAgent ������Ʈ�� ������ �� ������ ����
        agent = GetComponent<NavMeshAgent>();
        //�������� ����������� �ӵ��� ���̴� �ɼ��� ��Ȱ��ȭ
        agent.autoBraking= false;

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

    // Update is called once per frame
    void Update()
    {
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
