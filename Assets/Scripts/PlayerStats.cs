using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    // Don't want to carry over money from lost game
    public static int Money;
    public int startMoney;

    public static int Lives;
    public int startLives;

    public static int Rounds;

    void Start ()
    {
        Money = startMoney;
        Lives = startLives;

        Rounds = 0;
    }
}
