using UnityEngine;
using System.Collections;

public class Hitscan : MonoBehaviour {
    
    private Camera Camera;
    private float offSetDistance = -0.01f;
    public GameObject bulletHole;

    void Start ()
    {
        Camera = Camera.main;
    }

    void Activate()
    {
        Ray ray_camera = Camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray_camera, out hit, 1000)) {
            if (hit.collider.gameObject.tag == "Map")
            {
                var hitRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                GameObject newBulletHole = Instantiate(bulletHole, hit.point, hitRotation) as GameObject;
                newBulletHole.transform.Rotate(new Vector3(90, 0, 0));
                
                newBulletHole.transform.position = Vector3.MoveTowards(newBulletHole.transform.position, newBulletHole.transform.forward, offSetDistance);
            }
            else if (hit.collider.gameObject.tag == "Balloon")
            {
                hit.collider.gameObject.SendMessage("Pop");
            }
        }
    }

}
