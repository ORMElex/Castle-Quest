using UnityEngine;

public class ScrollController : MonoBehaviour
{
    private DialogueSystem ds;
    public bool toDestroy = false;
    void Start()
    {
        ds = this.GetComponent<DialogueSystem>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ds.StartDialogue();
        }
    }

     public void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ds.FinishDialogue();
            if(toDestroy) DestroyScroll();
        }
    }

    public void DestroyScroll()
    {
        ds.FinishDialogue();
        Destroy(this.gameObject);
    }
}
