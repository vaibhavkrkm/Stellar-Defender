using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator transition;


    public void LoadScene(string sceneName)
    {
        transition.SetTrigger("StartCrossfade");
        StartCoroutine(LoadSceneAfterTransition(sceneName));
    }

    private IEnumerator LoadSceneAfterTransition(string sceneName)
    {
        print("loading scene");
        yield return new WaitForSeconds(0.15f);
        print("scene loaded");
        SceneManager.LoadScene(sceneName);
        yield break;
    }
}
