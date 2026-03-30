using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    //Stats
    [SerializeField] Vector3 bulletSpread = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private float shootDelay = 0.5f;
    [SerializeField] private float bulletSpeed = 100.0f;
    [SerializeField] private int bulletStrength = 1;

    //Common
    [SerializeField] private Transform firePoint;
	[SerializeField] private AudioSource gunSound;
    [SerializeField] private TrailRenderer bulletTrail;
    [SerializeField] private ParticleSystem shootingSystem;
    [SerializeField] private ParticleSystem impactSystem;
    [SerializeField] private LayerMask maskTarget;
    [SerializeField] private LayerMask maskDefault;

    private float lastShootTime = 0.0f;
    public void Shoot()
    {
        if(lastShootTime + shootDelay < Time.time)
        {
            shootingSystem.Play();
            Vector3 direction = GetDirection();
			
			gunSound.Play();

            if (Physics.Raycast(firePoint.position, direction, out RaycastHit hitTarget, float.MaxValue, maskTarget))
            {
				Debug.Log("DianaAcertada");
                TrailRenderer trail = Instantiate(bulletTrail, firePoint.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, hitTarget.point, hitTarget.normal, true, false));

                hitTarget.transform.gameObject.GetComponent<TargetScript>().HitTaken(bulletStrength);

                lastShootTime = Time.time;
            }
            else if(Physics.Raycast(firePoint.position, direction, out RaycastHit hitDefault, float.MaxValue, maskDefault))
            {
                TrailRenderer trail = Instantiate(bulletTrail, firePoint.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, hitDefault.point, hitDefault.normal, true, true));

                lastShootTime = Time.time;
            }
            else
            {
                TrailRenderer trail = Instantiate(bulletTrail, firePoint.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, firePoint.position + direction * 100.0f, Vector3.zero, false, false));

                lastShootTime = Time.time;
            }
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = -transform.forward;

        direction += new Vector3(
            Random.Range(-bulletSpread.x, bulletSpread.x),
            Random.Range(-bulletSpread.y, bulletSpread.y),
            Random.Range(-bulletSpread.z, bulletSpread.z));

        direction.Normalize();

        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hitPoint, Vector3 hitNormal, bool madeImpact, bool particles)
    {
        Vector3 startPos = trail.transform.position;
        float distance = Vector3.Distance(trail.transform.position, hitPoint);
        float distanceLeft = distance;

        while (distanceLeft > 0)
        {
            trail.transform.position = Vector3.Lerp(startPos, hitPoint, 1 - (distanceLeft / distance));

            distanceLeft -= bulletSpeed * Time.deltaTime;

            yield return null;
        }

        trail.transform.position = hitPoint;

        if (madeImpact && particles)
        {
            Instantiate(impactSystem, hitPoint, Quaternion.LookRotation(hitNormal));
        }

        Destroy(trail.gameObject, trail.time);
    }
}
