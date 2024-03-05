using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootBehaiour: MonoBehaviour
{
    protected bool isPauseObject;

    private void OnEnable()
    {
        GameManager.Instance.onPauseGame += OnPauseGame;
    }

    private void OnDestroy()
    {
        GameManager.Instance.onPauseGame -= OnPauseGame;
    }

    protected virtual void OnPauseGame(bool isPause)
    {
        isPauseObject = isPause;
    }
}
