using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionScript : MonoBehaviour
{
    public void tekrarYükle()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
