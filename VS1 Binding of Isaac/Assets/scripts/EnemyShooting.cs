using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    private float fire;
    public Transform firePoint;
    private Vector2 lastPos;
    private Vector2 curPos;
    private Vector2 playerPos;
    public GameObject bulletPrefab;
    public float bulletForce = 7f;
    private float nextTimeToFire = 0f;
    public float fireRate = 5f;

    
    void Start(){
       
    }
    void Update()
    {
        if(GameObject.FindWithTag(Constants.Tags.PLAYER) != null){
            playerPos = GameObject.FindWithTag(Constants.Tags.PLAYER).transform.position;
        }

       fire = Random.Range(1f, 100f);
       if(fire >= 99.7){
           Shoot();
       }
    }

    void Shoot(){
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Isenemy = true;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        
    }

}
