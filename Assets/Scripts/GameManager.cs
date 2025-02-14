using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [NonSerialized] public int outcome = 1;
    [SerializeField] private PhysicsMaterial _playerPM;
    [SerializeField] private GameObject _wallEntry;
    [SerializeField] private Animator _cameraFinalAct;
    [SerializeField] private GameObject _fireParticleSys; 
    [SerializeField] private GameObject _castle;
    [SerializeField] private GameObject [] _devilMage; 
    [SerializeField] private GameObject _priest;
    [SerializeField] private GameObject _priestFinalPoint;
    [SerializeField] private TextAsset _priestXML;
    [SerializeField] private Canvas _dialogueCanvas;
    [SerializeField] private Canvas _pauseGameUI;

    private GameObject _player;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _cameraFinalAct =GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
    }

    void OnEnable()
    {
        List<DialogueSystem> dialogueSystems = FindObjectsByType<DialogueSystem>(FindObjectsSortMode.None).ToList();
        foreach (var diasys in dialogueSystems)
        {
            diasys.DialogueCondition += HandleEvent;
        }    
    }

    void OnDisable()
    {
        List<DialogueSystem> dialogueSystems = FindObjectsByType<DialogueSystem>(FindObjectsSortMode.None).ToList();
        foreach (var diasys in dialogueSystems)
        {
            diasys.DialogueCondition -= HandleEvent;
        }    
    }

    public void HandleEvent(string condition)
    {
        Debug.Log(condition);
        switch(condition)
        {
            case "confess":
                _wallEntry.gameObject.SetActive(false);
                SetFritcion(1,1);
                outcome <<= 2;
                Debug.Log(outcome);
                break;
            case "conspiracy":
                GameObject.FindGameObjectWithTag("Guard").GetComponentsInChildren<DialogueSystem>()[0].metCondition = true;
                outcome <<= 2;
                Debug.Log(outcome);
                _wallEntry.gameObject.SetActive(true);
                SetFritcion(0.2f,0.2f);
                var priestDs = GameObject.FindGameObjectWithTag("Priest").GetComponentInChildren<DialogueSystem>();
                priestDs.metCondition = true;
                break;
            case "checkTruth":
                if (outcome != 4)
                {
                    priestDs = GameObject.FindGameObjectWithTag("Priest").GetComponentInChildren<DialogueSystem>();
                    priestDs.metCondition = false;
                    priestDs.nodeOffset = 1;
                    SetFritcion(1,1);
                    var priestMover = GameObject.FindGameObjectWithTag("Priest").GetComponentInChildren<PriestMover>();
                    priestMover.TranslatePriest();
                    _wallEntry.gameObject.SetActive(false);
                }
                break;
            case "pass":
                GameObject.FindGameObjectWithTag("Guard").GetComponent<Collider>().enabled = false;
                break;
            case "scrollPass":
                GameObject.FindGameObjectWithTag("Guard").GetComponentsInChildren<DialogueSystem>()[0].metCondition = true;
                FindAnyObjectByType<ScrollController>().DestroyScroll();
                SetFritcion(1,1);
                break;
            case "leavePrist":
                GameObject.FindGameObjectWithTag("Priest").GetComponentInChildren<PriestMover>().wandering = true;
                break;
            case "priestCome":
                _priest.GetComponent<PriestFinalMover>().wandering = true;
                _priest.GetComponent<PriestFinalMover>().PriestCame += TurnOnBox;
                break;
            case "demonlose":
                var a = _devilMage[2].GetComponentInChildren<Animator>();
                a.Play("Death");
                break;
            case "death":
                _player.GetComponent<PlayerMovementComp>().Dead();
                InvokeLoadLevel("");
                break;
            case "finaldeath":
                _player.GetComponent<PlayerMovementComp>().Dead(false);
                Invoke(nameof(InvokeCameraRemoval), 2f);
                InvokeLoadLevel("");
                break;
            default:
                break;
        }
    }

    private void SetFritcion(float dynamicFriction, float staticFriction)
    {
        _playerPM.dynamicFriction = dynamicFriction;
        _playerPM.staticFriction = staticFriction;
    }

    public void FinalStage()
    {
        Debug.Log(outcome);
        switch(outcome)
        {
            case 1:
                _player.GetComponent<PlayerMovementComp>().enabled = false;
                PlayCameraRemoval("CameraFinalRemotion");
                SaveEnd();
                InvokeLoadLevel("");
                break;
            case 4:
                var partSysTrans = _castle.transform.position;
                partSysTrans.y -= 7f;
                GameObject particleObj = Instantiate(_fireParticleSys, partSysTrans, Quaternion.identity);
                particleObj.transform.localScale = new Vector3(17f, 17f, 17f);
                _devilMage[0].GetComponent<DialogueSystem>().cameraClipname = "CameraFinalRemotionV2";
                _devilMage[0].SetActive(true);
                _player.GetComponent<PlayerMovementComp>().StopPlayer();

                Vector3 direction = _devilMage[0].transform.position - _player.transform.position;
                direction.y = 0;
                Quaternion newRotation = Quaternion.LookRotation(direction);
                _player.transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, 360f * Time.deltaTime);
        
                _devilMage[0].GetComponent<DialogueSystem>().DialogueEnded += PlayCameraRemoval;
                _devilMage[0].GetComponent<DialogueSystem>().DialogueEnded += InvokeLoadLevel;
                SaveEnd();
                break;
            case 8:
                _player.GetComponent<PlayerMovementComp>().StopPlayer();
                _devilMage[1].SetActive(true);
                SaveEnd();
                break;
            case 16:
                _player.GetComponent<PlayerMovementComp>().StopPlayer();
                _devilMage[2].SetActive(true);
                
                _priest.GetComponent<DialogueSystem>()._xml = _priestXML;
                _priest.SetActive(true);
                _priest.GetComponent<BoxCollider>().enabled = false;
                _priest.GetComponent<DialogueSystem>().DialogueEnded += PlayCameraRemoval;
                _priest.GetComponent<DialogueSystem>().DialogueEnded += InvokeLoadLevel;
                SaveEnd();
                break;
            default:
                break;
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void InvokeCameraRemoval()
    {
        PlayCameraRemoval();
    }
    private void PlayCameraRemoval(string cameraClip = "CameraFinalRemotion")
    {
        _cameraFinalAct.Play(cameraClip);
    }

    private void TurnOnBox()
    {
        _priest.GetComponent<BoxCollider>().enabled = true;
    }

    private void SaveEnd()
    {
        float endCount = PlayerPrefs.GetFloat("GameEnds");
        Debug.Log("Ends count:" + endCount);
        if (endCount >= 4) return;
        endCount++;
        PlayerPrefs.SetFloat("GameEnds", endCount);
        Debug.Log("Ends count:" + endCount);
    }

    private void InvokeLoadLevel(string holder)
    {
        Invoke(nameof(LoadTitles), 6f);
    }

    private void LoadTitles()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }


    public void PauseGame()
    {
        Time.timeScale = 0;
        _dialogueCanvas.gameObject.SetActive(false);
        _pauseGameUI.gameObject.SetActive(true);
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
        _pauseGameUI.gameObject.SetActive(false);
    }
}
