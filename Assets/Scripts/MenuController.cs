using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    private GameManager manager;

    private void Start()
    {
        manager = GameManager.instance;
    }

    public void ChangeScene() 
    { 
        manager.ChangeScene();
    }

    public void Quit()
    {
        manager.Quit();
    }

}
