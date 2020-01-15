using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float fireRate = 5f;
    public float bulletForce = 7f;
    private float _nextTimeToFire = 0f;

    void Update(){
        if(Input.GetKeyDown(KeyCode.Return) && Time.time >= _nextTimeToFire){
            _nextTimeToFire = Time.time + 1f/fireRate;
            Shoot();
        }
    }

    void Shoot(){
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
    }
}
