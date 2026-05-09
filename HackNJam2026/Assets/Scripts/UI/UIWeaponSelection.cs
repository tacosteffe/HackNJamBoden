using UnityEngine;
using UnityEngine.UI;
public class UIWeaponSelection : MonoBehaviour
{
    public Button ballOneButton;
    public Button ballTwoButton;
    void Update()
    {
        


    }

    public void BallOne()
    {
        PlayerCatapult.Instance.SetCurrentBall = 0;
        PlayerCatapult.Instance.ChangeBall();
    }

    public void BallTwo()
    {
        PlayerCatapult.Instance.SetCurrentBall = 1;
        PlayerCatapult.Instance.ChangeBall();
    }
}
