using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookThroawer : MonoBehaviour
{
    [SerializeField] private GameObject bookPrefab; // Book prefab to be thrown
    [SerializeField] private float throwInterval = 5.0f; // Time interval between throws
    [SerializeField] private Vector2 throwForceRange = new Vector2(5f, 10f); // Min and max throw force range
    [SerializeField] private Vector2 throwAngleRange = new Vector2(-30f, 30f); // Min and max angle range in degrees
    private GameObject player;
    private bool isThrowing = true;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(ThrowBooks());
    }

    private void Update()
    {
        if ((Vector2.Distance(player.transform.position, transform.position) < 4.0f) && isThrowing)
        {
            isThrowing = false;
            // cutscence activation
        }
    }

    private IEnumerator ThrowBooks()
    {
        while (true)
        {
            // Instantiate a new book at the thrower's position
            GameObject book = Instantiate(bookPrefab, transform.position, Quaternion.identity);

            // Get the Rigidbody2D of the book
            Rigidbody2D bookRb = book.GetComponent<Rigidbody2D>();

            // Randomize throw angle and force
            float randomAngle = Random.Range(throwAngleRange.x, throwAngleRange.y);
            float randomForce = Random.Range(throwForceRange.x, throwForceRange.y);

            // Calculate direction based on angle
            Vector2 throwDirection = Quaternion.Euler(0, 0, randomAngle) * Vector2.down;

            // Apply force to the book to launch it with the calculated direction and force
            bookRb.AddForce(throwDirection * randomForce, ForceMode2D.Impulse);

            // Wait for the next throw interval
            yield return new WaitForSeconds(throwInterval);
            if(!isThrowing)
                yield break;
        }
    }
}
