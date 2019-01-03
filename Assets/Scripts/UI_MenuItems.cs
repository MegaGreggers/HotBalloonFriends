using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MenuItems : MonoBehaviour
{

    public GameObject menuCursor;
    public List<GameObject> menuItems = new List<GameObject>();
    public Vector3 cursorOffsetFromMenuItem = new Vector3(-0.1f, 0f, 0f);
    public int currentIndex = 0;

    public bool moveUp = false;
    public bool moveDown = false;
    public bool moveLeft = false;
    public bool moveRight = false;

    // Use this for initialization
    void Start()
    {
        currentIndex = 0;
        menuCursor.transform.position = menuItems[0].transform.position + cursorOffsetFromMenuItem;
        BalloonGameManager.Instance.currentUI_menuScript = this;
    }
	
	// Update is called once per frame
	public void Update ()
    {
        if (moveRight)
        {
            moveDown = true;
            moveRight = false;
        }
            
        if (moveLeft)
        {
            moveUp = true;
            moveLeft = false;
        }
            

        if (moveDown)
        {
            currentIndex++;

            if (currentIndex >= menuItems.Count)
                currentIndex = 0;

            UpdateCursorPosition(currentIndex);
            moveDown = false;
        }

        if (moveUp)
        {
            currentIndex-=1;

            if (currentIndex < 0)
                currentIndex = menuItems.Count - 1;

            UpdateCursorPosition(currentIndex);
            moveUp = false;
        }
    }

    void UpdateCursorPosition(int thisIndex)
    {
        menuCursor.transform.position = menuItems[thisIndex].transform.position + cursorOffsetFromMenuItem;
        FindObjectOfType<AudioManager>().Play("Menu_Blip_1");
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


        GUI.Label(new Rect(new Vector2(10, 10), new Vector2(500, 200)), "currentIndex = " + currentIndex, gUIStyleBlack);
        GUI.Label(new Rect(new Vector2(9, 9), new Vector2(500, 200)), "currentIndex = " + currentIndex, gUIStyleGreen);
    }
}
