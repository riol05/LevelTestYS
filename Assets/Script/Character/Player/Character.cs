using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    public int level;
    
    public int fullHP;
    //[HideInInspector]
    public int curHP;
    
    public int damage;

    public string name;

    public Dictionary<int, float> coolTime;
    public int speed;

    public int gameProgress;

    public NavMeshAgent agent;

    public Character( int fullhp, int damage, string name, int speed, int CurHP)
    {
        this.fullHP = fullhp;
        this.damage = damage;
        this.name = name;
        this.speed = speed;
        this.curHP = CurHP;
    }

    public virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    public virtual void GetDamage(int Damage) 
    {
        if(curHP > 0)
        {
            curHP-= Damage;
            if (curHP <= 0)
            { 
               Die();
            }
        }
        
    }
    public virtual void Die()
    {

    }
    public virtual void Move(Vector3 movePoint)
    {
    }


}
