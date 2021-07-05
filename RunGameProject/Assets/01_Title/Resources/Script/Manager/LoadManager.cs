using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    private class LoadingMonoBehaviour : MonoBehaviour { }

    public enum Scene
    {
        Title,
        Ingame,
        Story,
        Boss,
        Load
    }

    static Action onLoaderCallback;
    static AsyncOperation asyncOperation;

    // 1. 아무씬에서나 사용하면됨 원래의 씬 매니저에 있는 로드 함수 쓰는거처럼 씀됨
    public static void Load(Scene scene)
    {
        onLoaderCallback = () =>
        {
            GameObject loading = new GameObject("Loading Game Object");
            loading.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
        };

        SceneManager.LoadScene("00_Load");
    }

    // 3.
    private static IEnumerator LoadSceneAsync(Scene scene)
    {
        switch(scene)
        {
            case Scene.Title :
                asyncOperation = SceneManager.LoadSceneAsync("01_TItle");
                break;
            case Scene.Ingame :
                asyncOperation = SceneManager.LoadSceneAsync("02_Ingame");
                break;
            case Scene.Boss :
                asyncOperation = SceneManager.LoadSceneAsync("02_Ingame_1-Boss");
                break;
        }

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }

    // 그냥 필요할때 알잘딱깔
    public static float GetLoadingProgress()
    {
        if (asyncOperation != null)
            return asyncOperation.progress;
        else
            return 1f;
    }

    // 2.
    public static void LoaderCallback()
    {
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }
}