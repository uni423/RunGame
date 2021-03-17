using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Define;

public class CharDatas : MonoBehaviour
{
    [SerializeField]
    private List<CharStat> stats = new List<CharStat>();

    public CharStat GetStat(CharType type)
    {
        return stats[(int)type];
    }
}