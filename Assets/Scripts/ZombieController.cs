using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;

    public float walkingSpeed;

    enum STATE {IDLE, WANDER, ATTACK, CHASE, DEAD};
    STATE state = STATE.IDLE;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // モーションを止める関数
    public void TurnOffTrigger()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Run", false);
        animator.SetBool("Attack", false);
        animator.SetBool("Death", false);
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case STATE.IDLE:
                TurnOffTrigger();

                if (Random.Range(0, 5000) < 5)
                {
                    state = STATE.WANDER;
                }
                break;
            
            case STATE.WANDER:
                // 目的地があるかどうか
                if(!agent.hasPath)
                {
                    // 徘徊
                    float newX = transform.position.x + Random.Range(-5, 5);
                    float newZ = transform.position.z + Random.Range(-5, 5);

                    Vector3 NextPos = new Vector3(newX, transform.position.y, newZ);
                    agent.SetDestination(NextPos);
                    agent.stoppingDistance = 0;

                    TurnOffTrigger();

                    agent.speed = walkingSpeed;
                    animator.SetBool("Walk", true);
                }

                if (Random.Range(0, 5000) < 5)
                {
                    state = STATE.IDLE;
                    agent.ResetPath();
                }
                break;

        }
    }
}
