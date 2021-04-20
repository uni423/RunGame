using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject Prefab;
    public EnemyData Data;

    public void Spawn(int value = 1)
    {
        for(int i = 0; i < value; i++)
        {
            GameObject Obj = Instantiate(Prefab, transform);
            Obj.GetComponent<Enemy>().stat = Data;
        }
    }
}
