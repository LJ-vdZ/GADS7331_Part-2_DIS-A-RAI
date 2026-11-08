using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void StartGame() 
    {
        SceneFade.Load("Gameplay");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneFade.Load("Gameplay");
    }

    public void Home() 
    {
        SceneFade.Load("StartMenu");
    }
}
