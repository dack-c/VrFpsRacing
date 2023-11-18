using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIAudio : MonoBehaviour, ISelectHandler, IPointerEnterHandler/*, IPointerClickHandler*/
{
    public AudioClip SFX_Click;
    public AudioClip SFX_HoverOver;

    private Button button;
    private Toggle toggle;
    private void Awake()
    {
        if (TryGetComponent(out button))
        {
            button.onClick.AddListener(() => Play(SFX_Click, gameObject.transform.position));
        }
        if (TryGetComponent(out toggle))
        {
            toggle.onValueChanged.AddListener((value) => Play(SFX_Click, gameObject.transform.position));
        }
    }

    public void Play(AudioClip audioClip, Vector3? position = null)
    {
        GameObject clipObj = new GameObject(audioClip.name, typeof(AudioDestroyer));
        AudioSource src = clipObj.AddComponent<AudioSource>();
        if (position.HasValue)
        {
            clipObj.transform.position = position.Value;
            src.spatialBlend = 1;
            src.rolloffMode = AudioRolloffMode.Linear;
            src.maxDistance = 50;
            src.dopplerLevel = 0;
        }
        src.clip = audioClip;
        //src.outputAudioMixerGroup = mixerTarget;
        src.Play();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!button || button.interactable)
            Play(SFX_HoverOver, gameObject.transform.position);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (eventData is PointerEventData) return;

        if (!button || button.interactable)
            Play(SFX_HoverOver, gameObject.transform.position);
    }
}
