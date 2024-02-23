using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairItemController : MonoBehaviour
{
    public GameObject playableCar;

    private Damageable damageable;
    private float maxHealth;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        if(playableCar == null)
        {
            Debug.LogError("repairItem의 차 레퍼런스가 null입니다! 할당해주세요.");
        }
        else
        {
            damageable = playableCar.GetComponent<Damageable>();
            maxHealth = damageable.Health;
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void StartRepair()
    {
        damageable.Health = maxHealth;

        //수리 사운드 두 번 실행
        audioSource.Play();
        Invoke(nameof(PlaySoundAgain), 0.5f);
    }

    private void PlaySoundAgain()
    {
        audioSource.Stop();
        audioSource.Play();
    }
}
