using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public BallController ballcontroller;

    // Start is called before the first frame update
    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
