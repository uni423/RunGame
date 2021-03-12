using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager02 : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(TimeStop());
    }

    IEnumerator TimeStop()
    {
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0;
    }
}
