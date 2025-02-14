using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private float gameCompleteions;
    private GameObject saves;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //PlayerPrefs.SetFloat("Ends", 1);
        saves = GameObject.FindGameObjectWithTag("Saves");
        LoadSaves();
        DrawSaves();
    }

    private void DrawSaves()
    {
        saves.GetComponent<TMP_Text>().text = gameCompleteions.ToString();
    }

    private void LoadSaves()
    {
        gameCompleteions = 5- PlayerPrefs.GetFloat("GameEnds", 0);
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void ExitGame()
    {
        Debug.Log("Game quit");
        Application.Quit();
    }
}
