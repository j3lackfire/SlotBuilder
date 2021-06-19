using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    [SerializeField]
    public CardData cardData;

    public Image background;
    public Image main;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI dataText;

    public void Awake()
    {
        background = GetComponent<Image>();
        main = transform.Find("main").GetComponent<Image>();
        levelText = transform.Find("level").GetComponent<TextMeshProUGUI>();
        dataText = transform.Find("data").GetComponent<TextMeshProUGUI>();
    }

    public void SetData(CardData _cardData)
    {
        cardData = _cardData;
        background.sprite = Resources.Load<Sprite>("Frames/frame_" + cardData.level);
        main.sprite = Resources.Load<Sprite>("Card/" + cardData.name);
        levelText.text = "LV." + (cardData.level + 1).ToString();
        string prefix = cardData.type == 
            CardType.SWORD ? "ATK: " : 
            cardData.type == CardType.STAFF ? "MAGIC: " : 
            "GOLD: ";
        dataText.text = prefix + cardData.data;
    }

    public void SetData(string id) { SetData(CardDb.CARD_DB[id]);}

    public void SetShopData(CardData _cardData)
    {
        cardData = _cardData;
        background.sprite = Resources.Load<Sprite>("Frames/frame_" + cardData.level);
        main.sprite = Resources.Load<Sprite>("Card/" + cardData.name);
        levelText.text = "LV." + (cardData.level + 1).ToString();
        dataText.text = cardData.GetCardCost().ToString();
    }

    public void SetShopData(string id) { SetShopData(CardDb.CARD_DB[id]); }

    public void DoEffect()
    {
        Debug.Log("Card: " + dataText.text);
    }
}
