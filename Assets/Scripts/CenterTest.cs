using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CenterTest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;

    public void SetText(string newText)
    {
        if (label == null) return;

        label.text = newText;

        // Force the layout system to rebuild immediately
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }
}