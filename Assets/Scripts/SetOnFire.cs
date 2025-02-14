using UnityEngine;

public class SetOnFire : MonoBehaviour
{
    [SerializeField] private GameObject _fireParticleSys; 
    private GameManager gm;
    void Start()
    {
        gm = FindFirstObjectByType<GameManager>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && gm.outcome == 4)
        {
            var partSysTrans = GetComponent<BoxCollider>().transform.position;
            partSysTrans.y -= 2f;
            GameObject particleObj = Instantiate(_fireParticleSys, partSysTrans, Quaternion.identity);
            particleObj.transform.localScale = new Vector3(2f, 2f, 2f);
            gm.outcome <<= 1;
            gm.HandleEvent("checkTruth");
            GameObject.FindGameObjectWithTag("Guard").GetComponent<BoxCollider>().enabled = false;
            GameObject.FindGameObjectWithTag("Guard").GetComponent<GuardMover>().StartRunning();
        }      
    }
}
