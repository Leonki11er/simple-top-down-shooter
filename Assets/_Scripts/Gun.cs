using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _shootingPSystem;
    [SerializeField]
    private Transform _bulletSpawnPoint;
    [SerializeField]
    private ParticleSystem _impactPSystem;
    [SerializeField]
    private TrailRenderer _bulletTrail;
    [SerializeField]
    private float _shootDelay = 0.2f;
    [SerializeField]
    private float _speed = 50f;
    [SerializeField]
    private LayerMask _mask;
    [SerializeField]
    private bool _bouncingBullets;
    [SerializeField]
    private float _bounceDistance = 5f;

    private float _lastShootTime;

    public void Shoot()
    {
        if(_lastShootTime + _shootDelay < Time.time)
        {
            _shootingPSystem.Play();

            Vector3 direction = transform.up;
            TrailRenderer trail = Instantiate(_bulletTrail, _bulletSpawnPoint.position, Quaternion.identity);
            if(Physics.Raycast(_bulletSpawnPoint.position,direction, out RaycastHit hit, float.MaxValue, _mask))
            {
                StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, _bounceDistance, true));
            }
            else
            {
                StartCoroutine(SpawnTrail(trail, direction * 50, Vector3.zero, _bounceDistance, false));
            }
            _lastShootTime = Time.time;
        }
    }
    
    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 hitPoint, Vector3 hitNormal, float bounceDistance, bool madeImpact)
    {
        Vector3 startPosition = trail.transform.position;
        Vector3 direction = (hitPoint - trail.transform.position).normalized;

        float distance = Vector3.Distance(trail.transform.position, hitPoint);
        float startingDistance = distance;

        while (distance > 0)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hitPoint, 1 - (distance / startingDistance));
            distance -= Time.deltaTime * _speed;
            yield return null;
        }

        trail.transform.position = hitPoint;
        if (madeImpact)
        {
            Instantiate(_impactPSystem, hitPoint, Quaternion.LookRotation(hitNormal));

            if(_bouncingBullets && _bounceDistance > 0)
            {
                Vector3 bounceDirection = Vector3.Reflect(direction, hitNormal);

                if(Physics.Raycast(hitPoint, bounceDirection, out RaycastHit hit, bounceDistance, _mask))
                {
                    yield return StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, bounceDistance - Vector3.Distance(hit.point, hitPoint), true));
                }
                else
                {
                    yield return StartCoroutine(SpawnTrail(trail, bounceDirection*bounceDistance, Vector3.zero, 0, false));
                }
            }
        }

        Destroy(trail.gameObject, trail.time);
    }
}
