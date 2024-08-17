using UnityEngine.Rendering;

public static class Volumes {
    public static Volume mainVolume = null, UIVolume = null;

    public static T GetFromMain<T>() where T : VolumeComponent {
        if (mainVolume == null) {
            mainVolume = GameInfo.mainCamera.GetComponentInChildren<Volume>();
        }

        mainVolume.sharedProfile.TryGet(out T component);
        return component;
    }

    public static T GetFromUI<T>() where T : VolumeComponent {
        if (UIVolume == null) {
            UIVolume = GameInfo.UICamera.GetComponentInChildren<Volume>();
        }

        UIVolume.sharedProfile.TryGet(out T component);
        return component;
    }
}