using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject
{
    public EnemyData data;
    public int health;
    public int attack;
    public int sleepTurn;

    public EnemyObject(EnemyData _data)
    {
        data = _data;
        health = data.health;
        attack = data.attack;
        sleepTurn = 0;
    }

    public int GetAttack()
    {
        return (IsDead() || IsSleeping()) ? 0 : attack;
    }

    public bool IsSleeping() { return sleepTurn > 0; }

    public bool IsDead() { return health <= 0; }

    public override string ToString()
    {
        return string.Format("{0} - H: {1} - Sleep: {2} - IsDead: {3}", data.name, health, sleepTurn, IsDead());
    }
}

public class EnemyData
{
    public string name;
    public int health;
    public int attack;
    public int level;

    public string GetImagePath() { return string.Format("Enemy/{0}_{1}", level, name); }
}

public class EnemyDb
{
    public static Dictionary<string, EnemyData> ENEMY_DB = new Dictionary<string, EnemyData>()
    {
        {
            "egg",
            new EnemyData
            {
                name = "egg",
                health = 1,
                attack = 0,
                level = 0
            }
        },
        {
            "crow",
            new EnemyData
            {
                name = "crow",
                health = 5,
                attack = 2,
                level = 0
            }
        },
        {
            "wolf",
            new EnemyData
            {
                name = "wolf",
                health = 7,
                attack = 5,
                level = 1
            }
        },
        {
            "lion",
            new EnemyData
            {
                name = "lion",
                health = 11,
                attack = 3,
                level = 1
            }
        },
        {
            "wolf_leader",
            new EnemyData
            {
                name = "wolf_leader",
                health = 14,
                attack = 9,
                level = 2
            }
        },
        {
            "skeleton",
            new EnemyData
            {
                name = "skeleton",
                health = 25,
                attack = 5,
                level = 2
            }
        },
        {
            "ebony_knight",
            new EnemyData
            {
                name = "ebony_knight",
                health = 30,
                attack = 20,
                level = 3
            }
        },
        {
            "black_lion",
            new EnemyData
            {
                name = "black_lion",
                health = 50,
                attack = 12,
                level = 3
            }
        },
        
        {
            "dark_knight",
            new EnemyData
            {
                name = "dark_knight",
                health = 61,
                attack = 52,
                level = 4
            }
        },
        {
            "demon_lord",
            new EnemyData
            {
                name = "demon_lord",
                health = 150,
                attack = 22,
                level = 4
            }
        }
    };

    public static EnemyData GetRandom(int level)
    {
        switch(level)
        {
            case 0:
                return ENEMY_DB["crow"];
            case 1:
                return ENEMY_DB[RandomBool() ? "wolf" : "lion"];
            case 2:
                return ENEMY_DB[RandomBool() ? "wolf_leader" : "skeleton"];
            case 3:
                return ENEMY_DB[RandomBool() ? "black_lion" : "ebony_knight"];
            case 4:
                return ENEMY_DB[RandomBool() ? "demon_lord" : "dark_knight"];
            default:
                return ENEMY_DB["egg"];
        }
    }

    public static bool RandomBool()
    {
        return (int)(Random.Range(0, 2)) == 0;
    }
}
