using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    GameActor playerActor;

    //GOLD, attack and magic are the value gained this turn.
    //Total gold value is stored in the shop panel class
    public int attack;
    public int magic;
    public int gold;

    public int health;
    public int maxHealth;

    private string specialText = "";

    public TextMeshProUGUI infoText;

    private void Awake()
    {
        infoText = GameObject.Find("InfoText").GetComponent<TextMeshProUGUI>();
        playerActor = GameObject.Find("PlayerActor").GetComponent<GameActor>();
        maxHealth = 35;
        health = maxHealth;
    }

    private void Start()
    {
        UpdateActor();
    }

    public void ResetPlayerData()
    {
        specialText = "";
        attack = 0;
        magic = 0;
        gold = 0;
        playerActor.atkText.text = "0";
        infoText.text = "ENEMY PHASE...";
    }

    public void UpdateData()
    {
        specialText = string.IsNullOrEmpty(specialText) ? "SPIN RESULT: " : specialText;
        infoText.text = specialText + string.Format("ATK: {0} - MAGIC: {1} - GOLD: {2}", attack, magic, gold);
        playerActor.atkText.text = attack.ToString();
    }

    public void UpdateActor()
    {
        playerActor.healthText.text = health.ToString() + "/" + maxHealth.ToString();
        playerActor.atkText.text = attack.ToString();
    }

    public void FetchData(CardData[] cards)
    {
        specialText = "";
        int atkCard = 0;
        int magicCard = 0;
        int goldCard = 0;
        attack = 0;
        magic = 0;
        gold = 0;
        for (int i = 0; i < cards.Length; i ++)
        {
            HandleCard(cards[i]);
            if (cards[i].type == CardType.SWORD)
            {
                atkCard++;
            } else if (cards[i].type == CardType.STAFF)
            {
                magicCard++;
            } else if (cards[i].type == CardType.GOLD)
            {
                goldCard++;
            }
        }
        attack *= (int)Mathf.Pow(2, atkCard - 1);
        magic *= (int)Mathf.Pow(2, magicCard - 1);
        gold *= (int)Mathf.Pow(2, goldCard - 1);

        if (atkCard > 1 || magicCard > 1 || goldCard > 1)
        {
            if (atkCard > 1)
            {
                specialText = "ATK X" + (atkCard == 2 ? "2" : "4");
            } else if (magicCard > 1)
            {
                specialText = "MAGIC X" + (magicCard == 2 ? "2" : "4");
            }
            else if (goldCard > 1)
            {
                specialText = "GOLD X" + (goldCard == 2 ? "2" : "4");
            }
            specialText += " - ";
        }
        UpdateData();
    }

    private void HandleCard(CardData c)
    {
        switch (c.type)
        {
            case CardType.SWORD:
                attack += c.data;
                break;
            case CardType.STAFF:
                magic += c.data;
                break;
            case CardType.GOLD:
                gold += c.data;
                break;
        }
    }
}
