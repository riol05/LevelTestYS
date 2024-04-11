using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
public enum MonsterState
{
    Idle,
    Patrol,
    Chase,
    Attack
}
public class Monster : Character
{
    private MonsterState curState;
    private Dictionary<MonsterState, Action> stateMachine;
    private Vector3 pointDir;
    public LayerMask PlayerMask;
    private float attackCooldown;
    public Transform[] movePoint;

    private Coroutine monsterRoutine;
    public Monster(int level, int fullhp, int damage, string name,int speed) : base(level, fullhp, damage, name,speed)
    {
        level = 1;
        fullhp = 5;
        damage = 1;
        name = "monster";
        speed = 1;
    }
    public override void Awake()
    {
        base.Awake();
        curState = MonsterState.Idle;
        stateMachine = new Dictionary<MonsterState, Action>() {{ MonsterState.Idle, IdleMove },
            { MonsterState.Patrol,PatrolMove},
            { MonsterState.Chase,ChaseMove},
            {MonsterState.Attack,AttackMove},
        };
    }

    private IEnumerator Start()
    {
        pointDir = GameManager.Instance.player.transform.position;
        if ((pointDir - transform.position).magnitude > 20f)
        {
            curState = MonsterState.Idle;
            yield return new WaitForSeconds(3);
            curState = MonsterState.Patrol;
            yield return new WaitForSeconds(5);
        }
        else if ((pointDir - transform.position).magnitude > 10f)
        {
            curState = MonsterState.Chase;
            transform.localPosition = Vector3.zero;
            yield return new WaitForSeconds(2);
        }
        else if ((pointDir - transform.position).magnitude > 3f)
        {
            curState = MonsterState.Attack;
            Move(pointDir - transform.position * speed * Time.deltaTime);
            yield return new WaitForSeconds(1);
        }
        stateMachine[curState]?.Invoke();

    }

    private void IdleMove()
    {
        monsterRoutine = null;
    }
    private void PatrolMove()
    {
        monsterRoutine = StartCoroutine(PatrolCoroutine());
    }
    private void ChaseMove()
    {
        monsterRoutine = StartCoroutine(ChaseCoroutine());
    }
    private void AttackMove()
    {
        monsterRoutine = null;
        if (Time.time > attackCooldown + 1)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hitPlayer;
            if (Physics.Raycast(ray, out hitPlayer, 3, PlayerMask, QueryTriggerInteraction.Ignore))
                {
                    GameManager.Instance.player.GetDamage(damage);
                }
            attackCooldown = Time.time;
        }
    }
    public override void GetDamage(int Damage)
    {
        base.GetDamage(Damage);
    }
    public override void Die()
    {
        SpawnManager.Instance.Despawn(gameObject);
    }
    public override void Move(Vector3 dir)
    {
        if (agent.isStopped)
        {
            agent.isStopped = false;

            agent.SetDestination(movePoint[i].position);
     
        }
    }

    IEnumerator ChaseCoroutine()
    {
        Move(pointDir - transform.position * speed * Time.deltaTime);
        yield return new WaitForSeconds(2);
    }

    IEnumerator PatrolCoroutine()
    {
        while (true)
        {
            int i = UnityEngine.Random.Range(0, movePoint.Length);
            if (movePoint[i] != null)
            {
                Move(pointDir);
            }
            yield return new WaitForSeconds(2);
        }

    }
}
