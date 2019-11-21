using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime;
        var newXPosition = transform.position.x + moveSpeed * deltaX;
        var newYPosition = transform.position.y + moveSpeed * deltaY;
        transform.position = new Vector2(newXPosition, newYPosition);
    }
}
