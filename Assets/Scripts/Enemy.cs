using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    public float startSpeed = 10f;
    public float beforeAffectionSpeed;
    public float minEmotionEffectOnSpeed = 0.3f;
    public float speed;

    public float health;
    public float startHealth = 100;
    public float beforeAffectionHealth;
    public int worth = 50;

    private bool isDead = false;
    private bool emotionAffected = false;

    public GameObject deathEffect;

    [Header("Unity Stuff")]
    public Image healthBar;

    private AudioSource deathSound;

    void Start ()
    {
        speed = startSpeed;
        health = startHealth;
        beforeAffectionHealth = startHealth;
        beforeAffectionSpeed = startSpeed;
        deathSound = SoundManager.getInstance().enemyDeathSound.GetComponent<AudioSource>();
    }

    public void TakeDamage (float amount)
    {
        health -= amount;
        healthBar.fillAmount = health / startHealth;

        if (health <= 0 && !isDead)
        {
            Die();
        }
    }

    public void Slow (float slowAmount)
    {
        speed = startSpeed * (1f - slowAmount);
    }

    void Die ()
    {
        isDead = true;
        PlayerStats.Money += worth;
        PlayerStats.Score += worth * 100;

        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        deathSound.Play();

        WaveSpawner.EnemiesAlive--;
        Destroy(gameObject);
    }

    public void applyStressorReaction(int certainty)
    {
        if (emotionAffected)
        {
            return;
        }

        float speedAmount = speed * (certainty / 100) + minEmotionEffectOnSpeed * speed;
        speed = speed + speedAmount;
        startSpeed = startSpeed + speedAmount;


        float healthAmount = beforeAffectionHealth + beforeAffectionHealth * (certainty / 100);
        health = health + healthAmount;
        startHealth = startHealth + healthAmount;
        emotionAffected = true;
    }

    public void cleanUpReaction()
    {
        speed = startSpeed;
        startHealth = beforeAffectionHealth;
        startSpeed = beforeAffectionSpeed;

        if (health > beforeAffectionHealth)
        {
            health = beforeAffectionHealth;
        }

        if (speed > beforeAffectionSpeed)
        {
            speed = beforeAffectionSpeed;
        }

        emotionAffected = false;
    }

    public void applyCalmReaction(int certainty)
    {
        if (emotionAffected)
        {
            return;
        }

        float speedAmount = speed * (certainty / 100) + minEmotionEffectOnSpeed * speed;
        speed = speed - speedAmount;
        startSpeed = startSpeed - speedAmount;

        float healthAmount = beforeAffectionHealth - beforeAffectionHealth * (certainty / 200);
        startHealth = startHealth - healthAmount;
        emotionAffected = true;
    }
}
