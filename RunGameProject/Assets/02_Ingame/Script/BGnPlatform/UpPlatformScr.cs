using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UpPlatformScr : MonoBehaviour
{
    public float Speed;
    public bool Is_MoveOn = false;

    private void Start()
    {
        transform.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && Is_MoveOn == false)
        {
            Is_MoveOn = true;
            transform.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
            transform.DOLocalMoveY(-1000, Speed).From();
        }
    }
}
