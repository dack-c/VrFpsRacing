using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairItemController : MonoBehaviour
{
    public GameObject player;
    public float maxHealth;

    private Damageable damageable;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
    }

    public void StartRepair()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;

        damageable = player.transform.parent.GetComponent<Damageable>();
        audioSource = GetComponent<AudioSource>();

        damageable.Health = maxHealth;

        //수리 사운드 두 번 실행
        audioSource.Play();
        Invoke(nameof(PlaySoundAgain), 0.5f);
    }

    private void PlaySoundAgain()
    {
        audioSource.Stop();
        audioSource.Play();

        GameManager.I.PlayerItemController.CleanCurrentSlot();
        Destroy(gameObject, audioSource.clip.length);
    }
}
