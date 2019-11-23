using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Configuration Parameters
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 0.5f;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 20f;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        InitiateMoveBounderies();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject laserI = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            laserI.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
        }
    }

    private void InitiateMoveBounderies()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime;

        var newXPosition = Mathf.Clamp(transform.position.x + moveSpeed * deltaX, xMin, xMax);
        var newYPosition = Mathf.Clamp(transform.position.y + moveSpeed * deltaY, yMin, yMax);

        transform.position = new Vector2(newXPosition, newYPosition);
    }
}
