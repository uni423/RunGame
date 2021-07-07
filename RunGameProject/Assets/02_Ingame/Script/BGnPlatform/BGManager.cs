using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;

[System.Serializable]
public class Level
{
    public List<GameObject> Phaeton = new List<GameObject>();
}

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

    public GameObject GameClear;

    [SerializeField]
    public List<Level> LevelPhaeton = new List<Level>();

    public GameObject PastPhaeton;
    public GameObject NowPhaeton;
    public GameObject FuturePhaeton;

    public int minLevel = 0;
    public int maxLevel = 1;

    public int maxDistance = 0;

    public void Init()
    {
        if ((int)GameManager.Instance.stage % 10 == 9)
            Is_BossStage = true;

        if (!Is_BossStage)
        {
            minLevel = ((int)GameManager.Instance.stage % 10);
            maxLevel = ((int)GameManager.Instance.stage % 10) + 2;

            switch ((int)GameManager.Instance.stage / 10)
            {
                case 0: maxDistance = 20000; break;
                case 1: maxDistance = 12500; break;
                case 2: maxDistance = 15000; break;
            }
            PastPhaeton = Instantiate(LevelPhaeton[0].Phaeton[0], Tile[0].gameObject.transform);
            PastPhaeton.transform.position = new Vector3(0, -30);
            PastPhaeton.SetActive(true);

            NowPhaeton = Instantiate(LevelPhaeton[0].Phaeton[0], Tile[0].gameObject.transform);
            NowPhaeton.transform.position = new Vector3(1280, -30);
            NowPhaeton.SetActive(true);

            FuturePhaeton = Instantiate(LevelPhaeton[0].Phaeton[0], Tile[0].gameObject.transform);
            FuturePhaeton.transform.position = new Vector3(2560, -30);
            FuturePhaeton.SetActive(true);
            GameClear.SetActive(false);
        }
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

            if (PastPhaeton.transform.position.x < -1270)
                GroundPhaetonUpdate();
        }
    }

    public void GroundPhaetonUpdate()
    {
        if (maxDistance > 0)
        {
            int level = 0;
            int phaeton = 0;
            do
            {
                level = Random.Range(minLevel, maxLevel + 1);
                phaeton = Random.Range(0, LevelPhaeton[level].Phaeton.Count);
            }
            while ((FuturePhaeton.name == LevelPhaeton[level].Phaeton[phaeton].name));

            Destroy(PastPhaeton);

            PastPhaeton = NowPhaeton;
            NowPhaeton = FuturePhaeton;

            FuturePhaeton = Instantiate(LevelPhaeton[level].Phaeton[phaeton], Tile[0].gameObject.transform);
            FuturePhaeton.transform.position = new Vector3(NowPhaeton.transform.position.x + 1280, -30);
            FuturePhaeton.SetActive(true);

            maxDistance -= 1280;
        }
        else if (!GameClear.activeSelf)
        {
            GameObject goalin = Instantiate(LevelPhaeton[0].Phaeton[0], Tile[0].gameObject.transform);
            goalin.transform.position = new Vector3(2560, -30);
            goalin.SetActive(true); 
            goalin = Instantiate(LevelPhaeton[0].Phaeton[0], Tile[0].gameObject.transform);
            goalin.transform.position = new Vector3(3840, -30);
            goalin.SetActive(true);

            GameClear.transform.position = new Vector3(2560, -30);
            GameClear.SetActive(true);
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
            BSpeed = ((maxDistance + 500f) / 100);
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
