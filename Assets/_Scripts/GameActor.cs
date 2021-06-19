using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameActor : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI atkText;

    private Player player;
    private InputListener inputListener;

    public EnemyObject enemyObject;
    private Image logo;
    private bool isPlayer;

    private void Awake()
    {
        inputListener = FindObjectOfType<InputListener>();
        healthText = transform.Find("HealthInfo").GetComponentInChildren<TextMeshProUGUI>();
        atkText = transform.Find("AttackInfo").GetComponentInChildren<TextMeshProUGUI>();

        player = FindObjectOfType<Player>();
        isPlayer = gameObject.name == "PlayerActor";

        logo = transform.Find("Logo").GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(OnActorPressed);
    }

    public void SetData(EnemyObject e, bool sleeping = true)
    {
        enemyObject = e;
        if (sleeping)
        {
            var egg = EnemyDb.GetRandom(-1);
            logo.sprite = Resources.Load<Sprite>(egg.GetImagePath());
            //0 level enemy (bird) will just sleep indefinitely
            enemyObject.sleepTurn = enemyObject.data.level <= 1 ? 1 : enemyObject.data.level;
            healthText.text = "??";
            atkText.text = "??";
        } else
        {
            SetRealData();
        }
    }

    public void SetRealData()
    {
        logo.sprite = Resources.Load<Sprite>(enemyObject.data.GetImagePath());
        healthText.text = enemyObject.data.health.ToString();
        atkText.text = enemyObject.data.attack.ToString();
    }


    public void UpdateText(int health, int attack)
    {
        healthText.text = health.ToString();
        atkText.text = attack.ToString();
    }

    private void OnActorPressed()
    {
        if (isPlayer)
        {
            inputListener.OnSpacePressed();
        } else
        {
            if (inputListener.phase != GamePhase.PLAYER)
            {
                return;
            } else
            {
                if (player.attack > 0)
                {
                    if (enemyObject.IsSleeping())
                    {
                        player.infoText.text = "You can't attack a sleeping enemy!";
                    }
                    else
                    {
                        if (player.attack >= enemyObject.health)
                        {
                            player.attack -= enemyObject.health;
                            enemyObject.health = 0;
                            FindObjectOfType<EnemyManager>().OnEnemyDie(this);
                        }
                        else
                        {
                            enemyObject.health -= player.attack;
                            player.attack = 0;
                            UpdateText(enemyObject.health, enemyObject.attack);
                        }
                        player.UpdateActor(); 
                    }
                }
            }
        }
    }

}
