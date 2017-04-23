using UnityEngine;

public class Bullet : MonoBehaviour {

    // Target should come from Turret
    private Transform target;
    public float speed = 70f;
    public int damage = 50;

    public float explosionRadius = 0f;
    public GameObject impactEffect;
    
    public void Seek (Transform _target)
    {
        target = _target;
    }

	// Update is called once per frame
	void Update () {
		if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        // Don't overshoot the target
        if(dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        // No orbital behaviours :( - Space.World
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

        // Rotate to face the target
        transform.LookAt(target);
	}

    void HitTarget()
    {
        GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 5f);

        // Hit many or single target
        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target);
        }

        Destroy(gameObject);
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        // For each target that was hit by explosion
        foreach (Collider collider in colliders)
        {
            if(collider.tag == "Enemy")
            {
                Damage(collider.transform);
            }
        }
    }

    void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        if(e != null)
        {
            e.TakeDamage(damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
