using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameValue gameValue;
    [SerializeField] FollowCamera followCam;
    [SerializeField] Spawner spawner;
    [SerializeField] Player[] players;

    public float gameTime;
    public int gameLevel;

    bool isGameClear;
    bool isPauseGame;

    IEnumerator Start()
    {
        gameTime = 0f;
        AudioManager.Instance.PlayBGM();

        // 캐릭터를 생성한다.
        Player player = Instantiate(players[gameValue.characterIndex]);
        player.transform.position = Vector3.zero;
        player.Setup();
        switch(gameValue.characterIndex)
        {
            case 0:
                player.AddItem(ItemManager.Instance.GetItem("ITWE0001"));
                break;
            case 1:
                player.AddItem(ItemManager.Instance.GetItem("ITWE0003"));
                break;
        }

        yield return null;
        followCam.Setup(player.transform);
        spawner.SwitchSpawner(true);
    }


    private void Update()
    {
        if (isPauseGame)
            return;

        gameTime += Time.deltaTime;
        gameLevel = (int)(gameTime / MAX_GAME_TIME / MAX_LEVEL);
        if(!isGameClear && gameTime >= MAX_GAME_TIME)
            isGameClear = true;
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
    public void GoTitle()
    {
        SceneManager.LoadScene("Title");
    }



    public event Action<bool> onPauseGame;      // 일시정지 이벤트.

    public const float MAX_GAME_TIME = 15f;
    public const int MAX_LEVEL = 30;
}
