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
        public int idx;
        public string name;
        [TextArea(3, 10)]
        public string sentence;
        public int setSprite;
    }

    [Header ("Visualize")]
    public GameObject CanvasBox;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image[] sprites;
    public Color[] colors;

    public Queue<Dialogue> dialogueQueue;
    public Dialogue[] dialogues;

    public TextAsset textAsset;

    public string[] characters;


    void Start()
    {
        dialogueQueue = new Queue<Dialogue>();
        ReadTextAsset();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            DisplayNextSentence(characters);
        }
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
                tmp = line.Split(new string[] {", "}, StringSplitOptions.None);
                characters = new string[tmp.Length];
                foreach (string text in tmp)
                {
                    characters[idx++] = text;
                }
                idx = 0;
                continue;
            }
            tmp = line.Split(new string[] {"--"}, StringSplitOptions.None);
            
            Dialogue dialogue = new Dialogue();
            dialogue.idx = int.Parse(tmp[0]) - 1;
            dialogue.name = characters[dialogue.idx];
            dialogue.setSprite = int.Parse(tmp[1]);
            dialogue.sentence = tmp[2];
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
        
        nameText.text = dialogue.name;
        string sentence = dialogue.sentence;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

        for (int idx = 0; idx < sprites.Length; idx++)
        {
            if (idx == dialogue.idx) sprites[idx].color = colors[0];
            else sprites[idx].color = colors[1];
        }
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
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
