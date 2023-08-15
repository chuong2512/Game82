using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OpenUrlButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        Application.OpenURL("market://details?id=" + Application.identifier);
    }
}
