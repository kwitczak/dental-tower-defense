using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    // Don't want to carry over money from lost game
    public static int Money;
    public int startMoney;

    void Start ()
    {
        Money = startMoney;
    }
}
