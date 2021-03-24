using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Hp;
    public EnemyData stat;

    public bool IsDead;

    public void Start()
    {
        Hp = 10;
        IsDead = false;
    }

    public void Update()
    {
        if (!GameManager.Instance.IsGamePlay)
            return;
        transform.position -= new Vector3(stat.Speed, 0);
    }

    public float Damage(float damage)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            IsDead = true;
            Destroy(gameObject);
            return stat.Exp;
        }

        return 0;
    }
    
    public void PlayerAattack(float value)
    {
        transform.position += new Vector3(value, 0);
    }    
}
