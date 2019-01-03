using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BalloonGameManager : MonoBehaviour
{
    public static BalloonGameManager Instance;

    public bool playerInputsEnabled = false;

    public enum players { player1, player2, player3, player4 };
    public List<GameObject> playerGOs = new List<GameObject>();

    public enum statType { kills, coins, speed };

    public enum GameScreen { title, options, characterSelect, gameSettings, gamePlay, results, credits, adventure, battle };
    public static GameScreen currentGameScreen;
    public GameScreen desiredGameScreen;

    public int numPlayersInThisRound = 2;

    public GameObject UI_PauseMenu;
    public GameObject UI_Score;
    public GameObject UI_TitleLogo_Interactive;
    public static bool GameIsPaused = false;
    public bool startPressed = false;
    public bool cancelPressed = false;
    public bool acceptPressed = false;
    public bool UI_moveUp = false;
    public bool UI_moveDown = false;
    public bool UI_moveLeft = false;
    public bool UI_moveRight = false;

    public int p1_score = 0;
    public int p2_score = 0;
    public int p3_score = 0;
    public int p4_score = 0;

    public int p1_coins = 0;
    public int p2_coins = 0;
    public int p3_coins = 0;
    public int p4_coins = 0;

    public GameObject p1;
    public GameObject p2;
    public GameObject p3;
    public GameObject p4;

    public Transform p1_marker;
    public Transform p2_marker;
    public Transform p3_marker;
    public Transform p4_marker;

    public Vector3 markerOffset;

    public Sprite p1_sprite;
    public Sprite p2_sprite;
    public Sprite p3_sprite;
    public Sprite p4_sprite;
    public Sprite deathSprite;

    public bool playerKilled = false;
    private CameraShake camShakeScript;

    public Scene titleScreen;

    public UI_MenuItems currentUI_menuScript;
    public float timeOffScreenBeforeDeath = 2.0f;

    public AnimationCurve testCurve;

    void Awake ()
    {
        // Make this GameObject persist between scenes
        DontDestroyOnLoad(gameObject);

        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        p1_sprite = p1_marker.gameObject.GetComponent<SpriteRenderer>().sprite;
        p2_sprite = p2_marker.gameObject.GetComponent<SpriteRenderer>().sprite;
        p3_sprite = p3_marker.gameObject.GetComponent<SpriteRenderer>().sprite;
        p4_sprite = p4_marker.gameObject.GetComponent<SpriteRenderer>().sprite;

        camShakeScript = Camera.main.GetComponent<CameraShake>();

        // UI_Camera = GameObject.FindGameObjectWithTag("UI_Camera");
        // FindObjectOfType<AudioManager>().Play("ElevatorMusic");

        GameScreenLoader(GameScreen.title);
    }

    private void Start()
    {
        GameObject[] playerBalloons = GameObject.FindGameObjectsWithTag("Balloon");
        foreach (GameObject go in playerBalloons)
        {
            if (go.GetComponent<HotAirBalloon>())
            {
                // go.GetComponent<HotAirBalloon>().playerControlsEnabled = false;
                playerGOs.Add(go);
            }
        }
    }

    private void Update()
    {
        // Logic for scene/menu transitions
        if (startPressed)
        {
            if (currentGameScreen == GameScreen.title)
            {
                GameScreenLoader(GameScreen.gameSettings);
                startPressed = false;
                return;
            }
            else if (currentGameScreen == GameScreen.gamePlay)
            {
                if (GameIsPaused)
                {
                    Resume();
                    return;
                }
                else
                {
                    Pause();
                    return;
                }
            }
            
            // testCurve.Evaluate(Time.deltaTime);
        }

        if (acceptPressed)
        {
            if (currentUI_menuScript != null)
            {
                // Load Scene based on menu item index from currentUI_menuScript
                switch (currentGameScreen)
                {
                    case GameScreen.title:
                        GameScreenLoader(GameScreen.gameSettings);
                        break;
                    case GameScreen.gameSettings:
                        switch (currentUI_menuScript.currentIndex)
                        {
                            case 0:
                                Debug.Log("1P ADVENTURE: ");
                                break;
                            case 1:
                                Debug.Log("2P ADVENTURE: ");
                                break;
                            case 2:
                                Debug.Log("4P BATTLE: ");
                                break;
                            case 3:
                                Debug.Log("OPTIONS: ");
                                GameScreenLoader(GameScreen.options);
                                break;
                            case 4:
                                Debug.Log("BACK: ");
                                GameScreenLoader(GameScreen.title);
                                break;
                        }
                        break;

                    case GameScreen.options:
                        switch (currentUI_menuScript.currentIndex)
                        {
                            case 0:
                                Debug.Log("VOLUME: ");
                                break;
                            case 1:
                                Debug.Log("CONTROLS: ");
                                break;
                            case 2:
                                Debug.Log("CREDITS: ");
                                break;
                            case 3:
                                Debug.Log("BACK: Loading GameSettings");
                                GameScreenLoader(GameScreen.gameSettings);
                                break;
                        }
                        break;

                    case GameScreen.gamePlay:

                        break;
                }
            }
            else if (currentUI_menuScript == null)
            {
                Debug.LogWarning("currentUI_menuScript not assigned by current menu.");
            }
            
            acceptPressed = false;
        }

        if (cancelPressed)
        {
            if(currentGameScreen == GameScreen.gameSettings)
            {
                GameScreenLoader(GameScreen.title);
            }

            if (currentGameScreen == GameScreen.options)
            {
                GameScreenLoader(GameScreen.gameSettings);
            }

            cancelPressed = false;
        }

        if (playerKilled)
        {
            camShakeScript.AddTrauma(0.4f);
            StartCoroutine("DelayTimeOnImpact", 0.2f);
            playerKilled = false;
        }
    }

    private void LateUpdate()
    {
        // UpdateMarkerPositions();
    }

    private void GameScreenLoader(GameScreen curGameScreen)
    {
        switch (curGameScreen)
        {
            case GameScreen.title:

                SceneManager.LoadScene("Gameplay", LoadSceneMode.Single);
                SceneManager.LoadScene("TitleScreen", LoadSceneMode.Additive);
                FindObjectOfType<AudioManager>().Play("PushDaButtan");
                currentGameScreen = GameScreen.title;

                return;

            case GameScreen.options:
                
                if (SceneManager.GetSceneByName("GameSettings").isLoaded)
                    SceneManager.UnloadSceneAsync("GameSettings");
                // Load options scene additively
                if (!SceneManager.GetSceneByName("OptionsScreen").isLoaded && !SceneManager.GetSceneByName("GameSettings").isLoaded)
                    SceneManager.LoadSceneAsync("OptionsScreen", LoadSceneMode.Additive);

                currentUI_menuScript = FindObjectOfType<UI_MenuItems>();

                // draw OPTIONS text
                // Music Volume
                // SFX Volume
                // Voice Volume
                // Adult Mode OFF

                currentGameScreen = GameScreen.options;

                return;

            case GameScreen.gameSettings:
                
                if (SceneManager.GetSceneByName("TitleScreen").isLoaded)
                {
                    SceneManager.UnloadSceneAsync("TitleScreen");
                }

                if (SceneManager.GetSceneByName("OptionsScreen").isLoaded)
                    SceneManager.UnloadSceneAsync("OptionsScreen");

                if (!SceneManager.GetSceneByName("GameSettings").isLoaded)
                    SceneManager.LoadSceneAsync("GameSettings", LoadSceneMode.Additive);

                if(UI_TitleLogo_Interactive)
                    UI_TitleLogo_Interactive.SetActive(false);

                currentUI_menuScript = FindObjectOfType<UI_MenuItems>();

                // Journey 1P/2P Co-op
                // Battle 2-4 Players
                // Options
                currentGameScreen = GameScreen.gameSettings;

                return;

            case GameScreen.gamePlay:
                
                if (SceneManager.GetSceneByName("TitleScreen").isLoaded)
                    SceneManager.UnloadSceneAsync("TitleScreen");
                if (SceneManager.GetSceneByName("GameSettings").isLoaded)
                    SceneManager.UnloadSceneAsync("GameSettings");

                foreach (GameObject go in playerGOs)
                {
                    go.GetComponent<HotAirBalloon>().playerControlsEnabled = true;
                }

                // First to 10 Kills WINS!

                // Enable Score UI
                UI_Score.SetActive(true);

                // Game modes - Time, Stock, Special Delivery (eg. pickup and deliver parcels competetively)

                // Enable Player Markers

                // Enable CameraFollow script
                currentGameScreen = GameScreen.gamePlay;

                return;

            case GameScreen.results:

                // Scores
                // Acheivements
                // Add stats to player profiles
                // Rematch
                // Game Lobby (change settings/characters etc.)
                // TitleScreen
                currentGameScreen = GameScreen.results;

                return;

            case GameScreen.credits:

                // Let'em roll! (minigame?)
                currentGameScreen = GameScreen.credits;

                return;
        }
    }

    private void TogglePlayerControls(bool enabled)
    {
        // Find all HotAirBalloon scripts and enable/disable player controls
    }

    public void ChangeScore(int playerNumber, int addedScore, statType stat)
    {
        switch (stat)
        {
            case statType.kills:
              switch (playerNumber)
                {
                    case 0:
                        p1_score += addedScore;
                        return;
                    case 1:
                        p2_score += addedScore;
                        return;
                    case 2:
                        p3_score += addedScore;
                        return;
                    case 3:
                        p4_score += addedScore;
                        return;
                }
                return;

            case statType.coins:
                switch (playerNumber)
                {
                    case 0:
                        p1_coins += addedScore;
                        return;
                    case 1:
                        p2_coins += addedScore;
                        return;
                    case 2:
                        p3_coins += addedScore;
                        return;
                    case 3:
                        p4_coins += addedScore;
                        return;
                }
                return;
        }
    }

    void UpdateMarkerPositions()
    {
        p1_marker.position = p1.transform.position + markerOffset;
        p2_marker.position = p2.transform.position + markerOffset;
        p3_marker.position = p3.transform.position + markerOffset;
        p4_marker.position = p4.transform.position + markerOffset;
    }

    public void SwitchPlayerMarker(int player, bool isAlive)
    {
        if (!isAlive)
        {
            switch (player)
            {
                case 0:
                    Debug.Log("Player zero does not exist!");
                    return;
                case 1:
                    p1_marker.gameObject.GetComponent<SpriteRenderer>().sprite = deathSprite;
                    return;
                case 2:
                    p2_marker.gameObject.GetComponent<SpriteRenderer>().sprite = deathSprite;
                    return;
                case 3:
                    p3_marker.gameObject.GetComponent<SpriteRenderer>().sprite = deathSprite;
                    return;
                case 4:
                    p4_marker.gameObject.GetComponent<SpriteRenderer>().sprite = deathSprite;
                    return;
            }
        }
        else if (isAlive)
        {
            switch (player)
            {
                case 0:
                    Debug.Log("Player zero does not exist!");
                    return;
                case 1:
                    p1_marker.gameObject.GetComponent<SpriteRenderer>().sprite = p1_sprite;
                    return;
                case 2:
                    p2_marker.gameObject.GetComponent<SpriteRenderer>().sprite = p2_sprite;
                    return;
                case 3:
                    p3_marker.gameObject.GetComponent<SpriteRenderer>().sprite = p3_sprite;
                    return;
                case 4:
                    p4_marker.gameObject.GetComponent<SpriteRenderer>().sprite = p4_sprite;
                    return;
            }
        }
    }
    
    void Pause()
    {
        // Debug.Log("Paused");
        UI_PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        // startPressed = false;
    }

    void Resume()
    {
        // Debug.Log("UnPaused");
        UI_PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        // startPressed = false;
    }

    private IEnumerator DelayTimeOnImpact(float delay)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f;
    }
    
    void OnGUI()
    {
        GUIStyle gUIStyleWhite = new GUIStyle();
        GUIStyle gUIStyleBlue = new GUIStyle();
        GUIStyle gUIStyleGreen = new GUIStyle();
        GUIStyle gUIStyleBlack = new GUIStyle();
        GUIStyle gUIStyleRed = new GUIStyle();
        GUIStyle gUIStyleYellow = new GUIStyle();

        gUIStyleWhite.normal.textColor = Color.white;
        gUIStyleBlue.normal.textColor = Color.blue;
        gUIStyleGreen.normal.textColor = Color.green;
        gUIStyleBlack.normal.textColor = Color.black;
        gUIStyleRed.normal.textColor = Color.red;
        gUIStyleYellow.normal.textColor = Color.yellow;

        // GUI.Label(new Rect(new Vector2(10, 10), new Vector2(500, 200)), "P1: " + p1_score, gUIStyleBlack);
        // GUI.Label(new Rect(new Vector2(9, 9), new Vector2(500, 200)), "P1: " + p1_score, gUIStyleRed);
        // 
        // GUI.Label(new Rect(new Vector2(10, 30), new Vector2(500, 200)), "P2: " + p2_score, gUIStyleBlack);
        // GUI.Label(new Rect(new Vector2(9, 29), new Vector2(500, 200)), "P2: " + p2_score, gUIStyleGreen);
        // 
        // GUI.Label(new Rect(new Vector2(10, 50), new Vector2(500, 200)), "P3: " + p3_score, gUIStyleBlack);
        // GUI.Label(new Rect(new Vector2(9, 49), new Vector2(500, 200)), "P3: " + p3_score, gUIStyleBlue);
        // 
        // GUI.Label(new Rect(new Vector2(10, 70), new Vector2(500, 200)), "P4: " + p4_score, gUIStyleBlack);
        // GUI.Label(new Rect(new Vector2(9, 69), new Vector2(500, 200)), "P4: " + p4_score, gUIStyleYellow);


        // GUI.Label(new Rect(new Vector2(10, 10), new Vector2(500, 200)), "currentGameScreen = " + currentGameScreen, gUIStyleBlack);
        // GUI.Label(new Rect(new Vector2(9, 9), new Vector2(500, 200)), "currentGameScreen = " + currentGameScreen, gUIStyleGreen);
    }

}
