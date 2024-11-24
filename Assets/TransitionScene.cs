using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionScene : MonoBehaviour
{


    void Update()
    {
        if (Input.anyKey )
        {
            LoadNextScene();
        }
    }

    public void  LoadNextScene()
    {

        // Load the next scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
    }

}
