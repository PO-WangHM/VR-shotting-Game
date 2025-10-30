using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallHealth : MonoBehaviour
{
    public float WallHP = 100;
    public GameObject GameOverPanel;//结束面板展示
    public Text WallHPText;
    public GameObject playerObj;
    public GameObject WarnPanel;
    [Header("Audio Setting")]
    public AudioSource audioSource;//音效组件
    public AudioClip[] audioClips;//音效
    protected bool isPlayed = false;
    protected float currentHP = 0;
    protected float Coin;


    void Start()
    {
        currentHP = WallHP;
    }

    void Update()
    {
        ShowValue();
        getCoin();
        Die();
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        Debug.Log("Tag: " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit the wall! WallHP: " + WallHP);
            currentHP -= 20;
            FindObjectOfType<EnemySpawn>().EnemyDefeated();
            Destroy(collision.gameObject);
        }
    }
    void ShowValue()
    {
        WallHPText.text = "WallHP: " + currentHP + "/" + WallHP;
    }

    public void HPRecoverButton()
    {
        if (Coin >= 500)
        {
            if (currentHP != WallHP)
            {
                Coin = Coin - 500;
                audioSource.PlayOneShot(audioClips[1]);
                if (currentHP + 20 > WallHP)
                {
                    currentHP = WallHP ;
                }
                else
                {
                    currentHP += 20;
                }
            }
        }
        else
        {
            WarnPanel.gameObject.SetActive(true);
        }
    }

    void Die()
    {
        if (currentHP <= 0)
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

    public void getCoin()
    {
        OutputPlayerValue opv = playerObj.gameObject.GetComponent<OutputPlayerValue>();
        if (opv != null)
        {
           Coin =  opv.outputCoin();
        }
    }
}
