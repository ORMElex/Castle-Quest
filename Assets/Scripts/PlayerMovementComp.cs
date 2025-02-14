using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementComp : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _sensetivity;
    [SerializeField] private Transform _cameraTarget; 
    [SerializeField] private GameObject player; 
    [SerializeField] private Animator anim;


    private Rigidbody rb;
    private Vector2 movement;
    private Vector2 cameralook;
    private GameManager gm;
    private bool gamePaused = false;
    float xLook = 0;
    float yLook = 0;
    void Start()
    {
        gm = FindAnyObjectByType<GameManager>();
    }

    void FixedUpdate()
    {
        MoveChar();
        LookAround();
    }

    private void MoveChar()
    {
        float xMovement = movement.x * _speed * Time.deltaTime;
        float zMovement = movement.y * _speed * Time.deltaTime;
        if (xMovement!=0 || zMovement!=0)
        {
            if(anim) anim.SetBool("Moving", true);
            transform.rotation = Quaternion.Euler(0, _cameraTarget.transform.rotation.eulerAngles.y, 0);
            Vector3 direction = new Vector3(xMovement, 0, zMovement);
            transform.Translate(direction, Space.Self);
        }
        else
        {
            if(anim) anim.SetBool("Moving", false);
        }
    }

    private void LookAround()
    {
        xLook += cameralook.x;
        yLook -= cameralook.y; 

        yLook = Mathf.Clamp(yLook, -30, 35);
    
        Quaternion camrotation = Quaternion.Euler(yLook, xLook, 0);
        _cameraTarget.rotation = camrotation;

    }

    public void OnMove(InputValue Value)
    {
        movement = Value.Get<Vector2>();
    }

    public void OnLook(InputValue Value)
    {
        cameralook = Value.Get<Vector2>();
    }

    public void OnPause()
    {
        if(gamePaused) 
        {
            this.enabled = true;
            gamePaused = false;
            gm.UnpauseGame();
        }
        else
        {
           gamePaused = true;
            gm.PauseGame(); 
        }
    }

    public void StopPlayer()
    {
        this.enabled = false;
        anim.SetBool("Moving", false);
    }
    public void Dead(bool restartGame = true)
    {
        StopPlayer();
        anim.Play("Death");
        if (restartGame) Invoke(nameof(RestartGame), 3f);
    }
    

    private void RestartGame()
    {
        gm.RestartGame();
    }
}
