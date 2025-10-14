using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStart()
    {
        Time.timeScale = 1f;
    }
    public void Closs()
    {
        this.gameObject.SetActive(false);
    }
}
