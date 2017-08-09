using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour {

    public static int EnemiesAlive = 0;
    public static bool gameStarted = false;
    public Wave[] waves;

    public Transform spawnPoint;
    public float timeBetweenWaves;
    public Text waveCountdownText;

    private float countdown;
    private int waveIndex = 0;

    public GameManager gameManager;
    public EmotionManager emotionManager;

    private void Start()
    {
        countdown = timeBetweenWaves;
    }

    void Update ()
    {
        if (!gameStarted)
        {
            return;
        }

        if (EnemiesAlive > 0)
        {
            return;
        }

        if (waveIndex == waves.Length)
        {
            Debug.Log("MEH?");
            gameManager.WinLevel();
            this.enabled = false;
        }

        if (countdown <= 0f)
        {
            Debug.Log("Checking affective reaction...");
            emotionManager.runAffectiveReaction(waveIndex, (waveIndex + 1) == waves.Length);
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }

        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

        waveCountdownText.text = string.Format("{0:00.00}", countdown);
    }

    IEnumerator SpawnWave()
    {
        PlayerStats.Rounds++;

        Wave wave = waves[waveIndex];
        EnemiesAlive = wave.count;

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1f/wave.rate);
        }

        waveIndex++;
    }

    void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        EmotionManager.applyAura(enemy.GetComponent<Enemy>());
    }
}
