using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void VoidDelegate();
public delegate void VoidDelegateInt(int i);
public enum SpinPhase
{
    OFF,
    START,
    FASTEST,
    SLOWING
}

public class Reel : MonoBehaviour
{
    public static float REEL_START = 300f;
    public static float REEL_END = -300f;

    public static float ELEMENT_SIZE = 200f;

    public static int MAX_SPINNING_SPEED = 8;

    public VoidDelegate onSpinningCompleted;
    
    private List<string> reelsData;

    private int trackingIndex;
    public SpinPhase phase;
    public Card[] cards;

    private float spinSpeed;
    private float spinTime;
    private float spinAcceleration;
    private float spinDuration;
    private int reelIndex;
    private RectTransform rTransform;

    private void Awake()
    {
        rTransform = GetComponent<RectTransform>();
        phase = SpinPhase.OFF;
        cards = GetComponentsInChildren<Card>();
        trackingIndex = 0;

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
    }

    private void Start()
    {
        UpdateCardData();
        rTransform.anchoredPosition = new Vector2(rTransform.anchoredPosition.x, REEL_START);
    }

    private void Update()
    {
        if (phase != SpinPhase.OFF)
        {
            float yPos = rTransform.anchoredPosition.y - spinSpeed * ELEMENT_SIZE * Time.deltaTime;
            if (yPos < REEL_END) 
            { 
                yPos += (REEL_START - REEL_END);
                trackingIndex--;
                if (trackingIndex < 0)
                {
                    trackingIndex = reelsData.Count - 1;
                }
                UpdateCardData();
            }
            rTransform.anchoredPosition = new Vector2(rTransform.anchoredPosition.x, yPos); 
            
            spinTime += Time.deltaTime;
            switch(phase)
            {
                case SpinPhase.START:
                    spinSpeed += spinAcceleration * Time.deltaTime;
                    if (spinTime >= spinDuration / 3)
                    {
                        phase = SpinPhase.FASTEST;
                    }
                    break;
                case SpinPhase.FASTEST:
                    if (spinTime >= spinDuration * 2 / 3)
                    {
                        phase = SpinPhase.SLOWING;
                    }
                    break;
                case SpinPhase.SLOWING:
                    spinSpeed -= spinAcceleration * Time.deltaTime;
                    if (spinSpeed < 0) { spinSpeed = 0f; }
                    if (spinTime >= spinDuration)
                    {
                        phase = SpinPhase.OFF;
                        float y;
                        if (yPos > 215f && yPos  < 300f)
                        {
                            reelIndex = 3;
                            y = 300f;
                        }
                        else if (yPos > 15f && yPos < 215f)
                        {
                            reelIndex = 2;
                            y = 100f;
                        } else if (yPos > -185f && yPos < 15f)
                        {
                            reelIndex = 1;
                            y = -100f;
                        } else 
                        {
                            reelIndex = 0;
                            y = -300f;
                        }
                        rTransform.anchoredPosition =
                            new Vector2(rTransform.anchoredPosition.x, y);
                        onSpinningCompleted?.Invoke();
                    }
                    break;
            }
            
        }
    }

    public void SpinReel(float duration, VoidDelegate onCompleted)
    {
        phase = SpinPhase.START;
        spinSpeed = 0f;
        spinTime = 0f;
        spinDuration = duration;
        spinAcceleration = MAX_SPINNING_SPEED / (duration / 3) * Random.Range(1f, 1.33f);
        onSpinningCompleted = onCompleted;
    }

    public Card GetShowingCard()
    {
        return cards[reelIndex];
    }

    public void UpdateCardData() 
    {
        for (int i = 0; i < cards.Length; i ++)
        {
            int index = cards.Length - i - 1;
            cards[index].SetData(reelsData[trackingIndex]);
            trackingIndex++;
            if (trackingIndex >= reelsData.Count)
            {
                trackingIndex = 0;
            }
        }
    }
}
