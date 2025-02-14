using Unity.VisualScripting;
using UnityEngine;

public class NPCSetup : MonoBehaviour
{

    void Start()
    {
        Invoke(nameof(Deactivate), 2f);
    }

    private void Deactivate()
    {
        this.gameObject.SetActive(false);

    }
}
