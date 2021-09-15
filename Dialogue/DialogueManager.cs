using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DialogueManager : MonoBehaviour {

    public Text nameText;
    public Text dialogueText;
    public Button NextTextButton;
    public GameObject DialogueBox;
    public Canvas canvas;

    private BattleManager bm;


    // public Animator animator;

    private Queue<string> names;
    private Queue<string> sentences;

    void Awake () {
        InitializeBattleManager();
        InitializeCamera();
    }

    private void InitializeBattleManager() {
        bm = GameObject.FindObjectOfType<BattleManager>();
    }

    private void InitializeCamera()
    {
        canvas.worldCamera = GameObject.FindObjectOfType<Camera>();
    }

    public void StartDialogue (Dialogue dialogue)
    {
        DialogueBox.SetActive(true);

        // animator.SetBool("IsOpen", true);

        // nameText.text = dialogue.name;

        // Debug.Log(sentences);

        sentences = new Queue<string>();
        names = new Queue<string>();
        sentences.Clear();

        for  (int i = 0; i < dialogue.sentences.Count(); i++) {
            // Debug.Log(i);
            sentences.Enqueue(dialogue.sentences[i]);
            names.Enqueue(dialogue.names[i]);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence ()
    {
        // Debug.Log(sentences.Count);
        if (sentences.Count == 0)
        {
        //  Debug.Log("TESTY");

            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        string name = names.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence, name));
        // Debug.Log(sentences.Count);
    }

    private IEnumerator TypeSentence (string sentence, string name)
    {
        // Debug.Log("TER");
        nameText.text = name;

        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;

            // 24 frames
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
        }
    }

    private void EndDialogue()
    {
        // Debug.Log('E');
        DialogueBox.SetActive(false);
        // animator.SetBool("IsOpen", false);
        bm.StartCombat();
    }

}