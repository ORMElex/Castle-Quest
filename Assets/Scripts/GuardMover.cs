using UnityEngine;

public class GuardMover : MonoBehaviour
{
    [SerializeField] private GameObject[] _guards;
    [SerializeField] private GameObject[] _pointsToRun;

    private bool running = false;
    private int curPoint = 0;

    void Update()
    {
        if (running) RunToPoint();
    }

    public void StartRunning()
    {
        running = true;
        foreach (var guard in _guards)
        {
            guard.GetComponent<Animator>().SetBool("Moving", true);
            guard.GetComponent<NPCController>().DisableCollider();
        }
    }

     private void RunToPoint()
    {
        Debug.Log($"Running: {running}, CurPoint: {curPoint}");
        foreach (var guard in _guards)
        {
            if (Vector3.Distance(guard.transform.position, _pointsToRun[curPoint].transform.position) < 1) curPoint += 1;
            if (curPoint >= _pointsToRun.Length) 
            {
                running = false;
                StopGuard();
                return;
            }
            guard.GetComponent<NPCController>().MoveToPoint(_pointsToRun[curPoint].transform.position);
        }
    }

    private void StopGuard()
    {
        Debug.Log($"Stoping Running: {running}, CurPoint: {curPoint}");
        foreach (var guard in _guards)
        {
            guard.GetComponent<Animator>().SetBool("Moving", false);
            guard.GetComponent<Animator>().SetBool("Talk", true);
        }
    }

}


