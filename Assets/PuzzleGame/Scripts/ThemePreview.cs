﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ThemePreview : MonoBehaviour
{
    [SerializeField]
    Button button;

    [SerializeField]
    Image[] bricks;
    [SerializeField]
    Image[] background;
    [SerializeField]
    Text price;
    [SerializeField]
    Image ads;

    [SerializeField]
    GameObject[] checkMarks;

    ThemePreset theme;

    public event Action<ThemePreview> Click = delegate { };

    public ThemePreset Theme
    {
        get => theme;
        set
        {
            theme = value;
            UpdateTheme();
        }
    }

    void OnClick()
    {
        Click.Invoke(this);
    }

    void UpdateTheme()
    {
        if (theme == null)
            return;

        for (int i = 0; i < bricks.Length; i++)
        {
            int colorIndex = Mathf.Clamp(i, 0, theme.buttonColors.Length - 1);
            bricks[i].color = theme.buttonColors[colorIndex];
        }

        foreach (Image image in background)
        {
            image.color = theme.fieldColor;
        }

        bool isAvailable = theme.price.value <= 0 || ThemeController.Instance.IsThemePurchased(theme);

        price.color = theme.text;
        price.text = !isAvailable
            ? (theme.price.value - UserProgress.Current.GetItemPurchaseProgress(theme.name)).ToString()
            : string.Empty;

        ads.color = theme.text;
        ads.gameObject.SetActive(!isAvailable && theme.price.type == PriceType.Ads);

        foreach (GameObject checkMark in checkMarks)
        {
            checkMark.SetActive(ThemeController.Instance.CurrentTheme == theme);
        }
    }

    void OnThemeUpdate(ThemePreset themePreset)
    {
        UpdateTheme();
    }

    void Awake()
    {
        button.onClick.AddListener(OnClick);
        ThemeController.Instance.ThemeChanged += OnThemeUpdate;
        ThemeController.Instance.ThemePurchased += OnThemeUpdate;
    }

    void Start()
    {
        UpdateTheme();
    }

    void OnEnable()
    {
        UpdateTheme();
    }

    void OnDestroy()
    {
        ThemeController.Instance.ThemeChanged -= OnThemeUpdate;
        ThemeController.Instance.ThemePurchased -= OnThemeUpdate;
    }
}
