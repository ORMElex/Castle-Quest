using System;
using UnityEngine;

public class PriestFinalMover : MonoBehaviour
{
    [SerializeField] private GameObject _priest;
    [SerializeField] private GameObject _wanderPoints;
    public event Action PriestCame;
    
    public bool wandering = false;
    

    void Update()
    {
        if (wandering) WanderToPoint();
    }


    private void WanderToPoint()
    {
        if (Vector3.Distance(_priest.transform.position, _wanderPoints.transform.position) < 1)
        {
            wandering = false;
            PriestCame?.Invoke();
        } 
        _priest.GetComponent<NPCController>().MoveToPoint(_wanderPoints.transform.position);
    }


    
}
