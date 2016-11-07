using UnityEngine;
using System.Collections;

public class Equipment : MonoBehaviour {

    public GameObject Primary;
    public GameObject Secondary;
    public GameObject Legs;
    public GameObject Back;

	void Start () {
        Primary = Instantiate(Primary, transform) as GameObject;
        Secondary = Instantiate(Secondary, transform) as GameObject;
        Legs = Instantiate(Legs, transform) as GameObject;
        //Instantiate(Back, transform);
    }
	
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Primary.SendMessage("Activate");
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Secondary.SendMessage("Activate");
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Legs.SendMessage("Activate");
        }
	}
}
