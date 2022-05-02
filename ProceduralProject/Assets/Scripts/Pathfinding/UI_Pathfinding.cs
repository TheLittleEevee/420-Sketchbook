using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Pathfinding : MonoBehaviour
{
    void OnGUI()
    {
        if (!MyGrid.singleton.isWin && !MyGrid.singleton.isLose)
        {
            GUI.Box(new Rect(10, 10, 120, 25), "Tower Health: " + MyGrid.singleton.towerHealth.ToString());
            GUI.Box(new Rect(10, 40, 90, 25), "Money: " + MyGrid.singleton.money.ToString());

            if (GUI.Button(new Rect(10, Screen.height - 40, 130, 30), "Delete Terrain = $0") || Input.GetKey(KeyCode.Alpha1))
            {
                MyGrid.singleton.activeCubeSwap = 0;
                MyGrid.singleton.price = 0;
            }
            if (GUI.Button(new Rect(150, Screen.height - 40, 130, 30), "Wall Terrain = $20") || Input.GetKey(KeyCode.Alpha2))
            {
                MyGrid.singleton.activeCubeSwap = 1;
                MyGrid.singleton.price = 20;
            }
            if (GUI.Button(new Rect(290, Screen.height - 40, 130, 30), "Water Terrain = $10") || Input.GetKey(KeyCode.Alpha3))
            {
                MyGrid.singleton.activeCubeSwap = 2;
                MyGrid.singleton.price = 10;
            }
            if (GUI.Button(new Rect(430, Screen.height - 40, 130, 30), "Lava Terrain = $50") || Input.GetKey(KeyCode.Alpha4))
            {
                MyGrid.singleton.activeCubeSwap = 3;
                MyGrid.singleton.price = 50;
            }
            if (GUI.Button(new Rect(570, Screen.height - 40, 130, 30), "Spike Terrain = $5") || Input.GetKey(KeyCode.Alpha5))
            {
                MyGrid.singleton.activeCubeSwap = 4;
                MyGrid.singleton.price = 5;
            }

            if (!MyGrid.singleton.inWave)
            {
                if (GUI.Button(new Rect(Screen.width - 100, 10, 90, 40), "Start Wave " + MyGrid.singleton.waveNum.ToString()))
                {
                    MyGrid.singleton.inWave = true;
                    MyGrid.singleton.isPaused = false;
                    MyGrid.singleton.pausesLeft = 2;
                    MyGrid.singleton.totalEnemiesInWave = 10 + (5 * (MyGrid.singleton.waveNum - 1));
                    MyGrid.singleton.enemyMaxHealth = 100 + (25 * (MyGrid.singleton.waveNum - 1));
                }
            }
            else
            {
                if (!MyGrid.singleton.isPaused && MyGrid.singleton.pausesLeft > 0)
                {
                    if (GUI.Button(new Rect(Screen.width - 100, 10, 80, 40), "PAUSE " + MyGrid.singleton.pausesLeft.ToString()))
                    {
                        MyGrid.singleton.pausesLeft--;
                        MyGrid.singleton.isPaused = true;
                    }
                }
                else if (MyGrid.singleton.pausesLeft >= 0)
                {
                    if (GUI.Button(new Rect(Screen.width - 100, 10, 80, 40), "RESUME"))
                    {
                        if (MyGrid.singleton.pausesLeft == 0) MyGrid.singleton.pausesLeft--;
                        MyGrid.singleton.isPaused = false;
                    }
                }
            }
        }
        else if (MyGrid.singleton.isWin)
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "You Win!");
        }
        else if (MyGrid.singleton.isLose)
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Game Over!");

        }
    }
}
