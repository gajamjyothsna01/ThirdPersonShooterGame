using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieeController : MonoBehaviour
{
    Animator anim;
    public GameObject target;
    NavMeshAgent agent;
    //AudioSource audio;
   // public List<AudioClip> audioClips;
    public float walkingSpeed;
    public float runningSpeed;

   // public GameObject ragDollPrefab;
    enum STATE { IDLE, WONDER, CHASE, ATTACK, DEAD };
    STATE state = STATE.IDLE;//default state
    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        //anim.SetBool("isWalking", true);
        agent = this.GetComponent<NavMeshAgent>();
        //audio = this.GetComponent<AudioSource>();
        //.playOnAwake = audioClips[0];


    }

    // Update is called once per frame
    void Update()
    {
        /*
        agent.SetDestination(target.transform.position);

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            anim.SetBool("isWalking", true);
            anim.SetBool("isAttacking", false);
        }
        else
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isAttacking", true);
            audio.PlayOneShot(audioClips[1]);
        }
        
        if (Input.GetKey(KeyCode.W))
        {
            anim.SetBool("isWalking", true);
        }
        else
            anim.SetBool("isWalking", false);

        if (Input.GetKey(KeyCode.R))
        {
            anim.SetBool("isRunning", true);
        }
        else
            anim.SetBool("isRunning", false);

        if (Input.GetKey(KeyCode.A))
        {
            anim.SetBool("isAttacking", true);
        }
        else
            anim.SetBool("isAttacking", false);

        if (Input.GetKey(KeyCode.G))
        {
            anim.SetBool("isDead", true);
        }*/
        /* if (Input.GetKeyDown(KeyCode.M))
         {
             GameObject tempRagDoll  = Instantiate(ragDollPrefab, this.transform.position, Quaternion.identity);
             tempRagDoll.transform.Find("Hips").GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward*1000);
             Destroy(this.gameObject);
             return;

         }*/


        if (target == null && GameStarts.isGameOver == false)
        {
            target = GameObject.FindGameObjectWithTag("Player");
            return;
        }
        switch (state)
        {
            case STATE.IDLE:
                if (CanSeePlayer())
                    state = STATE.CHASE;
                else if (Random.Range(0, 1000) < 5)
                {
                    state = STATE.WONDER;
                }




                break;
            case STATE.WONDER:
                if (!agent.hasPath)
                {
                    float randValueX = transform.position.x + Random.Range(-5f, 5f);
                    float randValueZ = transform.position.z + Random.Range(-5f, 5f);
                    //float ValueY = Terrain.activeTerrain.SampleHeight(new Vector3(randValueX, 0f, randValueZ));
                    float randValueY = transform.position.y + Random.Range(-10f, -8f);
                    Vector3 destination = new Vector3(randValueX, randValueY, randValueZ);
                    agent.SetDestination(destination);
                    agent.stoppingDistance = 0f;
                    agent.speed = walkingSpeed;
                    TurnOffAllTriggerAnim();
                    anim.SetBool("isWalking", true);
                }
                if (CanSeePlayer())
                {
                    state = STATE.CHASE;
                }
                else if (Random.Range(0, 1000) < 7)
                {
                    state = STATE.IDLE;
                    TurnOffAllTriggerAnim();
                    agent.ResetPath();
                }

                break;

            case STATE.CHASE:
                if (GameStarts.isGameOver)
                {
                    TurnOffAllTriggerAnim();
                    state = STATE.WONDER;
                    return;
                }
                agent.SetDestination(target.transform.position);
                agent.stoppingDistance = 2f;
                TurnOffAllTriggerAnim();
                anim.SetBool("isRunning", true);
                agent.speed = runningSpeed;
                if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                {
                    state = STATE.ATTACK;
                }
                if (CannotSeePlayer())
                {
                    state = STATE.WONDER;
                    agent.ResetPath();
                }


                break;

            case STATE.ATTACK:
                if (GameStarts.isGameOver)
                {
                    TurnOffAllTriggerAnim();
                    state = STATE.WONDER;
                    return;
                }
                //TurnOffAllTriggerAnim();
                anim.SetBool("isAttacking", true);
                transform.LookAt(target.transform.position);//Zombies should look at Player
                if (DistanceToPlayer() > agent.stoppingDistance + 2)
                {
                    state = STATE.CHASE;
                }
                anim.SetBool("isAttacking", true);
               // print("Attack State");
                break;

            case STATE.DEAD:
                /*GameObject tempRagDoll = Instantiate(ragDollPrefab, this.transform.position, Quaternion.identity);
                tempRagDoll.transform.Find("Hips").GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 1000);
                Destroy(this.gameObject);*/
                Destroy(agent);
                

                break;

            default:
                break;
        }
    }
    public void TurnOffAllTriggerAnim()//All animation are off
    {
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("isDead", false);
    }

    public bool CanSeePlayer()
    {
        if (DistanceToPlayer() < 10)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private float DistanceToPlayer()
    {

        if (GameStarts.isGameOver)
        {
            return Mathf.Infinity;
        }
        return Vector3.Distance(target.transform.position, this.transform.position);
    }

    public bool CannotSeePlayer()
    {
        if (DistanceToPlayer() > 20f)
        {
            return true;
        }
        else
            return false;
    }

    public void KillZombiee()
    {
        TurnOffAllTriggerAnim();
        anim.SetBool("isDead", true);
        state = STATE.DEAD;
    }
    int damageAmount = 5;
    /*
    public void DamagePlayer()
    {
        if (target != null)
        {
            target.GetComponent<PlayerController>().TakeHit(damageAmount);
        }


    }*/
}

public class GameStarts
{
    public static bool isGameOver = false;



}