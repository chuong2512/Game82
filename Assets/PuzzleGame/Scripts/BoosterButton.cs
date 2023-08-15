using System;
using UnityEngine;
using UnityEngine.UI;

public class BoosterButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Button button;
    [SerializeField] private GameObject counter;
    [SerializeField] private GameObject counterEmpty;
    [SerializeField] private Text countText;
    [SerializeField] private Text emptyText;

    public event Action<BoosterPreset, bool> Select = delegate { };

    private BoosterPreset preset;
    private int highlightSortingOrder;
    private bool IsPurchased => BoostersController.Instance.IsBoosterPurchased(preset);

    public BoosterPreset Preset => preset;
    
    private void Awake()
    {
        button.onClick.AddListener(OnClick);

        BoostersController.Instance.BoosterPurchased += OnButtonUpdate;
        BoostersController.Instance.BoosterProceeded += OnButtonUpdate;
    }

    private void OnDestroy()
    {
        BoostersController.Instance.BoosterPurchased -= OnButtonUpdate;
        BoostersController.Instance.BoosterProceeded -= OnButtonUpdate;
    }

    public void Init(BoosterPreset preset, int highlightSortingOrder)
    {
        this.preset = preset;
        this.highlightSortingOrder = highlightSortingOrder;
        icon.sprite = preset.Icon;
        
        UpdateButton();
    }

    public void SetRaycast(bool value)
    {
        button.interactable = value;
    }

    private void Highlight()
    {
        gameObject.AddComponent<Canvas>();
        gameObject.GetComponent<Canvas>().overrideSorting = true;

        gameObject.GetComponent<Canvas>().sortingOrder = highlightSortingOrder;
    }

    public void Deselect()
    {
        Destroy(gameObject.GetComponent<Canvas>());
        UpdateButton();
    }

    private void UpdateButton()
    {
        counter.gameObject.SetActive(IsPurchased);
        counterEmpty.gameObject.SetActive(!IsPurchased);

        if (!IsPurchased) return;
        
        int count = BoostersController.Instance.GetBoosterPurchaseCount(preset);
        countText.text = count.ToString();
        SetRaycast(true);
    }

    void OnClick()
    {
        if(IsPurchased)
            Highlight();
            
        Select.Invoke(preset, IsPurchased);
    }

    private void OnButtonUpdate(BoosterPreset preset)
    {
        if(this.preset != preset) return;
        
        UpdateButton();
    }
    
    private void OnButtonUpdate(BoosterPreset arg1, bool value)
    {
        UpdateButton();
    }
}
