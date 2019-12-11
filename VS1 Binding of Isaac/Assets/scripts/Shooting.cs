using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bulletPrefab;
    
    public float fireRate = 5f;
    public float bulletForce = 7f;
    private float nextTimeToFire = 0f;
    void Update(){
        if (Input.GetKeyDown(KeyCode.UpArrow) && Time.time >= nextTimeToFire){
            nextTimeToFire = Time.time + 1f/fireRate;
            ShootUp();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && Time.time >= nextTimeToFire){
            nextTimeToFire = Time.time + 1f/fireRate;
            ShootDown();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && Time.time >= nextTimeToFire){
            nextTimeToFire = Time.time + 1f/fireRate;
            ShootLeft();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && Time.time >= nextTimeToFire){
            nextTimeToFire = Time.time + 1f/fireRate;
            ShootRight();
        }
    }

    void ShootUp() {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }

    void ShootDown() {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(-firePoint.up * bulletForce, ForceMode2D.Impulse);
    }

    void ShootRight() {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
    }

    void ShootLeft() {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(-firePoint.right * bulletForce, ForceMode2D.Impulse);
    }
}
