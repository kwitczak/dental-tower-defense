using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour {

    public static int EnemiesAlive = 0;
    public static int WavesLeft = 0;
    public static bool gameStarted = false;
    public Wave[] waves;

    public Transform spawnPoint;
    public float timeBetweenWaves;
    public Text waveCountdownText;
    public Text waveCounterText;

    private float countdown;
    private int waveIndex = 0;

    public GameManager gameManager;
    public EmotionManager emotionManager;

    private void Start()
    {
        countdown = timeBetweenWaves;
        updateWavesText();
    }

    void Update ()
    {
        if (!gameStarted)
        {
            return;
        }

        if (EnemiesAlive > 0 || WavesLeft > 0)
        {
            return;
        }

        if (waveIndex == waves.Length)
        {
            gameManager.WinLevel();
            this.enabled = false;
        }

        if (countdown <= 0f)
        {
            Debug.Log("Checking affective reaction...");
            emotionManager.runAffectiveReaction(waveIndex, (waveIndex + 1) == waves.Length);

            List<Wave> nestedWaves = flattenCurrentWave(waves[waveIndex]);
            foreach (Wave wave in nestedWaves)
            {
                StartCoroutine(SpawnWave(wave, true));
            }

            waveIndex++;
            updateWavesText();
            PlayerStats.Rounds++;
            countdown = timeBetweenWaves;
            return;
        }

        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

        waveCountdownText.text = string.Format("{0:00.00}", countdown);
    }

    List<Wave> flattenCurrentWave(Wave wave)
    {
        List<Wave> waves = new List<Wave>();

        do
        {
            EnemiesAlive += wave.count;
            if (wave.bossEnemy != null)
            {
                EnemiesAlive++;
            }

            WavesLeft++;
            waves.Add(wave);
            if (wave.nestedWave.Length > 0)
            {
                wave = wave.nestedWave[0];
            } else
            {
                break;
            }
            
        } while (wave != null);

        return waves;
    }

    IEnumerator SpawnWave(Wave wave, bool incrementWave)
    {
        yield return new WaitForSeconds(wave.waitBeforeStart);
        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            SoundManager.getInstance().spawn.GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(1f/wave.rate);
        }

        if (wave.bossEnemy != null)
        {
            SpawnEnemy(wave.bossEnemy);
        }

        WavesLeft--;
    }

    void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
        EmotionManager.applyAura(enemy.GetComponent<Enemy>());
    }

    void updateWavesText()
    {
        waveCounterText.text = waveIndex + " / " + waves.Length;
    }
}
