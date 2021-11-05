using MoreMountains.NiceVibrations;

namespace HyperCatSdk
{
    public static class HCVibrate
    {
        public static void Haptic(HapticTypes type)
        {
            if (GameManager.Instance.Data.Setting.Haptic == 0)
                return;

            MMVibrationManager.Haptic(type, false, true);
        }
    }
}