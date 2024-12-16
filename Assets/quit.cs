using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quit : MonoBehaviour
{

    // Quits the player when the user hits escape


    void Update()
    {
#if UNITY_EDITOR
        KeyCode quitKey = KeyCode.J;
#else
        KeyCode quitKey = KeyCode.Escape;
#endif

        if (Input.GetKeyDown(quitKey))
        {
            Application.Quit();

            Debug.Log("Quit Game.");
        }
    }
}

