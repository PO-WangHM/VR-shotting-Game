using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHealth : MonoBehaviour
{
    public float WallHP = 100;
    public GameObject GameOverPanel;//结束面板展示
    [Header("Audio Setting")]
    public AudioSource audioSource;//音效组件
    public AudioClip[] audioClips;//音效

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
            audioSource.PlayOneShot(audioClips[0]);
            Time.timeScale = 0f;
            GameOverPanel.gameObject.SetActive(true);
        }
    }
}
