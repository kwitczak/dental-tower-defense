using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour {

    private Rigidbody rb;
    private Transform target;
    private int wavepointIndex = 0;
    private Enemy enemy;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        target = Waypoints.points[0];
        rb = GetComponent<Rigidbody>();
        InvokeRepeating("Jump", 1f, 1f);
    }

    void Jump()
    {
        rb.AddForce(new Vector3(0, 8, 0), ForceMode.Impulse);
    }

    void Update()
    {
        Vector3 dir = new Vector3(
            target.position.x - transform.position.x,
            0f,
            target.position.z - transform.position.z
            );
        //Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 2f)
        {
            GetNextWaypoint();
        }

        enemy.speed = enemy.startSpeed;

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

    void EndPath()
    {
        PlayerStats.Lives--;
        Destroy(gameObject);
    }
}
