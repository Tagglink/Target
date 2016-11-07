using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class loadoutManager : MonoBehaviour {

    public void StartLevel(string scene)
    {
        SceneManager.LoadScene(scene);
    }
	
}
