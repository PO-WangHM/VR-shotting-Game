using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHealth : MonoBehaviour
{
    public float WallHP = 100;
    public GameObject GameOverPanel;//�������չʾ
    [Header("Audio Setting")]
    public AudioSource audioSource;//��Ч���
    public AudioClip[] audioClips;//��Ч
    protected bool isPlayed = false;

    void Start()
    {
        
    }

    void Update()
    {
        Die();
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        Debug.Log("Tag: " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit the wall! WallHP: " + WallHP);
            WallHP -= 100;
            FindObjectOfType<EnemySpawn>().EnemyDefeated();
            Destroy(collision.gameObject);
        }
    }
    void Die()
    {
        if (WallHP <= 0)
        {
            if(!isPlayed)
            {
                audioSource.PlayOneShot(audioClips[0]);
                isPlayed = true;
            }
            
            Time.timeScale = 0f;
            GameOverPanel.gameObject.SetActive(true);
        }
    }
}
