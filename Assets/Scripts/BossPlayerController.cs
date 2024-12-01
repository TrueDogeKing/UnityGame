using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlayerController : MonoBehaviour
{
    private PlayerController playerController;

    [SerializeField]
    private GameObject throwAngle;

    [SerializeField]
    private GameObject stick;
    private Rigidbody2D stickRigidbody;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        stickRigidbody = stick.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        throwAngle.SetActive(playerController.IsFacingRight);

        if (playerController.IsFacingRight)
        {
            UpdateThrow();
        }
    }

    void UpdateThrow()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(throwAngle.transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        throwAngle.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        throwAngle.SetActive(angle > -90 && angle < 90);

        float throwForce = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), throwAngle.transform.position) * 4.0f;
        throwForce = Mathf.Clamp(throwForce, 1f, 40f);

        for (int i = 0; i < throwAngle.transform.childCount; i++)
        {
            throwAngle.transform.GetChild(i).localPosition = new Vector3(0.4f + (i + 0.5f) * (throwForce / 20f), 0, 0);
        }

        if (Input.GetMouseButtonDown(0) && throwAngle.activeSelf)
        {
            stick.transform.position = throwAngle.transform.position;
            stickRigidbody.velocity = Vector2.zero;
            stickRigidbody.angularVelocity = 0;
            stickRigidbody.AddForce(throwAngle.transform.right * throwForce, ForceMode2D.Impulse);
            stickRigidbody.AddTorque(0.5f, ForceMode2D.Impulse);
        }
    }
}
