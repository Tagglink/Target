using UnityEngine;
using System.Collections;

public class MovementControl : MonoBehaviour {

    public int power;
    private Collider disabledEdge;

    void OnTriggerStay(Collider col)
    {
        float angle = 90;
        if (col != disabledEdge && col.gameObject.tag == "Edge" && Input.GetKeyDown(KeyCode.Space) && Vector3.Angle(transform.forward, col.gameObject.transform.forward) < angle)
        {
            EdgeClimb(col.gameObject);
            StartCoroutine(DisableEdge(col));
        }
    }

    IEnumerator DisableEdge(Collider col)
    {
        disabledEdge = col;
        Debug.Log("Disabled" + disabledEdge);
        yield return new WaitForSeconds(0.25f);
        if (disabledEdge == col)
            disabledEdge = null;
    }

    void EdgeClimb(GameObject Edge)
    {
        Debug.Log("Working as intended");
        gameObject.GetComponent<Rigidbody>().AddForce(transform.up * power);
    }
}
