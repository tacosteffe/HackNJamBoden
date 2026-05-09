using TMPro;
using UnityEngine;

public class PlayerValues : MonoBehaviour
{
    public TextMeshProUGUI NormalBallAmmo;
    public TextMeshProUGUI ExplotionBallAmmo;
    public TextMeshProUGUI Health;
    public TextMeshProUGUI Score;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        NormalBallAmmo.text = $"Ammo: {PlayerCatapult.Instance.NormalBallAmmo}";
    }

    void SetActiveBN()
    {

    }
}
