using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Value", menuName = "Local/GameValue")]
public class GameValue : ScriptableObject
{
    [Header("Game")]
    public int characterIndex;

    [Header("Record")]
    public int clearCount;
    public int defeatCount;
    public int killCount;


    public void Load()
    {
        clearCount = PlayerPrefs.GetInt(nameof(clearCount), 0);
        defeatCount = PlayerPrefs.GetInt(nameof(defeatCount), 0);
        killCount = PlayerPrefs.GetInt(nameof(killCount), 0);
    }
    public void Save()
    {
        PlayerPrefs.SetInt(nameof(clearCount), clearCount);
        PlayerPrefs.SetInt(nameof(defeatCount), defeatCount);
        PlayerPrefs.SetInt(nameof(killCount), killCount);
    }
   
}
