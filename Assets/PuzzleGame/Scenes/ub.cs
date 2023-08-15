using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ub : MonoBehaviour
{
    public GameObject obj;
    public GameObject buttonX;
    
    void Start()
    {
        obj.SetActive((GameDataManager.Instance.playerData.time <= 0));
    }
    
    public bool open;
    public LoadScene loadScene;
    
    void Update()
    {
        buttonX.SetActive((GameDataManager.Instance.playerData.time > 0));

        if ((GameDataManager.Instance.playerData.time > 0) && !open)
        {
            open = true;
            loadScene.Load();
        }
    }
}
