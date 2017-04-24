using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

    private void Update() {
        if (Input.anyKeyDown) {
            OnClick();
        }
    }

    public void OnClick() {
        //Start the game
        StartCoroutine(LoadNewScene(1));
    }

    public static IEnumerator LoadNewScene(int scene = 1) {
        int SceneToLoad = 1;

        //yield return new WaitForSeconds( 3 );
        AsyncOperation async;
        async = SceneManager.LoadSceneAsync(SceneToLoad);

        while (!async.isDone) {
            Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            SceneManager.SetActiveScene(newlyLoadedScene);
            Debug.Log(string.Format("Done Loading Scene: {0} ID {1}", newlyLoadedScene.name, SceneToLoad));
            yield return null;
        }
    }

}
