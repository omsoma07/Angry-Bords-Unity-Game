using UnityEngine;

public class ObstacleRotator : MonoBehaviour{
    public bool clockwise;

    void Update(){
        if(clockwise){
            transform.Rotate(new Vector3(75, 0, 0) * Time.deltaTime);
        }
        else{
            transform.Rotate(new Vector3(-75, 0, 0) * Time.deltaTime);
        }
    }
}
