using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public event Action<bool> onPauseGame;      // �Ͻ����� �̺�Ʈ.
    
    public float gameTime;
    const float MAX_GAME_TIME = 600f;

    bool isGameClear;
    bool isPauseGame;

    void Start()
    {
        gameTime = 0f;
        AudioManager.Instance.PlayBGM();
    }
    private void Update()
    {
        if (isPauseGame)
            return;

        gameTime += Time.deltaTime;
        if(!isGameClear && gameTime >= MAX_GAME_TIME)
        {
            isGameClear = true;
            // ��� ����..?
        }
    }

    public void OnDeadPlayer()
    {
        AudioManager.Instance.PlayBGM(AUDIO_STATE.STOP);
        MiddleUI.Instance.OpenResult(isGameClear);
    }
    public void SwitchPause(bool isPause)
    {
        isPauseGame = isPause;
        onPauseGame?.Invoke(isPause);
    }
}
