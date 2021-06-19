using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
    SWORD,
    STAFF,
    GOLD,
    MAGIC
}

[System.Serializable]
public class CardData
{
    public string name;
    public CardType type;
    public int level; //from 0 to 4
    public int data;

    public int GetCardCost() { return (int)(level * Mathf.Pow(2, level)); }

    public override string ToString()
    {
        return string.Format("{0} - {1} - {2} - {3}", name, type, level, data);
    }
}

public class CardDb
{
    public static Dictionary<string, CardData> CARD_DB = new Dictionary<string, CardData>
    {
        { 
            "sword_0",
            new CardData
            {
                name = "sword_0",
                type= CardType.SWORD,
                level= 0,
                data= 2
            }
        },
        {
            "sword_1",
            new CardData
            {
                name = "sword_1",
                type= CardType.SWORD,
                level= 1,
                data= 5
            }
        },
        {
            "sword_2",
            new CardData
            {
                name = "sword_2",
                type= CardType.SWORD,
                level= 2,
                data= 15
            }
        },
        {
            "sword_3",
            new CardData
            {
                name = "sword_3",
                type= CardType.SWORD,
                level= 3,
                data= 48
            }
        },
        {
            "sword_4",
            new CardData
            {
                name = "sword_4",
                type= CardType.SWORD,
                level= 4,
                data= 120
            }
        },

        //Staff
        {
            "staff_0",
            new CardData
            {
                name = "staff_0",
                type= CardType.STAFF,
                level= 0,
                data= 3
            }
        },
        {
            "staff_1",
            new CardData
            {
                name = "staff_1",
                type= CardType.STAFF,
                level= 1,
                data= 7
            }
        },
        {
            "staff_2",
            new CardData
            {
                name = "staff_2",
                type= CardType.STAFF,
                level= 2,
                data= 18
            }
        },
        {
            "staff_3",
            new CardData
            {
                name = "staff_3",
                type= CardType.STAFF,
                level= 3,
                data= 42
            }
        },
        {
            "staff_4",
            new CardData
            {
                name = "staff_4",
                type= CardType.STAFF,
                level= 4,
                data= 91
            }
        },

        //GOLD -> x4 -> x3 -> x2 -> x1.5
        {
            "gold_0",
            new CardData
            {
                name = "gold_0",
                type= CardType.GOLD,
                level= 0,
                data= 1
            }
        },
        {
            "gold_1",
            new CardData
            {
                name = "gold_1",
                type= CardType.GOLD,
                level= 1,
                data= 4
            }
        },
        {
            "gold_2",
            new CardData
            {
                name = "gold_2",
                type= CardType.GOLD,
                level= 2,
                data= 12
            }
        },
        {
            "gold_3",
            new CardData
            {
                name = "gold_3",
                type= CardType.GOLD,
                level= 3,
                data= 24
            }
        },
        {
            "gold_4",
            new CardData
            {
                name = "gold_4",
                type= CardType.GOLD,
                level= 4,
                data= 36
            }
        },
    };

    public static CardData GetRandom(int minLevel, int maxLevel, string[] excludeList)
    {
        List<CardData> returnList = new List<CardData>();
        foreach (var pair in CARD_DB)
        {
            if (Array.IndexOf(excludeList, pair.Key) != -1)
            {
                continue;
            } 
            if (pair.Value.level >= minLevel && pair.Value.level <= maxLevel)
            {
                returnList.Add(pair.Value);
            }
        }
        return returnList[UnityEngine.Random.Range(0, returnList.Count)];
    }

    public static CardData GetRandom()
    {
        int index = UnityEngine.Random.Range(0, CARD_DB.Count);
        int count = 0;
        foreach (var pair in CARD_DB)
        {
            if (count == index)
            {
                return pair.Value;
            } else
            {
                count++;
            }
        }
        return null;
    }
}
