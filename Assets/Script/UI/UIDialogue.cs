using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class UIDialogue : MonoBehaviour
{
    public string nextScene;

    [Serializable]
    public struct Dialogue
    {
        public int lIdx;
        public string lName;

        public int rIdx;
        public string rName;

        public string speaker;

        [TextArea(3, 10)]
        public string sentence;
        public int setSprite;
    }

    [Header ("Visualize")]
    public GameObject CanvasBox;
    public GameObject panel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image[] Lsprites;
    public Image[] Rsprites;
    public Color[] colors;

    public Queue<Dialogue> dialogueQueue;
    public Dialogue[] dialogues;

    public TextAsset textAsset;

    public string[] characters;

    private int lCurrentSprite = -1;
    private int rCurrentSprite = -1;

    private float holdTime = 0;
    private bool isHold;

    void Start()
    {
        Time.timeScale = 1f;
        dialogueQueue = new Queue<Dialogue>();
        ReadTextAsset();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            DisplayNextSentence(characters);
            isHold = true;
        }
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
        {
            holdTime = 0;
            isHold = false;
        }
        if (isHold) holdTime += Time.deltaTime;
        if (holdTime >= 2f) EndDialogue();
    }

    void ReadTextAsset()
    {
        string[] script = textAsset.text.Split(new string[] {"\n"}, StringSplitOptions.None);
        string[] tmp;
        int idx = 0;
        Dialogue[] dialogues = new Dialogue[script.Length-1];

        foreach (string line in script)
        {
            if (characters.Length == 0)
            {
                idx = 0;
                tmp = line.Split(new string[] {","}, StringSplitOptions.None);
                characters = new string[tmp.Length];
                foreach (string text in tmp)
                {
                    characters[idx++] = text;
                }
                if(characters.Length != 0)
                {
                    characters[0] = characters[0].Substring(1);
                }
                idx = 0;
                continue;
            }
            tmp = line.Split(new string[] {","}, StringSplitOptions.None);
            
            Dialogue dialogue = new Dialogue();
            dialogue.lIdx = int.Parse(tmp[0]);
            dialogue.lName = (dialogue.lIdx < 0)? "" : characters[dialogue.lIdx];

            dialogue.rIdx = int.Parse(tmp[1]);
            dialogue.rName = (dialogue.rIdx < 0)? "" : characters[dialogue.rIdx];

            dialogue.speaker = (int.Parse(tmp[2]) == 1)? dialogue.rName : dialogue.lName; 

            //dialogue.setSprite = int.Parse(tmp[1]);
            dialogue.sentence = tmp[3];
            dialogues[idx++] = dialogue;
        }

        StartDialogue(characters, dialogues);
    }

    public void StartDialogue(string[] characters, Dialogue[] dialogues)
    {    
        dialogueQueue.Clear();

        foreach (Dialogue dialogue in dialogues)
        {
            dialogueQueue.Enqueue(dialogue);
        }

        DisplayNextSentence(characters);
    }

    public void DisplayNextSentence(string[] characters)
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }
        Dialogue dialogue = dialogueQueue.Dequeue();
        
        nameText.text = dialogue.speaker;
        string sentence = dialogue.sentence;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

        if (lCurrentSprite != dialogue.lIdx)
        {
            if (lCurrentSprite >= 0) Lsprites[lCurrentSprite].gameObject.SetActive(false);
            if (dialogue.lIdx >= 0)
            {
                Lsprites[dialogue.lIdx].gameObject.SetActive(true);
            }

            lCurrentSprite = dialogue.lIdx;
        }

        if (rCurrentSprite != dialogue.rIdx)
        {
            if (rCurrentSprite >= 0) Rsprites[rCurrentSprite].gameObject.SetActive(false);
            if (dialogue.rIdx >= 0)
            {
                Rsprites[dialogue.rIdx].gameObject.SetActive(true);
            }

            rCurrentSprite = dialogue.rIdx;
        }

        if (dialogue.lName == dialogue.speaker)
        {
            if(lCurrentSprite >= 0) Lsprites[lCurrentSprite].color = colors[0];
            if(rCurrentSprite >= 0) Rsprites[rCurrentSprite].color = colors[1];
        }
        else
        {
            if(lCurrentSprite >= 0) Lsprites[lCurrentSprite].color = colors[1];
            if(rCurrentSprite >= 0) Rsprites[rCurrentSprite].color = colors[0];
        }

        // for (int idx = 0; idx < sprites.Length; idx++)
        // {
        //     if (idx == dialogue.idx) sprites[idx].color = colors[0];
        //     else sprites[idx].color = colors[1];
        // }
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }

    void EndDialogue()
    {
        StartCoroutine(SwitchScene());
    }

    IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(nextScene);
    }
}