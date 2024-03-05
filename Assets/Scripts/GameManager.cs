using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public event Action<bool> onPauseGame;   // 일시정지 이벤트.
    private bool isPauseGame;                // 일시정지 여부.

    private void SwitchPause()
    {
        isPauseGame = !isPauseGame;
        onPauseGame?.Invoke(isPauseGame);
    }
    public void SwitchPauseForce(bool isPause)
    {
        isPauseGame = !isPause;
        SwitchPause();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            SwitchPause();
    }
}
