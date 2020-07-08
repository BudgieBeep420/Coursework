using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float enemyViewRadius = 10f;
    [SerializeField] private Transform PlayerTransform;
    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private float enemyRotationSpeed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemyFieldOfView();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyViewRadius);
    }

    private void CheckEnemyFieldOfView()
    {
        float distance = Vector3.Distance(PlayerTransform.position, transform.position);
        if (distance <= enemyViewRadius)
        {
            Agent.SetDestination(PlayerTransform.position);

            if(distance <= Agent.stoppingDistance)
            {
                // Attack player
                MakeEnemyFaceTarget();
            }
        }
    }

    private void MakeEnemyFaceTarget()
    {
        Vector3 direction = (PlayerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * enemyRotationSpeed);
    }
}
