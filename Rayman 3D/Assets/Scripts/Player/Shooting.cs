using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public Rigidbody bulletRB;
    public KeyCode chargeShot;
    private float _chargeShotTimer = 0;

    public float fireRate = 3f;
    public float bulletForce = 7f;
    private float _nextTimeToFire = 0f;

    void Update(){
        if(Input.GetKeyDown(chargeShot) && Time.time >= _nextTimeToFire){
            _nextTimeToFire = Time.time + 1f/fireRate;
            _chargeShotTimer += Time.deltaTime;
        }

        if(Input.GetKeyUp(chargeShot)){
            
        }

    }

    void Shoot(){
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
    }
}
