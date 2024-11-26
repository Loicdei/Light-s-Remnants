using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
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
