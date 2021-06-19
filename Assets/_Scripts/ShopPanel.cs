using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopPanel : MonoBehaviour
{
    public int data;
    
    public bool isGoldShop;

    private InputListener inputListener;
    private string goldMagicText;
    private string goldMagicInfo;

    private Card[] cards;
    private List<Card> selectedCards = new List<Card>();

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI infoText;

    private void Awake()
    {
        if (gameObject.name == "Shop") { isGoldShop = true; }
        else if (gameObject.name == "Spell") { isGoldShop = false; } 
        else { Debug.LogError("Shop naming invalid."); }

        goldMagicText = isGoldShop ? "GOLD" : "MAGIC";
        goldMagicInfo = isGoldShop ? "Unspent GOLD will be saved." : "Any MAGIC not cast will be removed.";

        inputListener = FindObjectOfType<InputListener>();
        cards = GetComponentsInChildren<Card>();
        titleText = transform.Find("Title").GetComponent<TextMeshProUGUI>();
        infoText = transform.Find("Info").GetComponent<TextMeshProUGUI>();
        data = 0;
    }

    private void Start()
    {
        UpdateShop();
    }

    public void UpdateShop()
    {
        selectedCards.Clear();
        if (isGoldShop)
        {
            List<string> exclude = new List<string>();
            foreach (var card in cards)
            {
                exclude.Add(card.cardData.name);
            }
            foreach (var card in cards)
            {
                var data = CardDb.GetRandom(1, 4, exclude.ToArray());
                exclude.Add(data.name);
                card.SetShopData(data);
            }
        } else
        {
            foreach (var card in FindObjectsOfType<SpellCard>())
            {
                card.isClicked = false;
            }
        }
    }

    public void SetShopData(int value)
    {
        if (isGoldShop)
        {
            data += value;
        } else
        {
            data = value;
            foreach (var card in FindObjectsOfType<SpellCard>())
            {
                card.isClicked = false;
                card.UpdateData();
                if (inputListener.phase == GamePhase.SPINNING)
                {
                    card.isActived = false;
                }
            }
        }
        titleText.text = goldMagicText + ": "  + data.ToString();
        infoText.text = goldMagicInfo;
    }

    public void OnShopButtonPressed(GameObject button)
    {
        if (inputListener.IsUIShowing())
        {
            inputListener.OnEscapePressed();
        } else
        {
            Card card = button.GetComponent<Card>();

            if (isGoldShop)
            {
                if (data < card.cardData.GetCardCost() || selectedCards.IndexOf(card) != -1)
                {
                    
                    infoText.text = data < card.cardData.GetCardCost() ? 
                        "Not enough GOLD!" : "You already bought this card this turn!";
                }
                else
                {
                    infoText.text = "Choose a reel to add your card!";
                    
                    inputListener.FindCostText("Reel_0").text = "GOLD:\n" + card.cardData.GetCardCost().ToString();
                    inputListener.FindCostText("Reel_1").text = "GOLD:\n" + (card.cardData.GetCardCost() * 2).ToString();
                    inputListener.FindCostText("Reel_2").text = "GOLD:\n" + (card.cardData.GetCardCost() * 3).ToString(); 
                    
                    inputListener.ToggleReelSelection((int i) =>
                    {
                        int costMultiplier = i + 1;
                        int trueCost = costMultiplier * card.cardData.GetCardCost();
                        if (data < trueCost)
                        {
                            infoText.text = "Not enough GOLD! Adding to middle reel and last reel cost more!";
                        }
                        else
                        {
                            infoText.text = "Thanks for the GOLD!";
                            SetShopData(-trueCost);
                            selectedCards.Add(card);
                            inputListener.ToggleReelSelection(null);

                            if (i == 0) { ReelConfig.REEL_0.Add(card.cardData.name); }
                            else if (i == 1) { ReelConfig.REEL_1.Add(card.cardData.name); }
                            else if (i == 2) { ReelConfig.REEL_2.Add(card.cardData.name); }
                            else { Debug.LogError("Invalid reel selectgion: " + i); }
                        }
                    });
                }
            }
        }
    }
}
