using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotAirBalloon : MonoBehaviour {

    public BalloonGameManager.players thisPlayer;

    public float liftPower = 120f;
    public float horizontalPower = 60f;

    public GameObject[] flammablePoints;
    public GameObject glowSprite;
    public Counter killCounter;
    public Counter coinCounter;

    public PlayersInputs mPlayerInputs;

    private float respawnTimer = 3.0f;
    private float respawnDelay;
    private Vector2 leftStickInput;
    private GameObject basket;
    private Animator balloonAnimator;
    private float springDistance;

    // Spawn Positions:
    private Vector3 mySpawnPosition;
    private Vector3 myBasketPosition;

    public bool playerControlsEnabled = true;

    public bool isOnFire = false;

    public bool playerIsOffScreen = false;
    private float playerIsOffScreenTimer = 0f;

    // public BezierSolution.BezierWalkerWithSpeed bezierWalker;


    void Start ()
    {
        mPlayerInputs = new PlayersInputs((int)thisPlayer + 1);

        basket = gameObject.transform.Find("Balloon_Basket").gameObject;
        balloonAnimator = GetComponentInChildren<Animator>();

        mySpawnPosition = transform.position;
        myBasketPosition = basket.transform.position;

        respawnDelay = respawnTimer;

        springDistance = GetComponent<SpringJoint2D>().distance;
        playerIsOffScreenTimer = BalloonGameManager.Instance.timeOffScreenBeforeDeath;

        // bezierWalker = GetComponent<BezierSolution.BezierWalkerWithSpeed>();
    }
	
	void Update ()
    {
        mPlayerInputs.Update();

        leftStickInput = mPlayerInputs.LeftStick;

        if (mPlayerInputs.StartButton.WentDown)
        {
            // Debug.Log("Start Pressed on Balloon");
            BalloonGameManager.Instance.startPressed = true;
        }

        if (mPlayerInputs.AButton.WentDown || mPlayerInputs.StartButton.WentDown)
        {
            // Debug.Log("Accept Pressed on Balloon");
            BalloonGameManager.Instance.acceptPressed = true;
        }

        if (mPlayerInputs.BButton.WentDown)
        {
            // Debug.Log("Cancel Pressed on Balloon");
            BalloonGameManager.Instance.cancelPressed = true;
        }

        if (mPlayerInputs.UI_Movement_Up.WentDown)
        {
            balloonAnimator.SetBool("balloon_thrustStart", true);
        }

        if (!balloonAnimator.GetBool("animator_isDead") && !BalloonGameManager.GameIsPaused)
        {
            if (playerControlsEnabled)
            {
                if (leftStickInput.y > 0.05f || Mathf.Abs(leftStickInput.x) > 0.05f)
                {
                    if (!glowSprite.activeSelf)
                        glowSprite.SetActive(true);

                    gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(leftStickInput.x * horizontalPower * Time.deltaTime, Mathf.Abs(leftStickInput.y * liftPower * Time.deltaTime)));
                    // Debug.Log(thisPlayer + "'s Left Stick Input: " + leftStickInput);
                }
                else if (leftStickInput.magnitude <= 0.15f)
                {
                    if (glowSprite.activeSelf)
                        glowSprite.SetActive(false);

                    // If the stick isn't in use, apply a light constant force to keep the balloon afloat.
                    gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 0.3f * liftPower * Time.deltaTime));
                }
                if (leftStickInput.y < 0.15)
                {
                    // Charge boost
                    // Start charge timer
                    // when isCharged is true, release to launch balloon in the opposite direction of the downward stick deflection
                    // Balloon gets boost flame effects
                }
            }
        }

        if (playerIsOffScreen)
        {
            playerIsOffScreenTimer -= Time.deltaTime;
            if(playerIsOffScreenTimer <= 0)
                DeathActions();
        }

        if (balloonAnimator.GetBool("animator_isDead"))
        {
            respawnDelay -= Time.deltaTime;
            if(respawnDelay <= 0)
                RespawnActions();
        }

        if(BalloonGameManager.Instance.currentUI_menuScript != null)
        {
            // Attempt to add D-Pad support for UI:

            // if (mPlayerInputs.UI_Movement_Up.WentDown || mPlayerInputs.Dpad_Movement_Up.WentDown)
            //     BalloonGameManager.Instance.currentUI_menuScript.moveUp = true;
            // if (mPlayerInputs.UI_Movement_Down.WentDown || mPlayerInputs.Dpad_Movement_Down.WentDown)
            //     BalloonGameManager.Instance.currentUI_menuScript.moveDown = true;
            // if (mPlayerInputs.UI_Movement_Left.WentDown || mPlayerInputs.Dpad_Movement_Left.WentDown)
            //     BalloonGameManager.Instance.currentUI_menuScript.moveLeft = true;
            // if (mPlayerInputs.UI_Movement_Right.WentDown || mPlayerInputs.Dpad_Movement_Right.WentDown)
            //     BalloonGameManager.Instance.currentUI_menuScript.moveRight = true;

            if (mPlayerInputs.UI_Movement_Up.WentDown)
                BalloonGameManager.Instance.currentUI_menuScript.moveUp = true;
            if (mPlayerInputs.UI_Movement_Down.WentDown)
                BalloonGameManager.Instance.currentUI_menuScript.moveDown = true;
            if (mPlayerInputs.UI_Movement_Left.WentDown)
                BalloonGameManager.Instance.currentUI_menuScript.moveLeft = true;
            if (mPlayerInputs.UI_Movement_Right.WentDown)
                BalloonGameManager.Instance.currentUI_menuScript.moveRight = true;
        }
    }

    private void DeathActions()
    {

        // bezierWalker.spline.gameObject.transform.position = transform.position;
        FindObjectOfType<AudioManager>().Play("Pop");
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        // GetComponent<SpringJoint2D>().enabled = false;

        glowSprite.SetActive(false);

        playerIsOffScreen = false;
        playerIsOffScreenTimer = BalloonGameManager.Instance.timeOffScreenBeforeDeath;

        BalloonGameManager.Instance.playerKilled = true;

        balloonAnimator.SetBool("animator_isDead", true);
    }

    private void RespawnActions()
    {
        BalloonGameManager.Instance.SwitchPlayerMarker((int)thisPlayer, true);

        transform.position = mySpawnPosition;
        basket.transform.position = myBasketPosition;
        basket.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<Rigidbody2D>().simulated = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<SpringJoint2D>().enabled = true;
        GetComponent<SpringJoint2D>().distance = springDistance;

        respawnDelay = respawnTimer;

        balloonAnimator.SetBool("animator_isDead", false);

        Debug.Log("Player " + thisPlayer + " respawned!");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Balloon"))
        {
            if (collision.gameObject.GetComponent<UnityJellySprite>())
            {
                FindObjectOfType<AudioManager>().Play("Pop");
                Destroy(collision.gameObject);
            }

            if (isOnFire)
            {
                if (collision.gameObject.GetComponent<HotAirBalloon>())
                {
                    // ------------------ I should NOT be relying on "balloonAnimator.GetBool("animator_isDead")" - it lives inside the Animator component
                    if (collision.gameObject.GetComponent<HotAirBalloon>().thisPlayer != thisPlayer && !balloonAnimator.GetBool("animator_isDead"))
                    {
                        collision.gameObject.GetComponent<HotAirBalloon>().killCounter.UpdateCounter(1);
                        BalloonGameManager.Instance.ChangeScore((int)thisPlayer, 1, BalloonGameManager.statType.kills);
                        // BalloonGameManager.Instance.SwitchPlayerMarker((int)collision.transform.parent.GetComponent<HotAirBalloon>().thisPlayer, false);

                        DeathActions();
                    }
                }
            }
            else if (!isOnFire)
            {
                FindObjectOfType<AudioManager>().Play("Bounce");
            }
        }
        if (collision.gameObject.CompareTag("Basket"))
        {
            if (collision.gameObject.GetComponent<BalloonBasket>())
            {
                if (!collision.gameObject.GetComponent<BalloonBasket>().myBalloonIsPopped)
                {
                    collision.gameObject.transform.parent.GetComponent<HotAirBalloon>().killCounter.UpdateCounter(1);
                    BalloonGameManager.Instance.ChangeScore((int)thisPlayer, 1, BalloonGameManager.statType.kills);
                    // BalloonGameManager.Instance.SwitchPlayerMarker((int)collision.transform.parent.GetComponent<HotAirBalloon>().thisPlayer, false);
                    DeathActions();
                }
            }
            else
            {
                FindObjectOfType<AudioManager>().Play("Bounce");
            }
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            FindObjectOfType<AudioManager>().Play("Coin");
            coinCounter.UpdateCounter(1);
            BalloonGameManager.Instance.ChangeScore((int)thisPlayer, 1, BalloonGameManager.statType.coins);
            ObjectPooler.Instance.SpawnFromPool("FX_CoinGETPool", collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("PurpleCoin"))
        {
            FindObjectOfType<AudioManager>().Play("PurpleCoin");
            coinCounter.UpdateCounter(5);
            BalloonGameManager.Instance.ChangeScore((int)thisPlayer, 5, BalloonGameManager.statType.coins);
            ObjectPooler.Instance.SpawnFromPool("FX_CoinGETPool", collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }

        
    }

    /*
    void OnJellyCollisionEnter(JellySprite.JellyCollision collision)
    void OnJellyCollisionExit(JellySprite.JellyCollision collision)
    void OnJellyCollisionStay(JellySprite.JellyCollision collision)

    void OnJellyCollisionEnter2D(JellySprite.JellyCollision2D collision)
    void OnJellyCollisionExit2D(JellySprite.JellyCollision2D collision)
    void OnJellyCollisionStay2D(JellySprite.JellyCollision2D collision)

    void OnJellyTriggerEnter(JellySprite.JellyCollider trigger)
    void OnJellyTriggerExit(JellySprite.JellyCollider trigger)
    void OnJellyTriggerStay(JellySprite.JellyCollider trigger)

    void OnJellyTriggerEnter2D(JellySprite.JellyCollider2D trigger)
    void OnJellyTriggerExit2D(JellySprite.JellyCollider2D trigger)
    void OnJellyTriggerStay2D(JellySprite.JellyCollider2D trigger)
    */
}
