using UnityEngine;
using System.Collections;

public class ProjectileGun : MonoBehaviour {
    
    public GameObject projectile;
    public Camera Camera;
    public int Speed = 0;
    private GameObject shotProjectile;

    void Start()
    {
        Camera = Camera.main;
    }

	void Activate ()
    {
        Ray ray = Camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        GameObject shotProjectile = Instantiate(projectile, Camera.transform.position, Camera.transform.rotation) as GameObject;
        shotProjectile.GetComponent<Rigidbody>().AddForce(ray.direction * Speed);
        shotProjectile.transform.Rotate(90, 0, 0);
        Physics.IgnoreCollision(shotProjectile.GetComponent<Collider>(), Camera.GetComponentInParent<Collider>());
    }
}
