using TMPro;
using UnityEngine;

public class PlayerValues : Singleton<PlayerValues>
{
    public TextMeshProUGUI NormalBallAmmo;
    public TextMeshProUGUI ExplotionBallAmmo;
    //public TextMeshProUGUI Health;
    //public TextMeshProUGUI Score;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Implement(this, out var _);
    }

    public void UpdateAmmoCount(int regular, int explosive)
    {
        NormalBallAmmo.text = $"Normal Ball Ammo: {regular}";
        ExplotionBallAmmo.text = $"Explotion Ball Ammo: {explosive}";
    }


    void SetActiveBN()
    {

    }
}
