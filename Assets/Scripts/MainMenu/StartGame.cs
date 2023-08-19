using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public GameObject creditsTab;
    void Start()
    {
        
    }

    // Update is called once per frame
    public void StartGameMainMenu()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void OpenCreditsTab()
    {
        creditsTab.SetActive(true);
    }
    public void CloseCreditsTab()
    {
        creditsTab.SetActive(false);
    }
    public void OpenDnoldSocials()
    {
        Application.OpenURL("https://twitter.com/DevNold");
    }
}
