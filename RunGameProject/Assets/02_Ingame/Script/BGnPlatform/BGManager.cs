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
    public float[] TreeOffset = new float[3];
    public float BOffset = 0f;
    public float Diff = 0f;

    public bool Is_InSpeed = false;
    public bool Is_BossStage = false;

    public Transform BackGround;

    public List<Tilemap> Trees = new List<Tilemap>();
    public List<Grid> Tile = new List<Grid>();

    public MeshRenderer groundRenderer;

    public void Init()
    {
        if ((int)GameManager.Instance.stage % 10 == 9)
            Is_BossStage = true;

        if (!Is_BossStage)
            BSpeed = (10000 / Tile[0].transform.Find("Ground").GetComponent<TilemapCollider2D>().bounds.size.x) * Speed;
        else
        {
            BSpeed = 50;
            groundRenderer = transform.Find("Gound Quad").GetComponent<MeshRenderer>();
        }
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGamePlay)
            return;
        if (Is_BossStage)
        {
            Offset += Time.deltaTime * (Speed / 1000);
            BOffset += Time.deltaTime * BSpeed;

            groundRenderer.material.SetTextureOffset("_MainTex", new Vector2(Offset, 0));
            BackGround.position = new Vector3(-1 * BOffset, 0);
        }
        else
        {
            Offset += Time.deltaTime * (Speed * 1.5f);
            BOffset += Time.deltaTime * BSpeed;

            BackGround.position = new Vector3(-1 * BOffset, 0);

            foreach (var grid in Tile)
                grid.transform.position = new Vector3(-1 * Offset, 0);

            int i = 0;
            foreach (var tree in Trees)
            {
                TreeOffset[i] += Time.deltaTime * (Speed * (1.5f - (Diff * i)));
                tree.transform.position = new Vector3(-1 * TreeOffset[i], 0);
                i++;
            }
        }
    }

    public void In_Speed(float speed, bool DOKill = false)
    {
        if (DOKill)
        {
            DOTween.Kill("MoveTW");
            Is_InSpeed = false;
        }

        Speed = speed;
        if (!Is_BossStage)
            BSpeed = (10000 / Tile[0].transform.Find("Ground").GetComponent<TilemapCollider2D>().bounds.size.x) * Speed;
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
