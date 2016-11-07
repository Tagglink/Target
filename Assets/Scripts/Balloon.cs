using UnityEngine;
using System.Collections;

public class Balloon : MonoBehaviour {

    public GameObject player;
    public ParticleSystem particles;

    void Pop()
    {
            Instantiate(particles, transform.position, transform.rotation);
            Destroy(gameObject);
    }
}
