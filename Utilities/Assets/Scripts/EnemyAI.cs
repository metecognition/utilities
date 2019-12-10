using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class EnemyAI : NetworkBehaviour
{

    //detection variables
    public GameObject target;
    public NavMeshAgent agent;
    private LayerMask mask;

    public float walkSpeed = 1.5f;
    public float runSpeed = 3.5f;
    
    private bool justLostSight = false;
    private bool canSee = false;
    private Vector3 targetLocation;
    private int sightTimer;

    //state variables
    private string currentState = "";
    public bool canPatrol = false;

    public GameObject[] patrolLocations;
    private int patrolCount = 0;
    private bool headingToPatrolLocation;

    private float navigationUpdateTime = 0.5f;
    private float navigationUpdateTimer = 0.5f;


    //combat variables
    public GameObject bulletObject;
    private float shootTimer = 0.0f;
    public float shootTimeMoving = 3.0f;
    public float shootTimeStanding = 1.0f;
    public float accuracy;

    private NavMeshAgent navigator;

    public bool dead = false;
    private bool hasDied = false;


    private void Start() {
        mask = LayerMask.GetMask("Walls");
        currentState = "idle";
        navigator = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dead == false) {
            target = FindNearestPlayer();
            if (target != null) {
                targetLocation = target.transform.position;
                Vector3 startPos = transform.position;
                Vector3 endPos = targetLocation;


                //checks if can see the target and determines state
                if (!Physics.Linecast(startPos, endPos, mask)) {
                    if (Vector3.Distance(startPos, endPos) < 6) {
                        currentState = "tooClose";
                    }
                    else {
                        currentState = "chase";
                    }
                }
                else {
                    if (canPatrol) {
                        currentState = "patrol";
                    }
                    else {
                        currentState = "idle";
                    }
                }
            }
            else {
                if (canPatrol) {
                    currentState = "patrol";
                }
                else {
                    currentState = "idle";
                }
            }
        }
        else {
            currentState = "dead";
        }

        //state machine
        switch (currentState) {
            case "idle":
                //Debug.Log("Idle");
                Idle();
                break;
            case "chase":
                //Debug.Log("Chasing");
                Chase();
                break;
            case "tooClose":
                //Debug.Log("Too Close");
                TooClose();
                break;
            case "patrol":
                //Debug.Log("Patrolling");
                Patrol();
                break;
            case "dead":
                if (hasDied == false) {
                    Kill();
                }
                break;
            default:
                Debug.Log("Enemy AI State Error");
                break;
        }

        navigationUpdateTimer -= Time.deltaTime;

    }

    //------------------------------
    //Chase State
    //Runs towards target, shooting slowly
    void Chase() {
        navigator.speed = runSpeed;
        headingToPatrolLocation = false;


        if (navigationUpdateTimer <= 0) {
            agent.SetDestination(targetLocation);
            navigationUpdateTimer = navigationUpdateTime;
        }

        if (Vector3.Distance(transform.position, targetLocation) > 8) {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.forward), 0.3f);
        } else {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetLocation - transform.position), 0.3f);
        }


        shootTimer += Time.deltaTime;
        if (shootTimer >= shootTimeMoving) {
            shootTimer = 0.0f;
            Shoot();
            //Create and Move Bullet
            /*GameObject bullet = Instantiate(bulletObject);
            bullet.transform.position = transform.position + transform.forward;
            bullet.transform.rotation = transform.rotation;
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.AddRelativeForce(bullet.transform.forward * 1200);*/
        }
        
    }

    //------------------------------
    //Idle State
    //Stands still, occasionally moves a short distance in a random direction
    void Idle() {
        navigator.speed = walkSpeed;
        headingToPatrolLocation = false;

        if (Random.Range(0, 300)<1){
            Vector3 randomMove = transform.position;
            randomMove.x += Random.Range(-5, 5);
            randomMove.z += Random.Range(-5, 5);

            if (navigationUpdateTimer <= 0) {
                agent.SetDestination(targetLocation);
                navigationUpdateTimer = navigationUpdateTime;
            }
            //Debug.Log("random movement");
        }
    }

    //------------------------------
    //Too Close State
    //Activated when the target is within close range
    //Shoots rapidly, always pointing at the target
    void TooClose() {
        headingToPatrolLocation = false;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetLocation - transform.position), 0.3f);

        shootTimer += Time.deltaTime;
        if (shootTimer >= shootTimeStanding) {
            shootTimer = 0.0f;
            Shoot();
            //Create and Move Bullet
            /*GameObject bullet = Instantiate(bulletObject);
            bullet.transform.position = transform.position + transform.forward;
            bullet.transform.rotation = transform.rotation;
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            bulletRb.AddRelativeForce(bullet.transform.forward * 1200);*/
        }
    }

    //------------------------------
    //Patrol State
    //Navigates to empty objects placed throughout the scene, repeats indefinitely, generally replaces idle state
    //Empties are stored in an array called patrolLocations
    void Patrol() {
        navigator.speed = walkSpeed;
        if (!headingToPatrolLocation) {
            headingToPatrolLocation = true;
            patrolCount += 1;
            if (patrolCount >= patrolLocations.Length) {
                patrolCount = 0;
            }
            if (navigationUpdateTimer <= 0) {
                agent.SetDestination(targetLocation);
                navigationUpdateTimer = navigationUpdateTime;
            }
        }

        if (Vector3.Distance(transform.position, patrolLocations[patrolCount].transform.position) < 4) {
            headingToPatrolLocation = false;
        }
    }

    void Kill() {
        navigator.enabled = false;
    }



    //----------------------------------------
    //Find Nearest Player Function
    //Finds and returns the GameObject of the nearest player
    public GameObject FindNearestPlayer() {
        GameObject[] playersOnMap;
        playersOnMap = GameObject.FindGameObjectsWithTag("Player");
        GameObject closestPlayer = null;
        float distanceToNearestPlayer = Mathf.Infinity;
        foreach (GameObject player in playersOnMap) {
            if (Vector3.Distance(transform.position, player.transform.position) < distanceToNearestPlayer) {
                distanceToNearestPlayer = Vector3.Distance(transform.position, player.transform.position);
                closestPlayer = player;
            }
        }

        return closestPlayer;
    }

    void Shoot() {
        GameObject bullet = Instantiate(bulletObject);
        bullet.transform.position = transform.position + transform.forward;
        bullet.transform.rotation = transform.rotation;
        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();

        bulletRB.velocity = transform.TransformDirection(new Vector3(Random.Range(-accuracy, accuracy), 0, 30 + Random.Range(-accuracy, accuracy)));
    }
    
}
