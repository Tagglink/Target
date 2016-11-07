using UnityEngine;
using System.Collections;

public class RepeatTexture : MonoBehaviour {
    
	void Start () {
        GetComponent<Renderer>().material.mainTextureScale = new Vector2(100, 100);
    }
}
