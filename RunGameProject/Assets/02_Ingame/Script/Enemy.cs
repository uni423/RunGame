using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Hp;
    public float Ad;
    public float Exp;
    public float Speed;

    public bool IsDead;

    public void Start()
    {
        Hp = 10;
        Ad = 3;
        Exp = 100;
        Speed = 1;

        IsDead = false;
    }

    public void Update()
    {
        if (!GameManager.Instance.IsGamePlay)
            return;
        transform.position -= new Vector3(Speed, 0);
    }

    public float Damage(float damage)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            IsDead = true;
            Debug.Log("IsDead");
            Destroy(gameObject);
            return Exp;
        }

        return 0;
    }
    
    public void PlayerAattack(float value)
    {
        transform.position += new Vector3(value, 0);
    }    
}
