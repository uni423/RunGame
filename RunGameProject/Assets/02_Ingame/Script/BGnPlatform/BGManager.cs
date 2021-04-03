using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;

public class BGManager : MonoBehaviour
{
    public float Speed = 0f;
    public float BSpeed = 0f;
    public float Offset = 0f;
    public float BOffset = 0f;
    public bool Is_InSpeed = false;

    public Transform BackGround;

    public List<Middleground> MiddleGround = new List<Middleground>();

    public List<Grid> Tile = new List<Grid>();

    public void In_Speed(float speed)
    {
        Speed = speed;
        BSpeed = (10000 / Tile[0].transform.Find("Ground").GetComponent<TilemapCollider2D>().bounds.size.x) * Speed;
        foreach (var MG in MiddleGround)
        {
            MG.Setsp(speed);
        }
    }

    public void Move(float sp, float duration, float offset)
    {
        Is_InSpeed = true;

        Offset += offset;
        BOffset += offset;

        foreach (var MG in MiddleGround)
        {
            MG.Offset += (offset * 0.0005f);
        }

        DOTween.To(() => Speed, x => Speed = x, sp, duration)
            .From()
            .OnComplete(() => Is_InSpeed = false);
    }

    private void Start()
    {
        BSpeed = (10000 / Tile[0].transform.Find("Ground").GetComponent<TilemapCollider2D>().bounds.size.x) * Speed;
        //BSpeed = (10000 / Tile[0].transform.Find("Ground").GetComponent<CompositeCollider2D>().bounds.size.x) * Speed;
    }

    private void Update()
    {
        if (Is_InSpeed == true)
            In_Speed(Speed);

        Offset += Time.deltaTime * (Speed * 1.5f);
        BOffset += Time.deltaTime * BSpeed;

        BackGround.position = new Vector3(-1 * BOffset, 0);

        foreach(var grid in Tile)
            grid.transform.position = new Vector3(-1 * Offset, 0);
    }
}
