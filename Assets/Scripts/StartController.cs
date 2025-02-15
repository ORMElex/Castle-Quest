using System;
using Unity.VisualScripting;
using UnityEngine;

public class StartController : MonoBehaviour
{
    private DialogueSystem _ds;
    void Start()
    {
        _ds = this.GetComponent<DialogueSystem>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        { 
            other.GetComponent<PlayerMovementComp>().StopPlayer();
            _ds.StartDialogue();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _ds.FinishDialogue();
            DestroyStartPoint();
        }
    }

    private void DestroyStartPoint()
    {
        if(this.gameObject) Destroy(this.gameObject);
    }
}
