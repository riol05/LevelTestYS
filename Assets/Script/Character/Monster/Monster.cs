using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MonsterState
{
    Idle,
    Patrol,
    Chase,
    Attack,
    Die,
}
public class Monster : Character
{
    private MonsterState curState;
    private Dictionary<MonsterState, Action> stateMachine;
    private Vector3 pointDir;
    public LayerMask PlayerMask;
    private float attackCooldown;
    public Transform[] movePoint;


    public Animator ani;
    private Coroutine monsterRoutine;
    public Monster( int fullhp, int damage, string name,int speed,int curHp) : base(fullhp, damage, name,speed, curHp)
    {
        fullhp = 3;
        damage = 1;
        name = "monster";
        speed = 3;
        curHp = fullhp;
    }
    public override void Awake()
    {
        base.Awake();
        curState = MonsterState.Idle;
        stateMachine = new Dictionary<MonsterState, Action>() {{ MonsterState.Idle, IdleMove },
            { MonsterState.Patrol,PatrolMove},
            { MonsterState.Chase,ChaseMove},
            {MonsterState.Attack,AttackMove},
            {MonsterState.Die,Die }
        };
        curHP = fullHP;
        //agent.speed = speed;
    }
    private void Start()
    {
        StartCoroutine(updateState());

    }

    private IEnumerator updateState()
    {
        while (curState != MonsterState.Die)
        {
            UpdateAnimation();
            pointDir = GameManager.Instance.player.transform.position;
            if (Vector3.Distance(transform.position, pointDir) < 2f)
            {
                curState = MonsterState.Attack;
                UpdateAnimation();
                agent.ResetPath();
                yield return new WaitForSeconds(2);
            }
            else if (Vector3.Distance(transform.position, pointDir) < 10f)
            {
                curState = MonsterState.Chase;
                yield return new WaitForSeconds(1);
            }
            else if (Vector3.Distance(transform.position,pointDir) >= 10f)
            {
                curState = MonsterState.Idle;
                yield return new WaitForSeconds(3);
                curState = MonsterState.Patrol;
                yield return new WaitForSeconds(5);
            }
            stateMachine[curState]?.Invoke();
            yield return null;
        }

        if (curState == MonsterState.Die)
        {
            ani.SetTrigger("Die");
            GameManager.Instance.GetScore();
            yield return new WaitForSeconds(2);
            SpawnManager.Instance.Despawn(gameObject);
        }

        stateMachine[curState]?.Invoke();
    }

    private void UpdateAnimation()
    {
        ani.SetFloat("Walk", agent.remainingDistance);
    }
    private void IdleMove()
    {
        if (agent.isStopped)
        {
            agent.isStopped = false;
        }
        monsterRoutine = null;

    }
    private void PatrolMove()
    {
        if (agent.isStopped)
        {
            agent.isStopped = false;
        }

        monsterRoutine = StartCoroutine(PatrolCoroutine());
    }
    

    private void ChaseMove()
    {
        if(agent.isStopped)
        {
            agent.isStopped = false;
        }
        monsterRoutine = StartCoroutine(ChaseCoroutine());
    }
    private void AttackMove()
    {
        monsterRoutine = null;
        if (Time.time > attackCooldown + 1)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hitPlayer;
            if (Physics.Raycast(ray, out hitPlayer, 2, PlayerMask, QueryTriggerInteraction.Ignore))
                {
                    hitPlayer.transform.GetComponent<Player>().GetDamage(damage);
                    agent.isStopped = true;
                }
            attackCooldown = Time.time;
            ani.SetTrigger("Punch");
        }
    }
    public override void GetDamage(int Damage)
    {
        if (curHP > 0)
        {
            curHP -= Damage;
            if (curHP <= 0)
            {
                curState = MonsterState.Die;
            }
        }
    }
    public override void Die()
    {
        agent.updatePosition = false;
        agent.updateRotation = false;

    }
    public override void Move(Vector3 dir)
    {
        if (agent.isStopped)
        {
            agent.isStopped = false;
        }
            agent.SetDestination(dir);
            UpdateAnimation();

    }

    IEnumerator ChaseCoroutine()
    {
        Move(pointDir);
        yield return null;
    }

    IEnumerator PatrolCoroutine()
    {
            int i = UnityEngine.Random.Range(0, movePoint.Length);
            if (movePoint[i] != null)
            {
                Move(movePoint[i].position);
            }
            yield return null;
    }
}
