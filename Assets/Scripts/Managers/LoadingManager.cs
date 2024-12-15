using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public Slider loadingSlider;
    //public Text loadingText;

    private AsyncOperation loadingOperation;

    private void Start()
    {
        DataController.Instance.LoadGameData();

        loadingOperation = SceneManager.LoadSceneAsync((int)GameEntries.Scenes.game);
        loadingOperation.allowSceneActivation = false;
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        while (!loadingOperation.isDone)
        {

            float progress = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            loadingSlider.value = progress;

            Debug.Log("Loading Progress: " + progress);

            //if (loadingText != null)
            //{
            //    loadingText.text = $"Loading... {Mathf.RoundToInt(progress * 100)}%";
            //}

            if (loadingOperation.progress >= 0.9f)
            {
                StartGameScene();
                yield break; 
            }

            yield return null;
        }
    }

    public void StartGameScene()
    {
        loadingOperation.allowSceneActivation = true;
    }
}
