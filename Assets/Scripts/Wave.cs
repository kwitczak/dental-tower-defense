using UnityEngine;

[System.Serializable]
public class Wave {

    public GameObject enemy;
    public GameObject bossEnemy;
    public int count;
    public int waitBeforeStart;
    public float rate;
    public Wave[] nestedWave;
}
