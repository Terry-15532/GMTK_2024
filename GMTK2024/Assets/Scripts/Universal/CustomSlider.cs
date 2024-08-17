using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(CustomSlider))]
public class SliderInspector : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        CustomSlider slider = (CustomSlider)target;
        if (GUILayout.Button("Refresh")) {
            slider.UpdateUI();
        }
    }
}
#endif

public class CustomSlider : MonoBehaviour {
    public CustomUIElement track, fillArea;
    public SliderHandle handle;
    public RectTransform start, end;
    public TextMeshProUGUI valueTxt;
    public UnityEvent<float> onValueChanged;

    public float _value;
    public float value {
        get {
            return _value;
        }
        set {
            _value = Mathf.Clamp((float)Math.Round(value, roundAccuracy), minValue, maxValue);
            if (onValueChanged != null) {
                onValueChanged.Invoke(_value);
            }
            UpdateUI();
        }
    }

    public float maxValue, minValue;

    public Vector2 handlePosDelta = Vector2.zero;
    public Vector2 startPos, endPos;
    public bool interactable;
    public bool fill, roundActualValue;
    public int roundAccuracy, displayedAccuracy;

    public void Start() {
        Init();
    }

    public void Init() {
        handle.slider = this;
        if (fill) {
            fillArea.SetActive(true);
            fillArea.fillAmount = valuePercentage;
        }
        UpdateUI();
    }

    public float valuePercentage {
        get {
            return (value - minValue) / (maxValue - minValue);
        }
        set {
            this.value = minValue.Lerp(maxValue, value);
        }
    }

    public void UpdateUI() {
        RefreshStartEndPoint();
        if (fill) {
            fillArea.SetActive(true);
            fillArea.fillAmount = valuePercentage;
        }
        valueTxt.text = Math.Round(value, displayedAccuracy).ToString();
        handle.position = Vector2.Lerp(startPos, endPos, valuePercentage) + handlePosDelta;
    }

    public void RefreshStartEndPoint() {
        startPos = start.anchoredPosition;
        endPos = end.anchoredPosition;
    }

}