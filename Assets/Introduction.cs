using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Introduction : MonoBehaviour {

 public void closeIntroduction()
    {
        WaveSpawner.gameStarted = true;
        gameObject.SetActive(false);
    }
}
