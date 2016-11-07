using UnityEngine;
using System.Collections;

public class Bullethole : MonoBehaviour {
    
    private bool hasHit;
    private float hitTime;
    private float offSetDistance = -0.01f;

    void FixedUpdate()
    {
        if (!hasHit)
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.up, Color.black);
            if (Physics.Raycast(transform.position, transform.up, out hit))
            {
                if (hit.collider.gameObject.tag == "Map" && hit.distance < 0.5f)
                {
                    gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    gameObject.GetComponent<Rigidbody>().Sleep();
                    Destroy(transform.GetComponent<Rigidbody>());
                    Destroy(transform.GetComponent<BoxCollider>());
                    transform.position = hit.point;
                    hasHit = true;

                }
                else if (hit.collider.gameObject.tag == "Balloon" && hit.distance < 0.25f)
                {
                    hit.collider.gameObject.SendMessage("Pop");
                }
            }
        }
    } 
}
