using UnityEngine;

public class WaterDeath : MonoBehaviour
{
    private PlayerMovementComp _playerMC;
    void Start()
    {
        _playerMC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementComp>();
    }

    
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _playerMC.Dead();
        }
    }
}
