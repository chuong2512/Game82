using System;
using UnityEngine;

public class GameOverAds : MonoBehaviour
{
    public float lastChanceSeconds = 3f;
    
    [SerializeField]
    GameObject restartGameObject;

    [SerializeField]
    RectTransform maskImage;

    private float maskSize;
    private bool isWatching;
    public event Action LastChanceComplete = delegate { };

    private void Awake()
    {
        maskSize = maskImage.rect.width;
    }
    
    void OnEnable()
    {
        restartGameObject.SetActive(true);
    }

    void OnDestroy()
    {
    }

    void OnWatchAdsClick()
    {
        isWatching = true;
        StopAllCoroutines();
    }

    public void LastChance()
    {
        restartGameObject.SetActive(true);
        
        maskImage.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maskSize);
    }

    public void Skip()
    {
        OnTimerOver();
    }

    void OnTimerOver()
    {
        restartGameObject.SetActive(true);
    }
    
    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.LogError(message);
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }
}
