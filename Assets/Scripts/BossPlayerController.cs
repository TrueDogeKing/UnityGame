using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlayerController : MonoBehaviour
{
    private PlayerController playerController;

    [SerializeField]
    private GameObject throwIndicator;

    [SerializeField]
    private GameObject can;

    [SerializeField]
    private BossController bossController;

    [SerializeField]
    private BossRoomStick stick;

    [SerializeField]
    private Vector2 throwAngleRange = new Vector2(20f, 60f);
    [SerializeField]
    private float throwAngleSpeed = 30f;
    private float throwAngleDir = 1f;
    private float throwAngle = 0f;

    [SerializeField]
    private Vector2 throwForceRange = new Vector2(10f, 20f);
    [SerializeField]
    private float throwForceSpeed = 30f;
    private float throwForceDir = 1f;
    private float throwForce = 10f;

    public bool hasStick = false;
    public bool hasCan = false;
    private bool canDrink = false;
    private bool isComingBack = false;

    private Rigidbody2D rigidbody2d;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (GameManager.instance.currentGameState == GameManager.GameState.PAUSE_MENU)
            return;
        if (GameManager.instance.currentGameState != GameManager.GameState.GAME)
            return;
        if (GameManager.instance.currentGameMode != GameManager.GameMode.BOSS)
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.instance.bossFightState = GameManager.BossFightState.AIM_ANGLE;
            can.transform.position = new Vector3(0, -2.5f, 0);
            can.transform.rotation = Quaternion.identity;
        }

        playerController.Status();

        Run();

        if (!GameManager.instance.isPlayersTurn)
            return;

        rigidbody2d.simulated = false;

        Drink();
        AimForce();
        AimAngle();
    }

    void AimAngle()
    {
        if (GameManager.instance.bossFightState != GameManager.BossFightState.AIM_ANGLE)
            return;

        throwForce = 10f;
        UpdateRange(ref throwAngle, ref throwAngleDir, throwAngleSpeed, throwAngleRange.x, throwAngleRange.y);
        UpdateThrowIndicator(true);

        if (Input.GetKeyDown(KeyCode.E))
        {
            GameManager.instance.bossFightState = GameManager.BossFightState.AIM_FORCE;
        }
    }

    void AimForce()
    {
        if (GameManager.instance.bossFightState != GameManager.BossFightState.AIM_FORCE)
            return;

        UpdateRange(ref throwForce, ref throwForceDir, throwForceSpeed, throwForceRange.x, throwForceRange.y);
        UpdateThrowIndicator(false);

        if (Input.GetKeyDown(KeyCode.E))
        {
            GameManager.instance.bossFightState = GameManager.BossFightState.THROW;

            bool hit = ThrowValue() < 0.5f;
            stick.Throw(StickThrowDirection.RIGHT, hit);

            StartCoroutine(WaitForThrow());
        }
    }

    IEnumerator WaitForThrow()
    {
        yield return new WaitForSeconds(2.5f);

        GameManager.instance.bossFightState = GameManager.BossFightState.DRINKING;
        canDrink = can.transform.position.x > 0.5f || can.transform.position.x < -0.5f;
        bossController.InitTurn();
    }

    void Run()
    {
        if (GameManager.instance.bossFightState != GameManager.BossFightState.DRINKING)
            return;

        if (GameManager.instance.isPlayersTurn)
            return;

        rigidbody2d.simulated = !isComingBack;
        playerController.Walking();

        if (!isComingBack && hasCan && hasStick && transform.position.x > -0.1 && transform.position.x < 0.1)
        {
            rigidbody2d.simulated = false;
            isComingBack = true;
            can.transform.position = new Vector3(0, -2.5f, 0);
            can.transform.rotation = Quaternion.identity;
        }

        if (isComingBack && transform.position.x > -14.1 && transform.position.x < -13.9)
        {
            GameManager.instance.isPlayersTurn = true;
            GameManager.instance.bossFightState = GameManager.BossFightState.AIM_ANGLE;
            transform.position = new Vector3(-14f, -1.94f, 0);
            transform.localScale = new Vector3(1, 1, 1);
            Animator animator = GetComponent<Animator>();
            animator.SetBool("IsWalking", false);
            hasStick = false;
            hasCan = false;
            canDrink = false;
            isComingBack = false;
        }
    }

    void Drink()
    {
        if (GameManager.instance.bossFightState != GameManager.BossFightState.DRINKING)
            return;

        if (!GameManager.instance.isPlayersTurn)
            return;

        if (canDrink && Input.GetKeyDown(KeyCode.E))
        {
            GameManager.instance.playerBeer -= 1f;
        }
    }

    void UpdateThrowIndicator(bool ignoreForce)
    {
        throwIndicator.transform.rotation = Quaternion.Euler(new Vector3(0, 0, throwAngle));

        for (int i = 0; i < throwIndicator.transform.childCount; i++)
        {
            Transform square = throwIndicator.transform.GetChild(i);
            square.localPosition = new Vector3(0.4f + i * (throwForce / 25f), 0, 0);
            Color color = Color.Lerp(Color.green, Color.white, ThrowValue(ignoreForce));
            square.GetComponent<SpriteRenderer>().color = color;
        }
    }

    private float ThrowValue(bool ignoreForce = false)
    {
        float a = (throwAngle - throwAngleRange.x) / (throwAngleRange.y - throwAngleRange.x);
        float f = (throwForce - throwForceRange.x) / (throwForceRange.y - throwForceRange.x);
        if (ignoreForce)
        {
            f = 0.5f;
        }
        a = Mathf.Abs(a - 0.5f) * 2f;
        f = Mathf.Abs(f - 0.5f) * 2f;

        float threshold = 0.3f;

        if (a < threshold && f < threshold)
            return 0f;

        return 1f;
    }

    private void UpdateRange(ref float value, ref float dir, float speed, float min, float max)
    {
        value += dir * speed * Time.deltaTime;
        if (value > max)
        {
            value = max;
            dir *= -1f;
        }
        if (value < min)
        {
            value = min;
            dir *= -1f;
        }
    }
}

// float throwForce = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), throwAngle.transform.position) * 4.0f;
// throwForce = Mathf.Clamp(throwForce, 1f, 40f);

// for (int i = 0; i < throwAngle.transform.childCount; i++)
// {
//     throwAngle.transform.GetChild(i).localPosition = new Vector3(0.4f + (i + 0.5f) * (throwForce / 20f), 0, 0);
// }

// if (Input.GetMouseButtonDown(0) && throwAngle.activeSelf)
// {
//     stick.transform.position = throwAngle.transform.position;
//     stickRigidbody.velocity = Vector2.zero;
//     stickRigidbody.angularVelocity = 0;
//     stickRigidbody.AddForce(throwAngle.transform.right * throwForce, ForceMode2D.Impulse);
//     stickRigidbody.AddTorque(0.5f, ForceMode2D.Impulse);
// }