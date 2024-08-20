using UnityEngine;
using static Tools;
using static SceneSwitching;

public class DeathBox : MonoBehaviour
{
    private bool triggered = false;

    public void Reset() {
        triggered = false;
    }

    void Start() {
        Stage.instance.resetStage.AddListener(Reset);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!triggered) {
            // Debug.Log("triggered");
            triggered = true;
            StopAllCoroutines();
            GetComponent<AudioSource>().Play();
            Character.instance.canMove = false;
            Character.instance.rb.linearVelocity = Vector3.zero;
            Character.instance.GetComponentInParent<Collider>().enabled = false;
            Character.instance.rb.freezeRotation = false;
            Character.instance.rb.angularVelocity = Random.onUnitSphere * 10;
            Character.instance.rb.linearVelocity = Vector3.up * 3;
            Character.instance.rb.useGravity = true;
            CallDelayed(() => {FadeOut(0.2f, 0.2f, 0.5f);}, 1f);
        }
    }

    public static void FadeOut(float time1 = 0.2f, float time2 = 0.2f, float timeMiddle = 0.1f) {
        MsgBox.msgShowing = false;
        var s = Create();
        s.SetActive(true);
        s.SetAttrAni(0, 1, time1, ColorAttr.a);
        //s.rectTransform.SetAsLastSibling();
        CallDelayed(() => {
            CallDelayed(() => {
                s.SetAttrAni(1, 0, time2, ColorAttr.a, true);
                Stage.instance.resetStage.Invoke();
                CallDelayed(() => {
                    Destroy(s.gameObject);
                }, time2);
            }, timeMiddle);
        }, time1);
    }
}
