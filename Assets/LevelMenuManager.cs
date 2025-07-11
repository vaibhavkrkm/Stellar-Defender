using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenuManager : MonoBehaviour
{
    public SceneLoader sceneLoader;
    private GameObject[] stageButtons;

    private void Start()
    {
        Time.timeScale = 1;

        // playing the menu music (if not already playing)
        MainMusicScript mainMenuMusic = MainMusicScript.Instance;
        if (mainMenuMusic != null && !mainMenuMusic.GetComponent<AudioSource>().isPlaying)
        {
            mainMenuMusic.GetComponent<AudioSource>().Play();
        }

        stageButtons = GameObject.FindGameObjectsWithTag("StageButton");

        foreach (GameObject stageButton in stageButtons)
        {
            int levelNo = int.Parse(stageButton.name.Substring("Stage".Length));
            if (levelNo <= Global.levelsUnlockedTill)
            {
                stageButton.GetComponent<Button>().interactable = true;
                stageButton.GetComponentInChildren<Text>().text = levelNo.ToString();
            }
            else
            {
                stageButton.GetComponent<Button>().interactable = false;
                stageButton.GetComponentInChildren<Text>().text = "_";
            }
        }
    }

    public void BackButtonClicked()
    {
        sceneLoader.LoadScene("MainMenu");
    }

    public void LevelButtonClicked(int level)
    {
        Global.selectedLevel = level;
        sceneLoader.LoadScene("Level");
    }
}
