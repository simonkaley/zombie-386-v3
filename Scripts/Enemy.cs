using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private KillTracker killTracker;
    private AudioPlayer audioPlayer;
    private PlayerData player;

    [SerializeField] float MoveSpeed = 1f;
    private float rayCastDist = 1;

    // Prefabs for health pack, machine gun, and shotgun
    public GameObject healthPackPrefab;
    public GameObject machineGunPrefab;
    public GameObject shotgunPrefab;

    // Probability of dropping each item
    private const float healthPackDropProbability = 0.05f;
    private const float machineGunDropProbability = 0.05f;
    private const float shotgunDropProbability = 0.05f;

    // Probability of being a boomer (5% chance)
    private const float boomerProbability = 0.1f;
    private bool isBoomer;

    // Explosion effect prefab for the boomer
    public GameObject explosionPrefab;

    // Sprite for the boomer
    public Sprite boomerSprite;

    // Variables for obstacle avoidance
    private bool isObstacleDetected = false;
    private Vector2 obstacleAvoidanceDirection;


    public GameObject bloodParticlesPrefab;

    private PauseGame pauseGame;

    private Animator animator;


    void Start()
    {
        player = FindObjectOfType<PlayerData>();
        killTracker = FindObjectOfType<KillTracker>();
        audioPlayer = FindObjectOfType<AudioPlayer>();

        pauseGame = FindObjectOfType<PauseGame>();

        isBoomer = Random.value <= boomerProbability;

        
        animator = GetComponent<Animator>();

        // Check if this enemy is a boomer
        if (isBoomer)
        {
            // Set the animator to play the boomer animation
            if (animator != null)
            {
                // Trigger the boomer animation
                animator.SetBool("Boomer", true);
            }
        }
    }

    void Update()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;

        // Check for obstacles in the path
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayCastDist, LayerMask.GetMask("Obstacle"));

        // If there is an obstacle and we haven't detected one before
        if (hit.collider != null && !isObstacleDetected)
        {
            isObstacleDetected = true;
            Vector2 normal = hit.normal;

            // Calculate avoidance direction perpendicular to the obstacle normal
            obstacleAvoidanceDirection = new Vector2(-normal.y, normal.x);
        }

        // If we're currently avoiding an obstacle
        if (isObstacleDetected)
        {
            // Move along the avoidance direction
            transform.position += (Vector3)obstacleAvoidanceDirection * Time.deltaTime * MoveSpeed;

            // Check if there's no longer an obstacle in the way
            RaycastHit2D hitAfterAvoidance = Physics2D.Raycast(transform.position, direction, rayCastDist, LayerMask.GetMask("Obstacle"));
            if (hitAfterAvoidance.collider == null)
            {
                isObstacleDetected = false;
            }
        }
        else
        {
            // Move towards the player
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Time.deltaTime * MoveSpeed);

            // Flip the enemy sprite based on the player's position
            if (player.transform.position.x > transform.position.x)
            {
                // Player is to the right of the enemy, so flip the enemy sprite to face right
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                // Player is to the left of the enemy, so flip the enemy sprite to face left
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
    }

    void OnDestroy()
    {
        audioPlayer.enemyDeathSounds();
        killTracker.addKill();

        

        // Drop items with a certain probability
        if (PlayerData.curHealth > 0 && !pauseGame.isPaused)
        {

            Instantiate(bloodParticlesPrefab, transform.position, Quaternion.identity);

            if (Random.value <= healthPackDropProbability)
            {
                Instantiate(healthPackPrefab, transform.position, Quaternion.identity);
            }

            if (Random.value <= machineGunDropProbability)
            {
                Instantiate(machineGunPrefab, transform.position, Quaternion.identity);
            }

            if (Random.value <= shotgunDropProbability)
            {
                Instantiate(shotgunPrefab, transform.position, Quaternion.identity);
            }

            if (isBoomer)
            {
                // Spawn explosion effect for boomer variant
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
