using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    public SceneLoader sceneLoader;

    public PlayerStats player;
    [HideInInspector] public LevelData currentLevel;
    private int currentWaveIndex;
    private int totalWaves;
    private bool waveStarted = false;
    private GameObject currentEnemyPrefab;
    private Dictionary<int, string> enemyShipDict = new Dictionary<int, string>()
    {
        {1, "EnemyShipWeak" },
        {2, "EnemyShipNormal" },
        {3, "EnemyShipStrong" },
        {4, "EnemyShipBoss" },
    };
    private List<GameObject> activeEnemies = new List<GameObject>();
    public Transform positionSet2;
    public Transform positionSet3;
    private Transform[] positionSet2Array = new Transform[2];
    private Transform[] positionSet3Array = new Transform[3];
    private List<Vector2> usedPositions = new List<Vector2>();
    public Transform posSingle;
    public List<GameObject> powerups = new List<GameObject>();
    private AudioSource[] impactSounds;

    public PlayerInput playerInput;

    public GameObject redBackground;
    public GameObject waveUI;
    public GameObject victoryUI;
    public GameObject gameoverUI;
    public GameObject strongBulletUI;
    public TMP_Text coinsText;
    public Text victoryCoinsText;
    private Animator coinsUIAnimator;
    public GameObject pauseUI;
    private Animator pauseUIAnimator;

    public GameObject bossHealthbar;
    public RectTransform pauseButton;
    private float pauseButtonXPosNormal;

    // flags
    private bool gameOver;
    private bool victory;
    [HideInInspector] public bool isPaused = false;

    // timers
    public float coinsUIDuration = 0.5f;
    private float coinsUITimer = 0f;
    private bool coinsUITimerTrigger = false;

    private float powerupSpawnGap;
    private float powerupSpawnTimer = 0f;
    
    // sounds and music
    public AudioSource levelMusic;
    public AudioClip bulletClip;
    public AudioSource bulletSound;
    public AudioSource strongBulletSound;
    public AudioSource explosionSound;
    public AudioSource powerupSound;
    public AudioSource playerHurtSound;
    public AudioSource coinSound;
    public AudioSource victorySound;
    public AudioSource defeatedSound;

    private void Awake()
    {
        // setting player bullet multiplier variable before anything
        Global.playerBulletMultiplier = Global.playerUpgradesDict["damage"][Global.currentPlayerLevels["damage"]];
    }

    private void Start()
    {
        print("Level: " + Global.selectedLevel.ToString());

        // stopping the menu music and playing the level music
        MainMusicScript mainMenuMusic = MainMusicScript.Instance;
        if (mainMenuMusic != null)
        {
            mainMenuMusic.GetComponent<AudioSource>().Stop();    // stopping menu music
        }
        levelMusic.GetComponent<AudioSource>().Play();    // playing level music

        coinsUIAnimator = GameObject.Find("CoinsUI").GetComponent<Animator>();
        pauseButtonXPosNormal = GameObject.Find("PauseButton").GetComponent<RectTransform>().anchoredPosition.x;
        pauseUIAnimator = pauseUI.GetComponent<Animator>();
        if (pauseUIAnimator == null)
        {
            Debug.LogError("No animator found!!");
        }

        // loading current level
        currentLevel = Resources.Load<LevelData>("Levels/Level" + Global.selectedLevel.ToString());
        totalWaves = currentLevel.waves.Count;

        // loading spawn position arrays
        for (int i=0; i<positionSet2.childCount; i++)
        {
            positionSet2Array[i] = positionSet2.GetChild(i);
        }
        for (int i=0; i<positionSet3.childCount; i++)
        {
            positionSet3Array[i] = positionSet3.GetChild(i);
        }

        // loading impact sounds
        impactSounds = GameObject.Find("ImpactSounds").GetComponentsInChildren<AudioSource>();

        // setting coinText to 0 when starting the game
        coinsText.text = "0";
        // setting strong bullet text
        updateStrongBulletText();

        powerupSpawnGap = UnityEngine.Random.Range(5f, 15f);
        StartCoroutine(StartLevel());

        // switch to Player action map
        playerInput.SwitchCurrentActionMap("Player");
    }

    private void Update()
    {
        // timers
        if (powerupSpawnTimer >= powerupSpawnGap)
        {
            // spawn powerup
            powerupSpawnTimer = 0f;
            powerupSpawnGap = UnityEngine.Random.Range(5f, 15f);
            GameObject currentPowerup = Global.GetRandomElement<GameObject>(powerups);
            Instantiate(currentPowerup, new Vector2(UnityEngine.Random.Range(-9f, 9f), 7.5f), transform.rotation);
        }
        else
        {
            powerupSpawnTimer += Time.deltaTime;
        }

        if (coinsUITimerTrigger == true)
        {
            coinsUITimer += Time.deltaTime;

            if (coinsUITimer >= coinsUIDuration)
            {
                // hide coinsUI
                coinsUIAnimator.SetTrigger("CoinEnd");
                coinsUITimer = 0f;
                coinsUITimerTrigger = false;
            }
        }

        // user input(s)
        if (!victory && !gameOver)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                // pause the game if escape is pressed
                GamePause();
            }
        }

        if (player.health <= 0)
        {
            GameOver();    // gameover when player health gets to 0
        }
    }

    public void updateStrongBulletText()
    {
        strongBulletUI.GetComponentInChildren<TMP_Text>().text = Global.strongBullets.ToString();
    }

    private void FinalizeLevel(bool win)
    {
        if (win)
        {
            Global.total_coins += Global.coins;    // adding the coins IF won
            if (Global.levelsUnlockedTill < Global.totalLevels && Global.selectedLevel == Global.levelsUnlockedTill)     // updating levelsUnlockedTill to unlock another level if won AND playing the latest level
                Global.levelsUnlockedTill += 1;

            // save the progress
            SaveSystem.SavePlayerData();
        }

        Global.coins = 0;
        Global.shieldsUp = false;
        Global.magnetMultiplier = 1;
        Global.strongBullets = 3;
    }

    // coroutine to hide wave UI after a few seconds after the wave is started
    private IEnumerator HideWaveUI(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        waveUI.SetActive(false);
    }

    private IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(1f);

        while (currentWaveIndex < totalWaves)
        {
            ShowWaveWarning(currentWaveIndex+1);
            yield return StartCoroutine(StartWave(currentLevel.waves[currentWaveIndex]));
            yield return new WaitUntil(() => activeEnemies.Count == 0);
            currentWaveIndex++;
        }

        // level complete
        StartCoroutine(Victory());
    }

    private IEnumerator Victory()
    {
        victory = true;
        // wait for 2 seconds
        yield return new WaitForSeconds(2f);
        // switch to UI action map
        playerInput.SwitchCurrentActionMap("UI");
        // stop level music
        levelMusic.Stop();
        // play victory sound
        victorySound.Play();
        // show victory UI
        Time.timeScale = 0;
        victoryUI.SetActive(true);
        victoryCoinsText.text = $"+ {Global.coins}";
        yield break;
    }

    private IEnumerator StartWave(WaveData wave)
    {
        if (waveStarted) yield break;
        waveStarted = true;

        if (wave.enemyTier == 4)
        {
            // enable boss healthbar
            if (!bossHealthbar.activeInHierarchy)
            {
                float canvasWidth = pauseButton.parent.GetComponent<RectTransform>().rect.width;    // using canvas width to put the button in top middle
                pauseButton.anchoredPosition = new Vector2(-canvasWidth / 2f, pauseButton.anchoredPosition.y);    // using anchoredPosition since its on canvas
                bossHealthbar.SetActive(true);
            }
        }
        else
        {
            // disable boss healthbar
            if (bossHealthbar.activeInHierarchy)
            {
                pauseButton.anchoredPosition = new Vector2(pauseButtonXPosNormal, pauseButton.anchoredPosition.y);    // using anchoredPosition since its on canvas
                bossHealthbar.SetActive(false);
            }
        }

        currentEnemyPrefab = Resources.Load<GameObject>("Enemies/" + enemyShipDict[wave.enemyTier]);
        if (currentEnemyPrefab == null)
        {
            Debug.LogError("Error loading enemy prefab!");
            waveStarted = false;
            yield break;
        }

        for (int i=0; i<wave.enemyAmount; i++)
        {
            SpawnNewEnemy(wave.enemyAmount);
            yield return new WaitForSeconds(2f);
        }

        if (usedPositions.Count != 0)
        {
            usedPositions.Clear();
        }

        currentEnemyPrefab = null;
        waveStarted = false;
    }

    private void ShowWaveWarning(int waveNo)
    {
        if (waveNo < totalWaves)
        {
            waveUI.GetComponentInChildren<Image>().color = new Color32(255, 255, 255, 255);
            waveUI.GetComponentInChildren<Text>().text = "<b>Wave: " + (waveNo).ToString() + "</b>";
        }
        else
        {
            waveUI.GetComponentInChildren<Image>().color = new Color32(255, 0, 0, 255);
            waveUI.GetComponentInChildren<Text>().text = "<b>Final Wave!</b>";
        }
        waveUI.SetActive(true);
        StartCoroutine(HideWaveUI(2f));
    }

    private void SpawnNewEnemy(int positionSet=0)
    {
        Vector2 currentPos = new Vector2();

        if (positionSet == 2)     // spawn using preset positions for enemies of amount 2
        {
            currentPos = positionSet2Array.GetRandomElement<Transform>().position;
            // regenerating the random position if it's already used
            while (usedPositions.Contains(currentPos))
            {
                currentPos = positionSet2Array.GetRandomElement<Transform>().position;
            }
            usedPositions.Add(currentPos);    // adding the current pos in usedPositions list
            GameObject newEnemy = Instantiate(currentEnemyPrefab, new Vector2(currentPos.x, 7.5f), transform.rotation);    // spawning the enemy
            activeEnemies.Add(newEnemy);    // adding the new enemy in activeEnemies list
        }
        else if (positionSet == 3)     // spawn using preset positions for enemies of amount 3
        {
            currentPos = positionSet3Array.GetRandomElement<Transform>().position;
            // regenerating the random position if it's already used
            while (usedPositions.Contains(currentPos))
            {
                currentPos = positionSet3Array.GetRandomElement<Transform>().position;
            }
            usedPositions.Add(currentPos);    // adding the current pos in usedPositions list
            GameObject newEnemy = Instantiate(currentEnemyPrefab, new Vector2(currentPos.x, 7.5f), transform.rotation);
            activeEnemies.Add(newEnemy);    // adding the new enemy in activeEnemies list
        }
        else if (positionSet == 1)    // boss/single enemy
        {
            if (posSingle != null)
            {
                GameObject newEnemy = Instantiate(currentEnemyPrefab, posSingle.position, transform.rotation);
                activeEnemies.Add(newEnemy);    // adding the new enemy in activeEnemies list
            }
        }
        else      // otherwise using random values for x position on the screen
        {
            GameObject newEnemy = Instantiate(currentEnemyPrefab, new Vector2(UnityEngine.Random.Range(-9f, 9f), 7.5f), transform.rotation);
            activeEnemies.Add(newEnemy);
        }
    }

    // function to perform tasks when an enemy is destroyed
    public void EnemyDestroyed(GameObject enemy, bool canSpawnCoins=false)
    {
        activeEnemies.Remove(enemy);

        if (canSpawnCoins)
        {
            explosionSound.Play();
            List<GameObject> enemyCoinSetList = enemy.GetComponentInChildren<EnemyShipScript>().coinSetList;
            GameObject currentCoinSet = Global.GetRandomElement(enemyCoinSetList);
            Instantiate(currentCoinSet, enemy.transform.position, enemy.transform.rotation);
        }
    }

    // function to perform tasks when a coin set is collected
    public void CoinSetCollected()
    {
        // updating the coin text
        coinsText.text = Global.coins.ToString();

        if (coinsUITimerTrigger == false)
        {
            coinsUITimerTrigger = true;
            coinsUIAnimator.SetTrigger("CoinStart");
        }

        coinsUITimer = 0f;
    }

    public void PlayStrongBulletSound()
    {
        strongBulletSound.PlayOneShot(strongBulletSound.clip);
    }
    
    public void PlayBulletSound()
    {
        bulletSound.PlayOneShot(bulletClip);
    }

    public void PlayImpactSound()
    {
        Global.GetRandomElement<AudioSource>(impactSounds).Play();
    }

    public void PlayPowerupSound()
    {
        powerupSound.Play();
    }

    // function to play hurt sound and show red effect for a split second when player takes damage
    public void PlayPlayerHurtEffect()
    {
        playerHurtSound.Play();
        StartCoroutine(ShowRedEffect());
    }

    private IEnumerator ShowRedEffect()
    {
        redBackground.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        redBackground.SetActive(false);
    }

    public void PlayCoinSound()
    {
        coinSound.Play();
    }

    public void GameOver()
    {
        if (!gameOver)
        {
            // stop level music
            levelMusic.Stop();
            // switch to UI action map
            playerInput.SwitchCurrentActionMap("UI");
            // play explosion sound (for player ship explosion)
            explosionSound.Play();
            // play defeated sound
            defeatedSound.Play();
            // show gameover UI
            gameOver = true;
            gameoverUI.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void GamePause()
    {
        if (isPaused)
        {
            // unpause the game
            // resume level music
            levelMusic.UnPause();
            // switch to Player action map
            playerInput.SwitchCurrentActionMap("Player");
            pauseUIAnimator.SetTrigger("EndPauseTrigger");
            StartCoroutine(PauseDelayedDeactivation());
            Time.timeScale = 1;
        }
        else
        {
            // pause the game
            // pause level music
            levelMusic.Pause();
            // switch to UI action map
            playerInput.SwitchCurrentActionMap("UI");
            pauseUI.SetActive(true);
            pauseUIAnimator.SetTrigger("StartPauseTrigger");   // triggering StartPauseTrigger to start pause fade in animation
            Time.timeScale = 0;
        }
        
        isPaused = !isPaused;    // revert the value of isPaused
    }

    private IEnumerator PauseDelayedDeactivation()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        pauseUI.SetActive(false);
    }

    public void GameoverOkButtonPressed()
    {
        Time.timeScale = 1;
        FinalizeLevel(win: false);
        sceneLoader.LoadScene("LevelMenu");
    }

    public void VictoryContinueButtonPressed()
    {
        Time.timeScale = 1;
        FinalizeLevel(win: true);
        sceneLoader.LoadScene("LevelMenu");
    }

    public void PauseExitButtonPressed()
    {
        Time.timeScale = 1;
        sceneLoader.LoadScene("MainMenu");
    }
}
