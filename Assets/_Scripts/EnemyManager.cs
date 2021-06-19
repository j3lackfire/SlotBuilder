using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    public static int[] SPAWN_ORDER = new int[] {
        0, 0, 1, 1, 0, 1, 2, 1, 0, 3, 2, 1, 0, 3, 1, 0, 2, 4,
        //1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
    };

    private int spawnIndex = 0;
    
    private Player player;
    private InputListener input;

    private GameActor[] enemies;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        input = FindObjectOfType<InputListener>();

        enemies = new GameActor[2];
        enemies[0] = GameObject.Find("Enemy_0").GetComponent<GameActor>();
        enemies[1] = GameObject.Find("Enemy_1").GetComponent<GameActor>();
    }

    private void Start()
    {
        enemies[0].SetData(GetNext());
        enemies[1].SetData(GetNext());
    }

    private EnemyObject GetNext()
    {
        if (spawnIndex < SPAWN_ORDER.Length)
        {
            EnemyData d = EnemyDb.GetRandom(SPAWN_ORDER[spawnIndex]);
            EnemyObject e = new EnemyObject(d);
            spawnIndex++;
            return e;
        } else
        {
            return null;
        }
    }

    public void OnEnemyDie(GameActor e)
    {
        var nextEnemy = GetNext();
        if (nextEnemy == null)
        {
            e.gameObject.SetActive(false);
            if(enemies[0].enemyObject.IsDead() && enemies[0].enemyObject.IsDead())
            {
                player.infoText.text = "All enemies die! You win the game! Press ESC to retry!";
            } 
        } else
        {
            e.SetData(nextEnemy);
        }
    }

    public void UpdateSleeping()
    {
        foreach(var e in enemies)
        {
            if (e.enemyObject.IsSleeping())
            {
                e.enemyObject.sleepTurn--;
                if (!e.enemyObject.IsSleeping())
                {
                    e.SetRealData();
                } else
                {
                    e.healthText.text += "?";
                    e.atkText.text += "?";
                }
            }
        }
    }

    public void StartEnemyPhase()
    {
        input.AdvanceNextPhase();

        int totalDmg = enemies[0].enemyObject.GetAttack() + enemies[1].enemyObject.GetAttack();
        player.infoText.text = string.Format("The enemies attack for {0} damage!", totalDmg);
        player.health -= totalDmg;
        player.UpdateActor();
        input.AdvanceNextPhase();
    }
}
