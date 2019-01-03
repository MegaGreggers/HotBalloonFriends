using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public int currentScore = 0;
    public GameObject onesDigit;
    public GameObject tensDigit;
    public Sprite[] numberSprites;
    int ones_digit = 0;
    int tens_digit = 0;

    private void Start()
    {
        onesDigit.GetComponent<SpriteRenderer>().sprite = numberSprites[0];
        tensDigit.GetComponent<SpriteRenderer>().sprite = numberSprites[0];
    }

    public void UpdateCounter(int score)
    {
        currentScore += score;
        GetCounterInfo(currentScore);
        RefreshCounter(ones_digit, tens_digit);
    }

    void GetCounterInfo(int score)
    {
        ones_digit = score % 10;

        if (score >= 10)
            tens_digit = (int)Mathf.Floor(score / 10);
        else
            tens_digit = 0;
    }

    void RefreshCounter(int ones, int tens)
    {
        for (int i = 0; i < numberSprites.Length; i++)
        {
            if (i == ones)
                onesDigit.GetComponent<SpriteRenderer>().sprite = numberSprites[i];
            if (i == tens)
                tensDigit.GetComponent<SpriteRenderer>().sprite = numberSprites[i];
        }
    }
}
