using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionScript : MonoBehaviour
{
    public void tekrarYŁkle()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
