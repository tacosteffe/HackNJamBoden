using TMPro;
using UnityEngine;

public class PlayerValues : Singleton<PlayerValues>
{
    public TextMeshProUGUI NormalBallAmmo;
    public TextMeshProUGUI ExplosionBallAmmo;

    void Awake()
    {
        Implement(this, out var _);
    }

    public void UpdateAmmoCount(int regular, int explosive)
    {
        NormalBallAmmo.text = $"Normal Ammo: {regular}";
        ExplosionBallAmmo.text = $"Explosion Ammo: {explosive}";
    }
}
