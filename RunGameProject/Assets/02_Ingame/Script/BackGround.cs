using UnityEngine;
using DG.Tweening;
using System.Collections;

public class BackGround : MonoBehaviour
{
    Renderer Renderer;
    public Player Player;

    public float Speed;
    public float Offset;

    public void Start()
    {
        Renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        Offset += Time.deltaTime * Speed;
        Renderer.material.SetTextureOffset("_MainTex", new Vector2(Offset, 0));
    }

    public void Setsp(int sp)
    {
        Speed = sp;
    }

    public void Move()
    {
        Speed = 3;
        DOTween.To(() => Speed, x => Speed = x, 0.8f, 0.3f);
    }
}
