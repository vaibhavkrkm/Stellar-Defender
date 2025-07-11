using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenuManager : MonoBehaviour
{
    public GameObject optionsUI;
    public GameObject storeUI;
    public GameObject storyUI;
    public GameObject howToPlayUI;
    public TMP_Text coinsText;
    public SceneLoader sceneLoader;

    public TMP_Text healthCoinsText;
    public TMP_Text damageCoinsText;
    public TMP_Text shieldCoinsText;
    public TMP_Text magnetCoinsText;

    public Text healthLevelText;
    public Text damageLevelText;
    public Text shieldLevelText;
    public Text magnetLevelText;

    public Button healthUpgradeButton;
    public Button damageUpgradeButton;
    public Button shieldUpgradeButton;
    public Button magnetUpgradeButton;

    public GameObject notEnoughCoinsText;

    private int healthLevel;
    private int damageLevel;
    private int shieldLevel;
    private int magnetLevel;

    // sounds
    public AudioSource clickedSound;
    public AudioSource upgradeSound;
    public AudioSource notEnoughCoinsSound;

    public AudioMixer mainMixer;

    private Coroutine notEnoughCoinsCoroutine = null;

    private void Awake()
    {
        // load player progress
        PlayerData playerData = SaveSystem.LoadPlayerData();
        if (playerData != null)
        {
            Global.levelsUnlockedTill = playerData.levelsUnlockedTill;
            Global.total_coins = playerData.total_coins;
            Global.currentPlayerLevels = playerData.currentPlayerLevels;
            Global.newGame = playerData.newGame;
        }
        else
        {
            Debug.Log("Unable to load data, is this your first time playing? If yes, this is normal.");
        }
    }

    private void Start()
    {
        // playing the menu music (if not already playing)
        MainMusicScript mainMenuMusic = MainMusicScript.Instance;
        if (mainMenuMusic != null && !mainMenuMusic.GetComponent<AudioSource>().isPlaying)
        {
            mainMenuMusic.GetComponent<AudioSource>().Play();
        }

        if (Global.newGame)
        {
            storyUI.SetActive(true);
        }
        RefreshUpgradesUI();
    }

    public void SetVolume(float volume)
    {
        float volumeDB = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20;
        mainMixer.SetFloat("volume", volumeDB);
    }

    public void RefreshUpgradesUI()
    {
        // trying to get upgrade levels for player, if not found, setting it to 1 as default
        if (!Global.currentPlayerLevels.TryGetValue("health", out healthLevel))
        {
            healthLevel = 1;
            Global.currentPlayerLevels.Add("health", healthLevel);
        }
        if (!Global.currentPlayerLevels.TryGetValue("damage", out damageLevel))
        {
            damageLevel = 1;
            Global.currentPlayerLevels.Add("damage", damageLevel);
        }
        if (!Global.currentPlayerLevels.TryGetValue("shield", out shieldLevel))
        {
            shieldLevel = 1;
            Global.currentPlayerLevels.Add("shield", shieldLevel);
        }
        if (!Global.currentPlayerLevels.TryGetValue("magnet", out magnetLevel))
        {
            magnetLevel = 1;
            Global.currentPlayerLevels.Add("magnet", magnetLevel);
        }

        coinsText.text = Global.total_coins.ToString();

        // updating UI texts for different upgrades
        if (healthLevel <= 2)
        {
            healthCoinsText.text = Global.playerUpgradesCosts["health"][healthLevel + 1].ToString();
            healthLevelText.text = $"<b>Lv. {healthLevel} ( ➝ {healthLevel + 1}) </b>";
            if (!healthUpgradeButton.IsInteractable())
                healthUpgradeButton.interactable = true;
        }
        else
        {
            healthCoinsText.text = "MAX";
            healthLevelText.text = $"<b>Lv. {healthLevel} (MAX) </b>";
            if (healthUpgradeButton.IsInteractable())
                healthUpgradeButton.interactable = false;
        }

        if (damageLevel <= 2)
        {
            damageCoinsText.text = Global.playerUpgradesCosts["damage"][damageLevel + 1].ToString();
            damageLevelText.text = $"<b>Lv. {damageLevel} ( ➝ {damageLevel + 1}) </b>";
            if (!damageUpgradeButton.IsInteractable())
                damageUpgradeButton.interactable = true;
        }
        else
        {
            damageCoinsText.text = "MAX";
            damageLevelText.text = $"<b>Lv. {damageLevel} (MAX) </b>";
            if (damageUpgradeButton.IsInteractable())
                damageUpgradeButton.interactable = false;
        }

        if (shieldLevel <= 2)
        {
            shieldCoinsText.text = Global.playerUpgradesCosts["shield"][shieldLevel + 1].ToString();
            shieldLevelText.text = $"<b>Lv. {shieldLevel} ( ➝ {shieldLevel + 1}) </b>";
            if (!shieldUpgradeButton.IsInteractable())
                shieldUpgradeButton.interactable = true;
        }
        else
        {
            shieldCoinsText.text = "MAX";
            shieldLevelText.text = $"<b>Lv. {shieldLevel} (MAX) </b>";
            if (shieldUpgradeButton.IsInteractable())
                shieldUpgradeButton.interactable = false;
        }

        if (magnetLevel <= 2)
        {
            magnetCoinsText.text = Global.playerUpgradesCosts["magnet"][magnetLevel + 1].ToString();
            magnetLevelText.text = $"<b>Lv. {magnetLevel} ( ➝ {magnetLevel + 1}) </b>";
            if (!magnetUpgradeButton.IsInteractable())
                magnetUpgradeButton.interactable = true;
        }
        else
        {
            magnetCoinsText.text = "MAX";
            magnetLevelText.text = $"<b>Lv. {magnetLevel} (MAX) </b>";
            if (magnetUpgradeButton.IsInteractable())
                magnetUpgradeButton.interactable = false;
        }
    }

    public void PlayButtonClicked()
    {
        clickedSound.Play();
        sceneLoader.LoadScene("LevelMenu");
    }

    public void OptionsButtonClicked()
    {
        clickedSound.Play();
        optionsUI.SetActive(true);
    }

    public void StoreButtonClicked()
    {
        clickedSound.Play();
        storeUI.SetActive(true);
    }

    public void HowToPlayButtonClicked()
    {
        clickedSound.Play();
        howToPlayUI.SetActive(true);
    }

    public void UpgradeButtonClicked(string property)
    {
        if (Global.currentPlayerLevels[property] > 2) return;

        int requiredCoins = Global.playerUpgradesCosts[property][Global.currentPlayerLevels[property] + 1];
        if (Global.total_coins >= requiredCoins)
        {
            upgradeSound.Play();

            Global.currentPlayerLevels[property] += 1;
            Global.total_coins -= requiredCoins;
            RefreshUpgradesUI();

            // save the progress
            SaveSystem.SavePlayerData();
        }
        else
        {
            notEnoughCoinsSound.Play();

            if (notEnoughCoinsCoroutine == null)
                notEnoughCoinsCoroutine = StartCoroutine(ShowNotEnoughCoins());
        }
    }

    private IEnumerator ShowNotEnoughCoins()
    {
        notEnoughCoinsText.SetActive(true);
        yield return new WaitForSeconds(1f);
        notEnoughCoinsText.SetActive(false);
        notEnoughCoinsCoroutine = null;
    }

    public void ExitButtonClicked()
    {
        clickedSound.Play();
        print("quitting");
        Application.Quit();
    }

    public void BackButtonClicked()
    {
        clickedSound.Play();
        if (optionsUI.activeInHierarchy)
            optionsUI.SetActive(false);
        else if (storeUI.activeInHierarchy)
            storeUI.SetActive(false);
    }

    public void StoryOkButtonClicked()
    {
        clickedSound.Play();
        Global.newGame = false;
        storyUI.SetActive(false);

        // save data for saving newGame variable
        SaveSystem.SavePlayerData();
    }

    public void HowToPlayOkButtonClicked()
    {
        clickedSound.Play();
        howToPlayUI.SetActive(false);
    }
}
