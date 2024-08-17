public class EmptyMono : UnityEngine.MonoBehaviour {
    public void Awake() {
        DontDestroyOnLoad(this.gameObject);
        Tools.SetCaller(this);
    }
}