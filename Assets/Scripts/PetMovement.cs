using UnityEngine;
using UnityEngine.AI;

public class PetMovement : MonoBehaviour{
    public Transform player;
    private NavMeshAgent navMeshAgent;
    private Vector3 offset;

    
    void Start(){
        navMeshAgent = GetComponent<NavMeshAgent>();
        offset = transform.position - player.position;

    }

    void Update(){
        if(player != null){
            navMeshAgent.SetDestination(player.position + offset);
        }
    }
}
