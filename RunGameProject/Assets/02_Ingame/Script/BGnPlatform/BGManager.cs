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

    private void Start()
    {
        BSpeed = (10000 / Tile[0].transform.Find("Ground").GetComponent<TilemapCollider2D>().bounds.size.x) * Speed;
    }

    private void Update()
    {
        if (Is_InSpeed)
            In_Speed(Speed);

        Offset += Time.deltaTime * (Speed * 1.5f);
        BOffset += Time.deltaTime * BSpeed;

        BackGround.position = new Vector3(-1 * BOffset, 0);

        foreach (var grid in Tile)
            grid.transform.position = new Vector3(-1 * Offset, 0);
    }

    public void In_Speed(float speed, bool DOKill = false)
    {
        if (DOKill)
        {
            DOTween.Kill("MoveTW");
            Is_InSpeed = false;
        }

        Speed = speed;
        BSpeed = (10000 / Tile[0].transform.Find("Ground").GetComponent<TilemapCollider2D>().bounds.size.x) * Speed;
        foreach (var MG in MiddleGround)
        {
            MG.Setsp(Speed);
        }
    }

    public void Move(float sp, float duration)
    {
        Is_InSpeed = true;

        DOTween.To(() => Speed, x => Speed = x, sp, duration)
            .From()
            .SetId("MoveTW")
            .OnComplete(() =>
            {
                Is_InSpeed = false;
            });
    }
}
