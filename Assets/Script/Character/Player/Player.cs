using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public enum PlayerState
{
    Idle,
    walk,
    Attack,
    Hit
}
public class Player : Character
{
    public Transform movePoint;
    CapsuleCollider coll;
    LayerMask monsterMask;

    int fullExp;
    int curExp;

    float gunShotCoolDown;
    
    public Gun gun;
    public Player(int level, int fullhp, int damage, string name,int speed) : base(level, fullhp, damage, name, speed)
    {
        level = 1;
        fullhp = 10;
        damage = 1;
        name = "Player";
        speed = 2;
    }

    public override void Awake()
    {
        base.Awake();
        coll = GetComponent<CapsuleCollider>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SetMoveDir();
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }
    public override void GetDamage(int Damage)
    {
        base.GetDamage(Damage);
    }

    public void Levelup(int level)
    {
        if(curExp >= fullExp )
        {
            curExp -= fullExp;
            ++level;
        }
    }
    public void Attack()
    {
        if (Time.time > gunShotCoolDown + .5)
        {
            gun.SpawnBullet();
            gunShotCoolDown = Time.time;
        }
    }
    public override void Move(Vector3 movePoint)
    {
        movePoint = this.movePoint.transform.position;
        if (agent.isStopped)
        {
            agent.isStopped = false;
        }
        agent.SetDestination(movePoint);
        //nextUpdateTime = Time.time + updateInterval;
    }

    private void SetMoveDir()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            movePoint.position = hit.point;
            Move(hit.point);
        }
        else if(Physics.Raycast(ray,out hit,100,monsterMask))
        {
            Attack();
        }
    }
}
