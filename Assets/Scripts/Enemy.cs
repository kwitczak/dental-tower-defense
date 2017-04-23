using UnityEngine;

public class Enemy : MonoBehaviour {

    public float speed = 10f;

    public int health = 100;
    public int value = 50;

    public GameObject deathEffect;

    private Transform target;
    private int wavepointIndex = 0;
    private Rigidbody rb;

    void Start ()
    {
        target = Waypoints.points[0];
        rb = GetComponent<Rigidbody>();
        InvokeRepeating("Jump", 1f, 1f);
    }

    public void TakeDamage (int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die ()
    {
        PlayerStats.Money += value;

        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);

        Destroy(gameObject);
    }

    void Update ()
    {
        Vector3 dir = new Vector3(
            target.position.x - transform.position.x,
            0f,
            target.position.z - transform.position.z
            );
        //Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 2f)
        {
            GetNextWaypoint();
        }

    } 

    void GetNextWaypoint()
    {
        if (wavepointIndex >= Waypoints.points.Length - 1)
        {
            EndPath();
            return;
        }

        wavepointIndex++;
        target = Waypoints.points[wavepointIndex];
    }

    void Jump()
    {
        rb.AddForce(new Vector3(0, 8, 0), ForceMode.Impulse);
    }

    void EndPath ()
    {
        PlayerStats.Lives--;
        Destroy(gameObject);
    }
}
