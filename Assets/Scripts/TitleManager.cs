using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameValue gameValue;

    private void Start()
    {
        gameValue.Load();
    }

    public void OnSelectCharacter(int index)
    {
        gameValue.characterIndex = index;
        SceneManager.LoadScene("Main");
    }
}
