using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Configuration Parameters
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 0.5f;
    [SerializeField] int health = 200;
    [SerializeField] GameObject deathVFX;
    [SerializeField] float deathExplosionTime = 1f;
    [SerializeField] AudioClip playerDeathSFX;
    [SerializeField] [Range(0, 1)] float playerDeathSFXVolume = 1f;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 20f;
    [SerializeField] float projectileFiringPeriod = 0.1f;
    [SerializeField] AudioClip laserSound;
    [SerializeField] [Range(0, 1)] float laserSoundVolume = 0.3f;

    Coroutine firingCoroutine;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer)
        {
            return;
        }
        HitManager(damageDealer);
    }

    public int GetHealth()
    {
        return health;
    }

    public void DetractFromHealth(int healthValue)
    {
        health -= healthValue;
    }

    private void HitManager(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            PlayerDeath();
        }
    }

    private void PlayerDeath()
    {
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, deathExplosionTime);
        AudioSource.PlayClipAtPoint(playerDeathSFX, Camera.main.transform.position, playerDeathSFXVolume);
        FindObjectOfType<Level>().GameOver();
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laserI = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            laserI.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(laserSound, Camera.main.transform.position, laserSoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
 
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
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
