using UnityEngine;

public class PriestMover : MonoBehaviour
{
    [SerializeField] private GameObject _priestPoint;
    [SerializeField] private GameObject _priest;
    [SerializeField] private GameObject[] _wanderPoints;
    
    public bool wandering = false;
    private int curPoint = 0;
    

    void Update()
    {
        if (wandering) WanderToPoint();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            TranslatePriest();
        }
    }

    public void TranslatePriest()
    {
        _priest.transform.position = _priestPoint.gameObject.transform.position;
        _priest.transform.Rotate(Vector3.up, 180f);
        Destroy(GetComponent<Collider>());
    }

    private void WanderToPoint()
    {
        if (Vector3.Distance(_priest.transform.position, _wanderPoints[curPoint].transform.position) < 1) curPoint = (curPoint + 1) % _wanderPoints.Length;
        _priest.GetComponent<NPCController>().MoveToPoint(_wanderPoints[curPoint].transform.position);
    }


    
}
