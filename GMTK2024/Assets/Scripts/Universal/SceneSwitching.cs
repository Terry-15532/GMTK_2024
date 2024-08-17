using UnityEngine;
using UnityEngine.SceneManagement;
using static GameInfo;
using static Tools;

public class SceneSwitching : CustomUIElement {
    public CustomUIElement background;

    public static void SwitchTo(string scene, float time1 = 0.2f, float time2 = 0.2f, float timeMiddle = 0.1f) {
        MsgBox.msgShowing = false;
        var s = Create();
        s.SetActive(true);
        s.SetAttrAni(0, 1, time1, ColorAttr.a);
        //s.rectTransform.SetAsLastSibling();
        CallDelayed(() => {
            SceneManager.LoadScene(scene);
            canvasRectTransform = null;
            currScene = scene;
            CallDelayed(() => {
                s.SetAttrAni(1, 0, time2, ColorAttr.a, true);
                CallDelayed(() => {
                    Destroy(s.gameObject);
                }, time2);
            }, timeMiddle);
        }, time1);
    }

    public static SceneSwitching Create() {
        var s = Instantiate(Resources.Load<SceneSwitching>("Prefabs/SceneSwitching"));
        DontDestroyOnLoad(s);
        return s;
    }
}
