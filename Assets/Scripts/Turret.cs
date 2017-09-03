using System;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    private Transform target;
    private Enemy targetEnemy;

    [Header("General")]
    public String enemyType;
    public String upgradeText;
    public float typeDamageReduce = 0.4f;
    public float range = 15f;

    [Header("Use Bullets (default)")]
    public GameObject bulletPrefab;
    public int bulletDamage = 5;
    private int baseDamage;
    public float fireRate = 1f;
    private float startFireRate;
    private float fireCountdown = 0f;

    [Header("Use Laser")]
    public bool useLaser = false;
    public int damageOverTime = 30;
    public float slowAmount = .5f;
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public string roadTag = "Road";

    public Transform partToRotate;
    public Transform partToAnimate;
    private Quaternion startingRotation;
    public float turnSpeed = 10f;

    public Transform firePoint;
    private Animator shotAnimation;

    public GameObject emotionAura;
    private float minEmotionEffectOnSpeed = 0.3f;
    private bool emotionAffected = false;

    public int towerLevel = 1;
    public Text towerLevelText;
    public Node node;

    private AudioSource attackSound;

    // Use this for initialization
    void Start()
    {
        attackSound = GetComponent<AudioSource>();
        baseDamage = useLaser ? damageOverTime: bulletDamage;
        startFireRate = fireRate;
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        if (partToAnimate != null)
        {
            shotAnimation = partToAnimate.GetComponent<Animator>();
            startingRotation = partToAnimate.rotation;
        }

    }

    // Find closest enemy and target it
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
        }
        else
        {
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {

        EmotionManager.applyTurretAura(this);

        if (target == null)
        {
            if (useLaser)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    impactEffect.Stop();
                    impactLight.enabled = false;
                }

            }

            toggleShootingAnimation(false);
            return;
        }


        LockOnTarget();

        if (useLaser)
        {
            Laser();
        }
        else
        {
            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }


    }

    void LockOnTarget()
    {
        toggleShootingAnimation(true);
        
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Laser()
    {
        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        targetEnemy.Slow(slowAmount);

        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }


        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - target.position;

        // Normalized vector -> reduced to 1
        impactEffect.transform.position = target.position + dir.normalized;
        // Point towards the turret
        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }

    void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if (!attackSound.isPlaying)
        {
            attackSound.Play();
        }

        if (bullet != null)
            bullet.Seek(target, this);
    }

    // Draw turret range on selection
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    private void toggleShootingAnimation(bool activate)
    {
        if (shotAnimation != null)
        {
            shotAnimation.enabled = activate;

            if (activate)
            {
                return;
            }

            // Go back to previous position slowly
            Vector3 rotation = Quaternion.Lerp(partToAnimate.rotation,
                startingRotation,
                Time.deltaTime * turnSpeed).eulerAngles;

            partToAnimate.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
            partToAnimate.localScale = new Vector3(1, 1, 1);
        }
    }

    public void applyStressorReaction(int certainty)
    {
        if (emotionAffected)
        {
            return;
        }

        float slowFireAmount = fireRate * (certainty / 500) + minEmotionEffectOnSpeed * fireRate;
        fireRate = fireRate - slowFireAmount;

        emotionAffected = true;
    }

    public void cleanUpReaction()
    {
        if (!emotionAffected)
        {
            return;
        }

        fireRate = startFireRate;
        emotionAffected = false;
    }

    public void applyCalmReaction(int certainty)
    {
        if (emotionAffected)
        {
            return;
        }

        float slowFireAmount = fireRate * (certainty / 500) + minEmotionEffectOnSpeed * fireRate;
        fireRate = fireRate + slowFireAmount;

        emotionAffected = true;
    }

    public int nextDamageUpdate()
    {
        return Convert.ToInt32(Math.Round(baseDamage * 0.2));
    }

    public int nextSpeedUpdate()
    {
        return Convert.ToInt32(Math.Round(startFireRate * 0.2));
    }

    public void simpleUpgrade()
    {
        towerLevel++;
        towerLevelText.text = new String('I', towerLevel);

        if(useLaser)
        {
            damageOverTime += nextDamageUpdate();
        } else
        {
            bulletDamage += nextDamageUpdate();
            fireRate += nextSpeedUpdate();
        }
        
       
    }

    void OnMouseDown()
    {
        node.showStats();
    }

    public int damageToEnemy(String _enemyType)
    {
        if (enemyType.Equals(_enemyType))
        {
            return bulletDamage;
        } else
        {
            return Convert.ToInt32(Math.Round(bulletDamage * typeDamageReduce));
        }
        
    }

    public int showDamage()
    {
        return useLaser ? damageOverTime : bulletDamage;
    }
}
