using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScr : MonoBehaviour
{
    public GameObject m_Player;
    void Start()
    {
        m_Player = GameManager.Instance.playerMG.Player;
        gameObject.SetActive(false);
        InvokeRepeating("CheckRender", 0, Time.deltaTime * 20);
    }

    public void CheckRender()
    {
        Vector2 vMy = transform.position;
        if (vMy.y < -1500)
            Destroy(gameObject);
        else
            vMy.y = 0;

        Vector2 vPlayer = m_Player.transform.position;
        vPlayer.y = 0;

        float Distance = Vector2.Distance(vPlayer, vMy);

        if (vMy.x < vPlayer.x)
        {
            if (Distance > 500f)
                Destroy(gameObject);
            else
                gameObject.SetActive(true);
        }
        else
        {
            if (Distance < 2500f)
                gameObject.SetActive(true);
        }
    }
}
