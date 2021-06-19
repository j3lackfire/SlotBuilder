using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelInfo : MonoBehaviour
{
    private List<string> reelsData;
    private Card[] cards;

    private void Awake()
    {
        switch (gameObject.name)
        {
            case "Reel_0":
                reelsData = ReelConfig.REEL_0;
                break;
            case "Reel_1":
                reelsData = ReelConfig.REEL_1;
                break;
            case "Reel_2":
                reelsData = ReelConfig.REEL_2;
                break;
            default:
                Debug.LogError("Invalid reel name: " + gameObject.name);
                break;
        }
        cards = GetComponentsInChildren<Card>();
    }

    private void OnEnable()
    {
        ShowReels();
    }

    public void ShowReels()
    {
        StartCoroutine(ShowReelsCoroutine());
    }
    
    private IEnumerator ShowReelsCoroutine()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < cards.Length; i++)
        {
            if (i < reelsData.Count)
            {
                cards[i].gameObject.SetActive(true);
                cards[i].SetData(reelsData[i]);
            }
            else
            {
                cards[i].gameObject.SetActive(false);
            }
        }
    }
}
