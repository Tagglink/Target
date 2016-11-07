using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class Dash : MonoBehaviour {

    public GameObject player;
    public Rigidbody playerRigidBody;

    public float distance;
    public float speed;
    public float cooldown;
    public bool isOnCooldown;

    private float time;

    private Camera mainCamera;
    private Ray ray;
    private Vector3 endPoint;
    private Vector3 startPoint;
    private Vector3 force;
    private float startTime;
    private bool isDashing;

    private RigidbodyFirstPersonController playerController;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<RigidbodyFirstPersonController>();
        mainCamera = Camera.main;
        playerRigidBody = player.GetComponent<Rigidbody>();
    }

    void Update()
    {

    }

    void Activate()
    {
        if (!isDashing && !isOnCooldown)
        {
            //Debug.Log("Dash activated");

            ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

            startPoint = transform.position;
            endPoint = (ray.direction * distance) + startPoint;
            force = ray.direction * speed;
            float velocity = (Vector3.Distance(new Vector3(0, 0, 0), force) / playerRigidBody.mass) * Time.fixedDeltaTime;
            time = distance / velocity;

            /*Debug.Log("Velocity: " + velocity);
            Debug.Log("Time: " + time);
            Debug.Log("Start position: " + startPoint);
            Debug.Log("Force vector: " + force);
            Debug.Log("End vector: " + endPoint);*/

            StartCoroutine(InitiateDash());
            player.GetComponent<Rigidbody>().AddForce(force);
        }
    }

    void PreDashActions()
    {
        playerRigidBody.useGravity = false;
        isDashing = true;
        playerController.OverrideVelocity = true;
    }

    void PostDashActions()
    {
        playerRigidBody.useGravity = true;
        playerRigidBody.velocity = Vector3.zero;
        isDashing = false;
        playerController.OverrideVelocity = false;
        StartCoroutine(InitiateCooldown());
    }

    IEnumerator InitiateDash()
    {
        PreDashActions();
        yield return new WaitForSeconds(time);
        PostDashActions();
    }

    IEnumerator InitiateCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }
}
