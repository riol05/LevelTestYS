using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    public int level;
    
    public int fullHP;
    [HideInInspector]
    public int curHP;
    
    public int damage;

    public string name;

    public Dictionary<int, float> coolTime;
    public int speed;

    public int gameProgress;

    public Rigidbody rb;
    public NavMeshAgent agent;

    public Character(int level, int fullhp, int damage, string name,int speed)
    {
        this.level = level;
        this.fullHP = fullhp;
        this.damage = damage;
        this.name = name;
        this.speed = speed;
    }

    public Character(int level, int fullhp, int damage, string name)
    {
        this.level = level;
        fullHP = fullhp;
        this.damage = damage;
        this.name = name;
    }

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        curHP = fullHP;
    }
    public virtual void GetDamage(int Damage) 
    {
        if(curHP > 0)
        {
            curHP-= Damage;
            if (curHP < 0)
                Die();
        }
            Die();
    }
    public virtual void Die()
    {

    }
    public virtual void Move(Vector3 movePoint)
    {
    }


}
