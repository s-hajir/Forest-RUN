using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStateManager {

    public static bool gamePlayActive = false;
    public static int lifes = 2;
    public static float speed = 40;
    public static int coins = 0;

    public static void deleteOneLife()
    {
        if (lifes>0)
        {
            lifes--;
        }
    }
}
