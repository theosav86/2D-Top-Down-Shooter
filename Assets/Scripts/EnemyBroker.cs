﻿using System;
public class EnemyBroker
{
    public static event Action<int, int> EnemyKilled;

    public static void CallEnemyKilled(int pointValue, int scrapValue)
    {
        if(EnemyKilled != null)
        {
            EnemyKilled(pointValue, scrapValue);
        }
    }
}
