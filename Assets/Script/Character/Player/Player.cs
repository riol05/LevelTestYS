using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;
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
    public LayerMask monsterMask;
    
    public Animator ani;
    int curExp;
    
    float gunShotCoolDown;
    
    public Gun gun;
    public Player(int fullhp, int damage, string name, int speed, int curHp) : base(fullhp, damage, name, speed, curHp)
    {
        fullhp = 10;
        damage = 1;
        name = "Player";
        speed = 2;
        curHp = fullhp;
    }

    public override void Awake()
    {
        base.Awake();
        coll = GetComponent<CapsuleCollider>();
        curHP = fullHP;
    }

    bool isCrouch;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetMoveDir();
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {

                Attack();
        }
        
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isCrouch= false;
            Crouch(isCrouch);
        }
        if (!agent.isStopped)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                isCrouch = true;
                Crouch(isCrouch);
            }
        }
        ani.SetFloat("Walk", agent.remainingDistance);
        agent.speed = 6;
        
    }
    private void Crouch(bool ison)
    {
        agent.speed += agent.speed;
        ani.SetBool("Crouch", ison);
    }

    public override void GetDamage(int Damage)
    {
        base.GetDamage(Damage);
    }

    public void GetEXP(int EXP)
    {
        curExp += EXP;
    }

    bool nowAttack;
    public void Attack()
    {
        if (Time.time > gunShotCoolDown + 1)
        {
            agent.ResetPath();
            agent.updatePosition = false;
            agent.updateRotation = false;
            agent.velocity = Vector3.zero;
            nowAttack = true;
            gunShotCoolDown = Time.time;
            ani.SetTrigger("Throw");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Vector3 direction;

            if (Physics.Raycast(ray, out hit))
            {
                direction = (hit.point - transform.position).normalized;
            }
            else
            {
                direction = transform.forward;
            }

            // 캐릭터 바라보는 방향을 마우스 방향으로 변경
            transform.rotation = Quaternion.LookRotation(direction);
            Invoke("OnThrowDagger",1f);
        }
    }

    private void OnThrowDagger()
    {
        gun.SpawnBullet();
        agent.updatePosition = true;
        agent.updateRotation = true;
        nowAttack = false;
    }


    public override void Move(Vector3 movePoint)
    {
        movePoint = this.movePoint.transform.position;
        if (agent.isStopped && !nowAttack)
        {
            agent.isStopped = false;
        }
        agent.velocity = Vector3.zero;
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
            Move(movePoint.position);
        }
        else if(Physics.Raycast(ray,out hit,100,monsterMask))
        {
            Attack();
        }
    }
    public override void Die()
    {
        GameManager.Instance.GameOver();
    }
}
