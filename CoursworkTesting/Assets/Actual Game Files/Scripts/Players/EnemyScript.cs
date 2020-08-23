using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float enemyViewRadius = 10f;
    [SerializeField] private float enemyRotationSpeed = 3f;
    [Space]
    
    [Header("Objects")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private NavMeshAgent agent;

    // Update is called once per frame
    private void Update()
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
        var distance = Vector3.Distance(playerTransform.position, transform.position);
        
        if (!(distance <= enemyViewRadius)) return;
        
        agent.SetDestination(playerTransform.position);

        if(distance <= agent.stoppingDistance)
            MakeEnemyFaceTarget();
    }

    private void MakeEnemyFaceTarget()
    {
        var direction = (playerTransform.position - transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * enemyRotationSpeed);
    }
}
