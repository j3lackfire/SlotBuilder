using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReelConfig 
{
    public static List<string> REEL_0 = new List<string>();
    public static List<string> REEL_1 = new List<string>();
    public static List<string> REEL_2 = new List<string>();

    public static void Reset()
    {
        REEL_0.Clear();
        REEL_1.Clear();
        REEL_2.Clear();

        REEL_0.AddRange(new string[] {
            "sword_1", "sword_0", "sword_0",
            "staff_1", "staff_0", "staff_0",
            "gold_1", "gold_0", "gold_0"
        });

        REEL_1.AddRange(new string[] {
            "staff_1", "staff_0",
            "gold_1", "gold_0",
            "sword_1", "sword_0",
        });

        REEL_2.AddRange(new string[] {
            "gold_0", "sword_0", "staff_0",
        });
    }
};

