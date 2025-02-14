using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private Canvas _dialogueCanvas;
    [SerializeField] public TextAsset _xml;
    [SerializeField] private string _conditionToMet;
    [NonSerialized] public int nodeOffset = 0;
    [SerializeField] public string cameraClipname;

    public event Action<string> DialogueCondition;
    public event UnityAction DialogueContinue;
    public event UnityAction<string> DialogueEnded;
    public bool metCondition;
    private int currentNode = 0;
    private bool dialogueEnded;
    private GameManager gm;
    private TMP_Text npctext;
    private Dialogue dialogue;
    private PlayerMovementComp playerMC;
    private Node[] nd;
    private List<TMP_Text> answersText;
    private List<Button> buts = new List<Button>(3);

    void Start()
    {
        playerMC =  GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementComp>();
        gm = FindAnyObjectByType<GameManager>();
        dialogue = Dialogue.Load(_xml);
        nd = dialogue.nodes;
        npctext =_dialogueCanvas.gameObject.GetComponentInChildren<TMP_Text>(true);
        
        var answerHolder = GameObject.FindGameObjectWithTag("Answer");
        answersText = answerHolder.GetComponentsInChildren<TMP_Text>(true).ToList();   
    }

    public void StartDialogue()
    {
        playerMC.enabled = false;
        dialogueEnded = false;
        _dialogueCanvas.gameObject.SetActive(true);
        buts = _dialogueCanvas.GetComponentsInChildren<Button>(true).ToList();
        for (int i = 0; i < buts.Count; i++)
        {   
            int index = i;
            buts[i].onClick.RemoveAllListeners();
            buts[i].onClick.AddListener(()=> AnswerButton(buts[index]));
        }
        DrawDialogue();
    }

    public void FinishDialogue()
    {
        playerMC.enabled = true;
        DialogueEnded?.Invoke(cameraClipname);
        buts = _dialogueCanvas.GetComponentsInChildren<Button>().ToList();
        for (int i = 0; i < buts.Count; i++)
        {   
            buts[i].onClick.RemoveAllListeners();
        }
        _dialogueCanvas.gameObject.SetActive(false);
    }

    private void DrawDialogue()
    {
        if (dialogueEnded) FinishDialogue();
        if (currentNode == 6) currentNode += nodeOffset;
        DialogueContinue?.Invoke();
        npctext.text = GetNpcText();
        
        foreach (var answer in answersText)
        {
            answer.gameObject.SetActive(false);
        }

        var newAnswers = GetPlayerAnswers();
        for (int i = 0; i < newAnswers.Count; i++)
        {
            answersText[i].gameObject.SetActive(true);
            answersText[i].text = (i+1) + ". " + newAnswers[i];
        }
    }



    private void AnswerButton(Button button)
    {
        int buttonClicked = button.name[0] - '0'; //Converts 1st char of button name into int
        AnswerPressed(buttonClicked-1);
    }

    private void AnswerPressed(int answerButton)
    {
        if (nd[currentNode].answers[answerButton].end == "true") dialogueEnded = true;
        DialogueCondition?.Invoke(nd[currentNode].answers[answerButton].condition);

        currentNode = nd[currentNode].answers[answerButton].nextNode;
        DrawDialogue();
    }



    private string GetNpcText() => nd[currentNode].Npctext;

    private List<string> GetPlayerAnswers()
    {
        List<string> textAnswers = new List<string>(nd[currentNode].answers.Length);
        for (int i = 0; i < nd[currentNode].answers.Length; i++)
        {
            if(nd[currentNode].answers[i].condition == _conditionToMet && !metCondition) continue;
            textAnswers.Add(nd[currentNode].answers[i].text);
        }

        return textAnswers;
    }
}
