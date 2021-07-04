﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    public EnemyData stat;
    public float NowHp;
    public Sprite dead;

    public bool Is_Dead;

    public void Start()
    {
        NowHp = stat.MaxHp;
        Is_Dead = false;
    }

    public float Damage(float damage)
    {
        NowHp -= damage;
        DOTween.Kill(this);
        transform.position = new Vector3(transform.position.x, -256);
        transform.DOMoveX(transform.position.x + 100, 0.2f);
        if (NowHp <= 0)
        {
            StartCoroutine(Dead());
            return stat.Exp;
        }
        return 0;
    }
    
    public IEnumerator Dead()
    {
        Is_Dead = true;
        transform.GetComponent<SpriteRenderer>().sprite = dead;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!Is_Dead)
                PlayerManager.Instance.Damage(stat.Ad, this.name);
        }
    }
}
