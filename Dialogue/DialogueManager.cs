using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DialogueManager : MonoBehaviour {

    public Text nameText;
    public Text dialogueText;
    public Button NextTextButton;
    // public GameObject HUD;
    public GameObject DialogueBox;

    private BattleManager bm;


    // public Animator animator;

    private Queue<string> names;
    private Queue<string> sentences;


    // Use this for initialization
    void Awake () {
        // Debug.Log("DDD");


        // sentences = new Queue<string>();
        InitializeBattleManager();
    }

    private void InitializeBattleManager() {
        bm = GameObject.FindObjectOfType<BattleManager>();
    }

    public void StartDialogue (Dialogue dialogue)
    {

        // Debug.Log("DDD");
        DialogueBox.SetActive(true);

        // animator.SetBool("IsOpen", true);

        // nameText.text = dialogue.name;

        // Debug.Log(sentences);

        sentences = new Queue<string>();
        names = new Queue<string>();
        sentences.Clear();

// Debug.Log(dialogue.sentences.Count());
        for  (int i = 0; i < dialogue.sentences.Count(); i++) {
            // Debug.Log(i);
            sentences.Enqueue(dialogue.sentences[i]);
            names.Enqueue(dialogue.names[i]);
        }

        // foreach (string sentence in dialogue.sentences)
        // {
        // }

        DisplayNextSentence();
    }

    public void DisplayNextSentence ()
    {
        // Debug.Log(sentences.Count);
        if (sentences.Count == 0)
        {
         // Debug.Log("TESTY");

            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        string name = names.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence, name));
        Debug.Log(sentences.Count);
    }

    private IEnumerator TypeSentence (string sentence, string name)
    {
        // Debug.Log("TER");
        nameText.text = name;

        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }

        // Button instantButton = Instantiate(NextTextButton, new Vector3(0, -50, 0), Quaternion.identity);
        // instantButton.transform.SetParent(HUD.transform, false);
        // instantButton.onClick.AddListener(DisplayNextSentence);
        // instantButton.clicked += () => Debug.Log("Clicked!");
    }

    private void EndDialogue()
    {
        // Debug.Log('E');
        DialogueBox.SetActive(false);
        // animator.SetBool("IsOpen", false);
        bm.StartCombat();
    }

}