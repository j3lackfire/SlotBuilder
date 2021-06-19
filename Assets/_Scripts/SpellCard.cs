using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellCard : MonoBehaviour
{
    private TextMeshProUGUI data;
    private InputListener input;
    private Player player;

    public bool isClicked;
    public bool isActived;

    private int cleaningCost = 1;

    private void Awake()
    {
        input = FindObjectOfType<InputListener>();
        player = FindObjectOfType<Player>(); 
        data = transform.Find("data").GetComponent<TextMeshProUGUI>();
        GetComponent<Button>().onClick.AddListener(OnClicked);
    
        UpdateData();
        isClicked = false;
        isActived = false;
    }

    public void OnClicked()
    {
        if (isActived)
        {
            input.spell.infoText.text = "You can only cast a spell once per turn!";
            return;
        }
        if (isClicked)
        {
            if (player.magic == 0)
            {
                input.spell.infoText.text = "You don't have any MAGIC!";
            }
            isActived = true;
            switch (gameObject.name)
            {
                case "attack":
                    player.attack += Mathf.CeilToInt(player.magic * 1.5f);
                    player.magic = 0;
                    player.UpdateActor();
                    input.spell.SetShopData(0);
                    break;
                case "clean:":
                default:
                    isActived = false;
                    if (player.magic < cleaningCost)
                    {
                        input.spell.infoText.text = "You don't have enough MAGIC!";
                    } else
                    {
                        input.spell.infoText.text = "Choose a card to remove from your deck!";

                        input.FindCostText("Reel_0").text = "MAGIC:\n" + cleaningCost.ToString();
                        input.FindCostText("Reel_1").text = "MAGIC:\n" + (cleaningCost*2).ToString();
                        input.FindCostText("Reel_2").text = "MAGIC:\n" + (cleaningCost*3).ToString(); 
                        
                        input.ToggleReelSelection((int i) =>
                        {
                            int trueCost = cleaningCost * (i + 1);
                            if (player.magic < trueCost)
                            {
                                input.spell.infoText.text = "Not enough MAGIC! Removing from MIDDLE and LAST reel cost more!";
                                isActived = false;
                            }
                            else
                            {
                                input.spell.infoText.text = "Card removed!";
                                player.magic -= trueCost;
                                input.spell.SetShopData(player.magic);
                                input.ToggleReelSelection(null);

                                if (i == 0) { ReelConfig.REEL_0.Remove(input.r0.GetShowingCard().cardData.name); }
                                else if (i == 1) { ReelConfig.REEL_1.Remove(input.r1.GetShowingCard().cardData.name); }
                                else if (i == 2) { ReelConfig.REEL_2.Remove(input.r2.GetShowingCard().cardData.name); }
                                else { Debug.LogError("Invalid reel selectgion: " + i); }

                                cleaningCost += 1;
                                isActived = true;
                                UpdateData();
                            }
                        });
                    }
                    break;
                case "heal":
                    player.health = Mathf.Min(player.maxHealth, Mathf.CeilToInt(player.magic * 2.5f) + player.health);
                    player.magic = 0;
                    player.UpdateActor();
                    input.spell.SetShopData(0);
                    break;
                case "max_life":
                    player.maxHealth += player.magic;
                    player.magic = 0;
                    player.UpdateActor();
                    input.spell.SetShopData(0); 
                    break;
            }
        } else
        {
            foreach(var card in FindObjectsOfType<SpellCard>())
            {
                card.isClicked = false;
            }
            isClicked = true;
            string helpText = "";
            switch (gameObject.name)
            {
                case "attack":
                    helpText = "Spend all your MAGIC. Your attack is increased by 150% of your MAGIC.";
                    break;
                case "heal":
                    helpText = "Spend all your MAGIC. Heal yourself by 250% of your MAGIC.";
                    break;
                case "max_life":
                    helpText = "Spend all your MAGIC.Increase max health by 100% of your MAGIC.";
                    break;
                case "clean:":
                default:
                    helpText = "Remove the currently showing card from your deck! The cost of cleaning is increased indefinitely!";
                    OnClicked();
                    break;
            }
            input.spell.infoText.text = helpText;
        }
        
    }

    public void UpdateData()
    {
        switch (gameObject.name)
        {
            case "attack":
                data.text = player.magic == 0 ? "??" : Mathf.CeilToInt(player.magic * 1.5f).ToString();
                break;
            case "heal":
                data.text = player.magic == 0 ? "??" : Mathf.CeilToInt(player.magic * 2.5f).ToString();
                break;
            case "max_life":
                data.text = player.magic == 0 ? "??" : player.magic.ToString();
                break;
            case "clean:":
            default:
                data.text = cleaningCost.ToString();
                break;
        }
    }
}
