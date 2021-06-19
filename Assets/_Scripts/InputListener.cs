using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public enum GamePhase {
    READY,
    SPINNING,
    PLAYER,
    ENEMY
}

public class InputListener : MonoBehaviour
{
    public Reel r0;
    public Reel r1;
    public Reel r2;

    public ShopPanel shop;
    public ShopPanel spell;

    //Make this drag and drop from the editor, so I can see the main game all the time.
    public GameObject deckInfo;
    public GameObject helpPanel;

    private Player player;

    public GamePhase phase;

    private void Awake()
    {
        ReelConfig.Reset();

        r0 = GameObject.Find("Reel_0").GetComponent<Reel>();
        r1 = GameObject.Find("Reel_1").GetComponent<Reel>();
        r2 = GameObject.Find("Reel_2").GetComponent<Reel>();

        player = FindObjectOfType<Player>();
        shop = GameObject.Find("Shop").GetComponent<ShopPanel>();
        spell = GameObject.Find("Spell").GetComponent<ShopPanel>();

        phase = GamePhase.READY;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) { OnSpacePressed(); }
        if (Input.GetKeyDown(KeyCode.Escape)) { OnEscapePressed(); }
    }

    public void AdvanceNextPhase()
    {
        var previous = phase;
        if (phase == GamePhase.READY)
        {
            phase = GamePhase.SPINNING;
            spell.SetShopData(0);
            spell.UpdateShop();

            shop.SetShopData(0);
            shop.UpdateShop();

            r0.SpinReel(2f, null);
            r1.SpinReel(2.75f, null);
            r2.SpinReel(3.5f, () =>
            {
                Time.timeScale = 1f;
                //something.
                player.FetchData(new CardData[] {
                        r0.GetShowingCard().cardData,
                        r1.GetShowingCard().cardData,
                        r2.GetShowingCard().cardData
                    });
                shop.SetShopData(player.gold);
                spell.SetShopData(player.magic);
                AdvanceNextPhase();
            });
        } else if (phase == GamePhase.SPINNING)
        {
            phase = GamePhase.PLAYER;
        } else if (phase == GamePhase.PLAYER)
        {
            spell.SetShopData(0);
            phase = GamePhase.ENEMY;
        } else
        {
            phase = GamePhase.READY;
        }
        //Debug.LogFormat("Phase update: {0} -> {1}", previous, phase);

    }

    public void OnSpacePressed()
    {
        if (IsUIShowing())
        {
            OnEscapePressed();
        } else
        {
            if (phase == GamePhase.READY)
            {
                if (player.health > 0)
                {
                    AdvanceNextPhase();
                    FindObjectOfType<EnemyManager>().UpdateSleeping();
                    player.infoText.text = "SPINNING...";
                }
                else
                {
                    player.infoText.text = "YOU LOSED! PRESS ESC TO RESTART THE GAME!";
                }
            } else if (phase == GamePhase.SPINNING)
            {
                Time.timeScale = 8f;
            } else if (phase == GamePhase.PLAYER)
            {
                player.ResetPlayerData();
                FindObjectOfType<EnemyManager>().StartEnemyPhase();
            }
        }
        
    }

    public bool IsUIShowing()
    {
        return deckInfo.activeSelf || reelSelection.activeSelf || helpPanel.activeSelf;
    }

    public void OnEscapePressed()
    {
        if (IsUIShowing())
        {
            HideUI();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void HideUI()
    {
        deckInfo.gameObject.SetActive(false);
        reelSelection.gameObject.SetActive(false);
        helpPanel.gameObject.SetActive(false);
    }

    public void ToggleDeckInfo() {  
        if (IsUIShowing())
        {
            HideUI();
        } else
        {
            deckInfo.SetActive(true);
        }
    }

    public void ToggleHelpPanel() {
        if (IsUIShowing())
        {
            HideUI();
        }
        else
        {
            helpPanel.SetActive(true);
        }
    }

    #region REEL_SELECTION_PARTH
    public GameObject reelSelection;
    public VoidDelegateInt reelSelectedDelegate;

    public void ToggleReelSelection(VoidDelegateInt OnSelected)
    {
        if (IsUIShowing())
        {
            HideUI();
        } else
        {
            reelSelection.SetActive(true);
            reelSelectedDelegate = OnSelected;
        }
    }

    public void OnReelSelected(int reel)
    {
        reelSelectedDelegate?.Invoke(reel);
    }

    public TextMeshProUGUI FindCostText(string name)
    {
        return reelSelection.transform.Find(name).Find("CostText").GetComponent<TextMeshProUGUI>();
    }
    #endregion
}
