using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {  get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
            return;
        }
         instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void OpenPanel()
    {

    }

    [SerializeField] SceneAsset scene;
    public void ChangeScene()
    {
        SceneManager.LoadScene(scene.name);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
