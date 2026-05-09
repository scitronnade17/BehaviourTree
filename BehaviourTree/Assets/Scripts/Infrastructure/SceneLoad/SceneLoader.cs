using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface ISceneLoader
{
    void LoadScene(int sceneIndex, Action OnLoaded = null);
}

public class SceneLoader : ISceneLoader
{
    private readonly ICoroutineRunner runner;

    public SceneLoader(ICoroutineRunner _runner)
    {
        runner = _runner;
    }

    public void LoadScene(int sceneIndex, Action OnLoaded = null)
    {
        runner.StartCoroutine(Load(sceneIndex, OnLoaded));
    }

    private IEnumerator Load(int sceneIndex, Action onLoaded = null)
    {
        AsyncOperation waitScene = SceneManager.LoadSceneAsync(sceneIndex);

        while (!waitScene.isDone)
            yield return null;

        onLoaded?.Invoke();
    }
}
