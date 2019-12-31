using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy")]
    float shotCounter;
    [SerializeField] float health = 100;
    [SerializeField] float minTimeBetweenShots = .5f;
    [SerializeField] float maxTimeBetweenShots = 5f;
    [SerializeField] GameObject deathVFX;
    [SerializeField] float deathExplosionTime = 1f;
    [SerializeField] AudioClip enemyDeathSFX;
    [SerializeField] [Range(0, 1)] float enemyDeathSFXVolume = 0.7f;
    [SerializeField] int scoreValue = 150;

    [Header("Enemy Projectile")]
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 5f;
    [SerializeField] AudioClip laserSound;
    [SerializeField] [Range(0, 1)] float laserSoundVolume = 0.7f;

    [Header("Enemy Drops")]
    [SerializeField] int dropChance = 10;
    [SerializeField] GameObject bonusItem;
    [SerializeField] float bonusItemSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
            AudioSource.PlayClipAtPoint(laserSound, Camera.main.transform.position, laserSoundVolume);
        }
    }

    private void Fire()
    {
        GameObject enemyLaser = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
        enemyLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
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

    private void HitManager(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        DropItem();
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(explosion, deathExplosionTime);
        AudioSource.PlayClipAtPoint(enemyDeathSFX, Camera.main.transform.position, enemyDeathSFXVolume);
    }

    private void DropItem()
    {
        int dropRandom = Random.Range(0, 100);
        if (dropRandom < dropChance)
        {
            GameObject createBonusItem = Instantiate(bonusItem, transform.position, Quaternion.identity) as GameObject;
            createBonusItem.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -bonusItemSpeed);
        }
    }
}
