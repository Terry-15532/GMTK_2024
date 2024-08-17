using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Settings;

public class TextLocalization : MonoBehaviour {
    public static List<TextLocalization> allTexts = new();
    public List<string> texts;
    public TextMeshProUGUI textMeshProUGUI;
    public TextMeshPro textMeshPro;

    public static void UpdateTexts() {
        foreach (TextLocalization t in allTexts) {
            t.Refresh();
        }
    }

    public void Awake() {
        allTexts.Add(this);
        textMeshPro = GetComponent<TextMeshPro>();
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        Refresh();

    }

    public void Refresh() {
        if ((int)language < texts.Count) {
            if (textMeshProUGUI) {
                textMeshProUGUI.text = texts[(int)language];
            }
            else if (textMeshPro) {
                textMeshPro.text = texts[(int)language];
            }
        }

        else {
            if (textMeshProUGUI) {
                textMeshProUGUI.text = texts[0];
            }
            else if (textMeshPro) {
                textMeshPro.text = texts[0];
            }
        }
    }

    public void OnDestroy() {
        allTexts.Remove(this);
    }
}