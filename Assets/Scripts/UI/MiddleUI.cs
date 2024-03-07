using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MiddleUI : Singleton<MiddleUI>
{
    [SerializeField] CanvasGroup panelGroup;
    [SerializeField] Image deadImage;
    [SerializeField] Image survivedImage;
    [SerializeField] Text timeText;

    void Start()
    {
        panelGroup.gameObject.SetActive(false);
    }
    private void Update()
    {
        TimeSpan span = TimeSpan.FromSeconds(GameManager.Instance.gameTime);
        timeText.text = span.ToString(@"mm\:ss");
    }

    public void OpenResult(bool isSurvived)
    {
        panelGroup.gameObject.SetActive(true);
        panelGroup.alpha = 0f;
        deadImage.enabled = !isSurvived;
        survivedImage.enabled = isSurvived;
        StartCoroutine(IEFadeIn());
    }

    private IEnumerator IEFadeIn()
    {
        yield return new WaitForSeconds(1f);
        
        GameManager.Instance.SwitchPause(true);
        AudioManager.Instance.PlaySe("lose");

        const float maxTime = 0.5f;
        float time = 0.0f;
        while (time < maxTime)
        {
            time = Mathf.Clamp(time + Time.deltaTime, 0.0f, maxTime);
            panelGroup.alpha = time / maxTime;
            yield return null;
        }
    }

}
