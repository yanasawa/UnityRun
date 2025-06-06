using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScript : MonoBehaviour
{
    [SerializeField] 
    private GameObject _loadingUI;

    [SerializeField] 
    private Slider _slider;

    void Start()
    {
        _loadingUI.SetActive(true);
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        if (StartScript.isStart == true)
        {
            AsyncOperation async = SceneManager.LoadSceneAsync("Main");
            while (!async.isDone)
            {
                _slider.value = async.progress;
                yield return null;
            }
        }
        else
        {
            AsyncOperation async = SceneManager.LoadSceneAsync("Start");
            while (!async.isDone)
            {
                _slider.value = async.progress;
                yield return null;
            }
        }
    }
}
