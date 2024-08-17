using System;
using UnityEngine;
using UnityEngine.Events;

public class Checkbox : Button {
    public UnityEvent<bool> onValueChanged;
    public CustomUIElement toggle;
    public bool _activated = false;
    public float toggleFadeTime = 0;

    public bool Activated {
        get { return _activated; }
        set {
            _activated = value;
            onValueChanged?.Invoke(value);
            Refresh();
        }
    }


    public override void Awake() {
        base.Awake();
        Refresh();
        //toggle.SetActive(Activated);
        //_Activated = Activated;
    }

    public override void OnMouseUpAsButton() {
        base.OnMouseUpAsButton();
        Activated = !Activated;

    }

    public void Refresh() {
        if (!Activated) {
            toggle.SetAttrAni(0, toggleFadeTime, ColorAttr.a, true, scaled: false);
        }
        else {
            toggle.SetActive(true);
            toggle.SetAttrAni(0, 1, toggleFadeTime, ColorAttr.a, scaled: false);
        }
    }

}