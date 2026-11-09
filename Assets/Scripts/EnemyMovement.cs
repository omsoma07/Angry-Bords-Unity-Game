using UnityEngine;
using UnityEngine.AI;
using TMPro;


public class EnemyMovement : MonoBehaviour{
    public Transform player;
    private NavMeshAgent navMeshAgent;

    private int lives = 5;

    public TextMeshProUGUI bossLives;
    public GameObject nest;
    public AudioClip tntSound;
    private AudioSource bossAudio;

    
    void Start(){
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetText();
        bossAudio = GetComponent<AudioSource>();
    }

    void Update(){
        if(player != null){
            navMeshAgent.SetDestination(player.position);
        }

        if(lives == 0){
            bossAudio.PlayOneShot(tntSound);
            nest.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }

    void SetText(){
        if(bossLives != null){
            bossLives.text = "Lives: " + lives.ToString();
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("TNT")){
            bossAudio.PlayOneShot(tntSound);
            other.gameObject.SetActive(false);
            lives--;
            SetText();
        }
    }
}
