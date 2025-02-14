using UnityEngine;

public class FinalStage : MonoBehaviour
{
    private GameManager gm;
    void Start()
    {
        gm = FindFirstObjectByType<GameManager>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gm.FinalStage();
        }       
    }
}
