using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UpHurdle : MonoBehaviour
{
    public EnemyData stat;
    public bool IsUp;
    private float vector;

    private void Start()
    {
        if (IsUp)
            transform.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
    }


    public void Update()
    {
        if (!GameManager.Instance.IsGamePlay)
            return;
        if (transform.position.x <= 700 && IsUp)
        {
            vector = transform.localPosition.y;
            IsUp = false;
            transform.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
            //transform.position = new Vector3(transform.position.x, -1000);
            transform.DOMoveY(-1000, 0.4f).From();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
                collision.GetComponent<Player>().Damage(stat.Ad);
        }
    }
}
