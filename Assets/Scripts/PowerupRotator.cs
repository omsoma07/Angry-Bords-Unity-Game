using UnityEngine;

public class PowerupRotator : MonoBehaviour{
    void Update(){
        transform.Rotate(new Vector3(0, 50, 0) * Time.deltaTime);
    }
}
