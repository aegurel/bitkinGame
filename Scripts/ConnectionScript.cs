using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionScript : MonoBehaviour
{
    public void tekrarY�kle()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
