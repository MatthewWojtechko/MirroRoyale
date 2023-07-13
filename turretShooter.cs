using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretShooter : MonoBehaviour
{
    public GameObject laser;
    public Type type;

    [Header("Random")]
    public float minTime;
    public float maxTime;

    [Header("Regular")]
    public float waitTime;

    [Header("Generic")]
    public float startWait = 0;
    public bool allowedToShoot = true;
    public GameObject shootEffect;
    public Transform shootPoint;
    public bool isShootTime = true;
    public bool isWaiting;

    // Start is called before the first frame update
    void Start()
    {
        if (startWait > 0)
            StartCoroutine(waitShoot(startWait));
    }

    // Update is called once per frame
    void Update()
    {
        if (allowedToShoot)
        {
            if (isShootTime)
            {
                shoot();
                isShootTime = false;
            }
            else if (!isWaiting)
            {
                StartCoroutine(waitShoot());
            }
        }
    }

    IEnumerator waitShoot()
    {
        isWaiting = true;
        if (type == Type.RANDOM)
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        else
            yield return new WaitForSeconds(waitTime);
        isWaiting = false;
        isShootTime = true;
    }

    IEnumerator waitShoot(float t)
    {
        isWaiting = true;
        yield return new WaitForSeconds(startWait);
        isWaiting = false;
        isShootTime = true;
    }

    void shoot()
    {
        GameObject l = GameObject.Instantiate(laser, shootPoint.position, Quaternion.identity);
        l.transform.up = this.transform.up;
        GameObject.Instantiate(shootEffect, shootPoint.position, Quaternion.identity);
    }

    public enum Type { RANDOM, REGULAR }
}
