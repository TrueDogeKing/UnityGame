using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField]
    private float drinkRate = 0.5f;

    [SerializeField]
    private float throwAccuracy = 0.5f;

    public Vector2 speedRange = new Vector2(1f, 2f);

    public float speed = 1f;

    public bool hasStick = false;
    public bool hasCan = false;
    public bool isComingBack = false;
    public bool canDrink = false;

    [SerializeField]
    private BossRoomStick stick;

    [SerializeField]
    private GameObject can;

    private BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (GameManager.instance.currentGameState == GameManager.GameState.PAUSE_MENU)
            return;
        if (GameManager.instance.currentGameState != GameManager.GameState.GAME)
            return;
        if (GameManager.instance.currentGameMode != GameManager.GameMode.BOSS)
            return;

        Run();
        Drink();
    }

    void Drink()
    {
        if (GameManager.instance.bossFightState != GameManager.BossFightState.DRINKING)
            return;

        if (GameManager.instance.isPlayersTurn)
            return;

        if (!canDrink)
            return;

        GameManager.instance.bossBeer -= drinkRate * Time.deltaTime;
    }

    void Run()
    {
        if (GameManager.instance.bossFightState != GameManager.BossFightState.DRINKING)
            return;

        if (!GameManager.instance.isPlayersTurn)
            return;

        float dir = -1f;

        if (hasCan && hasStick && transform.position.x < 0)
            dir = 1f;

        if (isComingBack)
            dir = 1f;

        transform.position += dir * Vector3.right * speed * Time.deltaTime;
        transform.localScale = new Vector3(-dir * 1.5f, 1.5f, 1.5f);

        if (!isComingBack && hasCan && hasStick && transform.position.x < 0.1f && transform.position.x > -0.1f)
        {
            boxCollider.enabled = false;
            isComingBack = true;

            can.transform.position = new Vector3(0, -2.5f, 0);
            can.transform.rotation = Quaternion.identity;
        }

        if (isComingBack && transform.position.x > 13.9f && transform.position.x < 14.1f)
        {
            GameManager.instance.isPlayersTurn = false;
            GameManager.instance.bossFightState = GameManager.BossFightState.THROW;
            transform.position = new Vector3(14f, -2.24f, 0);
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            StartCoroutine(Throw());
        }
    }

    IEnumerator Throw()
    {
        yield return new WaitForSeconds(0.5f);
        stick.Throw(StickThrowDirection.LEFT, Random.value < throwAccuracy);
        yield return new WaitForSeconds(2.5f);
        GameManager.instance.bossFightState = GameManager.BossFightState.DRINKING;
        canDrink = can.transform.position.x > 0.5f || can.transform.position.x < -0.5f;
    }

    public void InitTurn()
    {
        speed = Random.Range(speedRange.x, speedRange.y);
        boxCollider.enabled = true;
        hasCan = false;
        hasStick = false;
        isComingBack = false;
        canDrink = false;
    }
}
