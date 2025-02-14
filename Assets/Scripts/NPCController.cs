using System;
using Unity.VisualScripting;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Animator anim; 
    public DialogueSystem ds;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        ds = this.GetComponent<DialogueSystem>();
    }
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovementComp>().StopPlayer();
            ds.StartDialogue();
            ds.DialogueContinue += Talk;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            RotateToPoint(other.transform.position);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ds.FinishDialogue();
            ds.DialogueContinue -= Talk;
        }
    }
    
    void Talk()
    {
        if(anim)anim.Play("Talk");
    }

    public void MoveToPoint(Vector3 point)
    {
        RotateToPoint(point);
        transform.position = Vector3.MoveTowards(transform.position, point, _speed * Time.deltaTime);
    }

    public void DisableCollider()
    {
        if(GetComponent<BoxCollider>())GetComponent<BoxCollider>().enabled = false;
    }

    private void RotateToPoint(Vector3 point)
    {
        Vector3 direction = point - transform.position;
        direction.y = 0;

        Quaternion newRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, 360f * Time.deltaTime);
    }
}
