using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    void Start()
    {
        if ((GameDataManager.Instance.playerData.time > 0))
        {
            StartCoroutine(IEStart());
        }
    }
    
    IEnumerator IEStart()
    {
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("Game");
    }

    public void Load()
    {
        StartCoroutine(IEStart());
    }
}