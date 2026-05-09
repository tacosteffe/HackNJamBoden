using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCatapult : Singleton<PlayerCatapult>
{
    [Header("Catapult turn rate")]
    public float TurnRate = 5f;

    public bool TankControlsY = false;

    public AudioSource WheelTurn;

    //Movement input
    private Vector2 MoveInput = Vector2.zero;
    [SerializeField, Header("Starting rotation")]
    private float StartRotation = 0f;
    private float Rotation = 0f;

    private float AimAngle = 0f;
    private float MinAngle = 0f;
    private float MaxAngle = 45f;






    //TODO add more balls and make a selection between them
    [SerializeField]
    private GameObject Ball1_Prefab;
    [SerializeField]
    private GameObject Ball2_Prefab;



    private void Awake()
    {
        Implement(this, out var _);

        FireState = FIRING_STATE.WAITING;
        Rotation = StartRotation;
        CurrentFireAngle = CockedAngle;
        PlaceholderBall1.SetActive(true);

        RotatePlayer();
        UpdateArmRotation();
        SetPlaceholderBall(true);

        InputManager.Instance.SubscribeHeldAction("Player", "Move", OnMove);
        InputManager.Instance.SubscribeSingleAction("Player", "Jump", CallFire);
    }

    private void OnDestroy()
    {
        InputManager.Instance.UnsubscribeHeldAction("Player", "Move", OnMove);
        InputManager.Instance.UnsubscribeSingleAction("Player", "Jump", CallFire);
    }


    // Update is called once per frame
    void Update()
    {
        //If movement is held for keys AD turn right or left
        if (MoveInput.x < -0.2f || MoveInput.x > 0.2f)
        {
            Rotation += TurnRate * Time.deltaTime * Mathf.Sign(MoveInput.x);
            RotatePlayer();

            if (!WheelTurn.isPlaying)
                WheelTurn.Play();
        }
        else if (WheelTurn.isPlaying)
        {
            WheelTurn.Stop();
        }
        //If movement is held for keys WS change the aiming angle
        if (MoveInput.y < -0.2f || MoveInput.y > 0.2f)
        {
            AimAngle += TurnRate * Time.deltaTime * Mathf.Sign(MoveInput.y) * (TankControlsY ? 1f : -1f);
            AimAngle = Mathf.Clamp(AimAngle, MinAngle, MaxAngle);
        }


        if (FireState != FIRING_STATE.WAITING)
        {
            UpdateArm();
        }

        DrawAimLine();
    }

    void RotatePlayer()
    {
        transform.rotation = Quaternion.Euler(0f, Rotation, 0f);
    }








    #region Animation stuff

    private enum FIRING_STATE
    {
        WAITING = 0,
        FIRING,
        RETRACTING
    }


    private FIRING_STATE FireState = FIRING_STATE.WAITING;
    private float FireCooldown = 5f;

    private EasyTimerNL FireAnimTimer = new EasyTimerNL(0.15f);
    private EasyTimerNL RetractAnimTimer = new EasyTimerNL(3f);

    [SerializeField]
    private Transform ArmWrapper;

    private float CockedAngle = 5.5f;
    private float FiringAngle = -62.0f;
    private float CurrentFireAngle = 5.5f;

    [SerializeField]
    private GameObject PlaceholderBall1;
    [SerializeField]
    private GameObject PlaceholderBall2;

    [SerializeField]
    private LineRenderer AimLine;

    public float AimLineLength = 5f;

    void UpdateArm()
    {
        if (FireState == FIRING_STATE.FIRING)
        {
            float t = FireAnimTimer.Update(Time.deltaTime);
            if (t >= 1f)
            {
                FireAnimTimer.ResetTimer();
                FireState = FIRING_STATE.RETRACTING;
                SetPlaceholderBall(false);
                Launch();
            }
            CurrentFireAngle = Mathf.Lerp(CockedAngle, FiringAngle, t);
        }
        else if (FireState == FIRING_STATE.RETRACTING)
        {
            float t = RetractAnimTimer.Update(Time.deltaTime);
            if (t >= 1f)
            {
                RetractAnimTimer.ResetTimer();
                FireState = FIRING_STATE.WAITING;
                SetPlaceholderBall(true);
            }
            CurrentFireAngle = Mathf.Lerp(FiringAngle, CockedAngle, t);
        }
        UpdateArmRotation();
    }

    void UpdateArmRotation()
    {
        ArmWrapper.localRotation = Quaternion.Euler(0f, 0f, CurrentFireAngle);
    }

    public int SetCurrentBall { set => CurrentActive = value; }
    private int CurrentActive = 0;


    public void ChangeBall()
    {
        //IF FireState == Retracting active -> false

        SetPlaceholderBall(FireState != FIRING_STATE.RETRACTING);
    }

    public void SetPlaceholderBall(bool active)
    {
        PlaceholderBall1.SetActive(CurrentActive == 0 ? active : false);
        PlaceholderBall2.SetActive(CurrentActive == 1 ? active : false);
    }



    void DrawAimLine()
    {
        var pos = FiringLoc.position;
        var dir = (Quaternion.Euler(0f, 0f, AimAngle) * FiringLoc.right).normalized;

        AimLine.SetPositions(new Vector3[]
        {
            pos,
            pos + dir * AimLineLength
        });
    }

    #endregion


    #region Firing

    [SerializeField]
    private Transform FiringLoc;
    private float FiringForce = 30f;


    [SerializeField, Header("PlayerValues")]
    public int NormalBallAmmo;
    public int ExpltionBallAmmo;


    
    void Launch()
    {
        var pos = FiringLoc.position;
        var dir = (Quaternion.Euler(0f, 0f, AimAngle) * FiringLoc.right).normalized;

        var go = Instantiate(Ball1_Prefab);
        var ball = go.GetComponent<BallBase>();
        ball.Fire(pos, dir, FiringForce);
    }


    void OnMove(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
        Debug.Log(MoveInput);
    }

    //TODO call from UI?
    public void Fire()
    {
        if (FireState == FIRING_STATE.WAITING) FireState = FIRING_STATE.FIRING;
    }

    void CallFire(InputAction.CallbackContext ctx)
    {
        if (FireState == FIRING_STATE.WAITING) FireState = FIRING_STATE.FIRING;
    }

    #endregion

}
