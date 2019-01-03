using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class VoxelAlphabet : MonoBehaviour {
	
	
	public GameObject[] voxAlphabet = new GameObject[38];
	public string myText;
	public float textSize = 1.0f;
	public char[] myTextCharArray;
	private static char[] base26Chars = new char[38];
	// private float alphaXOffset = 0.5f;
	public Vector3 myPosition;
	Dictionary<char, GameObject> voxAlphaDictionary = new Dictionary<char, GameObject>();
	public Material textMaterial;
	
	public float leftMargin;
	public float rightMargin;
	public float lineSpacing = 1.0f;
	
	public float printTimeDelay = 0.07f;
	private int tempLetterIndex = 0;
	private float tempPrintTimeDelay = 0.0f;
	public float tempXPosition;
	public float tempYPosition;
	public int charactersPerLine = 20;
	private int lettersOnThisLine = 0;
	
	private bool printing = true;

    public float spacing = 1f;
    public bool isFinishedPrinting = false;
    public bool useShadows = true;
    public float shadowDistance = 0.008f;

    void Awake()
	{
		tempXPosition = 0f;
		tempYPosition = 0f;
		
		myTextCharArray = myText.ToCharArray();
		base26Chars = "abcdefghijklmnopqrstuvwxyz 0123456789!".ToCharArray();
		
		ParseAlphabetToMesh();
	}
	
	// Use this for initialization
	void Start () 
	{
		myPosition = gameObject.transform.position;

		//PrintAllVoxText(myText, myPosition);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(tempLetterIndex >= myTextCharArray.Length)
		{
			printing = false;
			
			tempLetterIndex = 0;
		}
		
		if(printing)
		{
            if (!isFinishedPrinting)
            {
                tempPrintTimeDelay += Time.deltaTime;

                if (tempLetterIndex < myTextCharArray.Length)
                {
                    if (tempPrintTimeDelay >= printTimeDelay)
                    {
                        PrintSingleVoxLetter(tempLetterIndex);
                        
                        

                        if (myTextCharArray[tempLetterIndex] == 'w' || myTextCharArray[tempLetterIndex] == 'u' || myTextCharArray[tempLetterIndex] == 'n' || myTextCharArray[tempLetterIndex] == 'v' || myTextCharArray[tempLetterIndex] == 'm')
                            tempXPosition += spacing * 1.2f;
                        else
                            tempXPosition += spacing;

                        // Resets our timer for the next letter
                        tempPrintTimeDelay = 0.0f;

                        // Plays a sound when we print a letter, but not a space
                        // if(myTextCharArray[tempLetterIndex] != ' ')
                        // 	GetComponent<AudioSource>().Play();

                        tempLetterIndex++;
                        lettersOnThisLine++;

                        if (lettersOnThisLine >= charactersPerLine)
                        {
                            tempYPosition -= lineSpacing;
                            lettersOnThisLine = 0;
                            tempXPosition = transform.position.x;
                            return;
                        }
                        return;
                    }
                }
                else if (tempLetterIndex >= myTextCharArray.Length)
                {
                    isFinishedPrinting = true;
                    printing = false;
                }
            }
			
		}
	}
	
	public Dictionary<char, GameObject> ParseAlphabetToMesh()
	{
		for(int i = 0; i < voxAlphabet.Length; i++)
		{
			voxAlphaDictionary.Add(base26Chars[i], voxAlphabet[i]);
		}
		return voxAlphaDictionary;
	}
	
	
	public void PrintSingleVoxLetter(int letterIndex)
	{
		GameObject alphaInstance = Instantiate(voxAlphaDictionary[myTextCharArray[letterIndex]], new Vector3(tempXPosition, tempYPosition, myPosition.z), Quaternion.Euler(gameObject.transform.rotation.x, 180, gameObject.transform.position.z)) as GameObject;
        alphaInstance.transform.parent = gameObject.transform;
        alphaInstance.transform.rotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y + 180f, 0f);
        alphaInstance.transform.localScale = new Vector3(-textSize, textSize, textSize);
        alphaInstance.transform.localPosition = new Vector3(tempXPosition, tempYPosition, myPosition.z);
        alphaInstance.SetActive(true);
        // alphaInstance.transform.localScale = new Vector3(-16f, 16f, 16f);

        if (textMaterial != null)
			alphaInstance.GetComponent<Renderer>().material = textMaterial;

        if (useShadows)
        {
            GameObject alphaInstanceShadow = Instantiate(voxAlphaDictionary[myTextCharArray[letterIndex]], new Vector3(tempXPosition, tempYPosition, myPosition.z), Quaternion.Euler(gameObject.transform.rotation.x, 180, gameObject.transform.position.z)) as GameObject;
            alphaInstanceShadow.transform.parent = alphaInstance.transform;
            alphaInstanceShadow.transform.rotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y +180f, 0f);
            alphaInstanceShadow.transform.localScale = Vector3.one;
            alphaInstanceShadow.transform.localPosition = new Vector3(shadowDistance,-shadowDistance, shadowDistance);
            alphaInstanceShadow.SetActive(true);
            alphaInstanceShadow.GetComponent<SpriteRenderer>().color = new Vector4(0f,0f,0f,0.35f);
            alphaInstanceShadow.GetComponent<SpriteRenderer>().sortingOrder = 4;
        }
	}
	
	public void PrintAllVoxText(string myText, Vector3 myPosition)
	{
		for(int j = 0; j < myTextCharArray.Length; j++)
		{
            PrintSingleVoxLetter(j);
			// GameObject alphaInstance = Instantiate(voxAlphaDictionary[myTextCharArray[j]], new Vector3(myPosition.x + alphaXOffset, myPosition.y, myPosition.z), Quaternion.Euler(gameObject.transform.rotation.x, 180, gameObject.transform.position.z)) as GameObject;
			// //GameObject alphaInstance = Instantiate(voxAlphaDictionary[myTextCharArray[j]], new Vector3(myPosition.x + alphaXOffset, myPosition.y, myPosition.z), transform.rotation) as GameObject;
			// 
			// alphaInstance.transform.parent = gameObject.transform;
			// alphaXOffset += 0.75f;
			// //alphaInstance.transform.localScale = new Vector3(textSize, textSize, textSize);
			// 
			// if(textMaterial != null)
			// 	alphaInstance.GetComponent<Renderer>().material = textMaterial;
			// //Debug.Log("Instantiated " + myTextCharArray[j].ToString());
		}
	}

    [ExecuteInEditMode]
    public class PrintAwake : MonoBehaviour
    {
        string myText;
        void Awake()
        {
            myText = GetComponent<VoxelAlphabet>().myText;
            GetComponent<VoxelAlphabet>().PrintAllVoxText(myText, transform.position);
        }

        void Update()
        {
            GetComponent<VoxelAlphabet>().PrintAllVoxText(myText, transform.position);
        }
    }
}
