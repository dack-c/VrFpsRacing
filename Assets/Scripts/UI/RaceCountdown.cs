using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RaceCountdown : MonoBehaviour
{
    [SerializeField]
    private Text countdownText;
    [SerializeField]
    private AudioClip countClipHigh;
    [SerializeField]
    private AudioClip countClipLow;
    private AudioSource countAudioSource;

    private void Start()
    {
        countAudioSource = GetComponent<AudioSource>();
    }

    public void StartCountdown()
    {
        StopAllCoroutines();
        StartCoroutine(nameof(IE_StartCountdown));
    }

    private IEnumerator IE_StartCountdown()
    {
        countdownText.gameObject.SetActive(true);
        countAudioSource.clip = countClipLow;

        countAudioSource.Play();
        countdownText.text = "Ready..";
        yield return new WaitForSeconds(2f);

        int countdown = 3;
        while (countdown > 0)
        {
            countAudioSource.Play();
            countdownText.text = $"{countdown}";
            countdown--;
            yield return new WaitForSeconds(1f);
        }

        countAudioSource.clip = countClipHigh;
        countAudioSource.Play();
        countdownText.text = "Start";
        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);
    }
}
