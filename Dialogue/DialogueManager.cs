using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DialogueManager : MonoBehaviour {

    public Image iconImage;
    public Text nameText;
    public Text dialogueText;
    public Button NextTextButton;
    public GameObject DialogueBox;
    public Canvas canvas;

    private BattleManager bm;


    // public Animator animator;

    private Queue<string> names;
    private Queue<string> sentences;
    private Queue<Sprite> icons;

    // void Awake () {
    //     InitializeBattleManager();
    //     // InitializeCamera();
    // }

    // private void InitializeBattleManager() {
    //     bm = GameObject.FindObjectOfType<BattleManager>();
    //     // Debug.Log("fsadfafd");
    // }

    // private void InitializeCamera()
    // {
    //     canvas.worldCamera = GameObject.FindObjectOfType<Camera>();
    // }

    public void StartDialogue (Dialogue dialogue)
    {
        DialogueBox.SetActive(true);

        // animator.SetBool("IsOpen", true);

        // nameText.text = dialogue.name;

        // Debug.Log(sentences);

        sentences = new Queue<string>();
        names = new Queue<string>();
        icons = new Queue<Sprite>();

        sentences.Clear();

        for  (int i = 0; i < dialogue.speeches.Count(); i++) {
            // Debug.Log(i);
            sentences.Enqueue(dialogue.speeches[i].sentence);
            names.Enqueue(dialogue.speeches[i].name);
            icons.Enqueue(dialogue.speeches[i].icon);
        }

        // if (dialogue.speeches.Count() > 0) {
            DisplayNextSentence();
        // }
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
        Sprite icon = icons.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(name, icon, sentence));
        // Debug.Log(sentences.Count);
    }

    private IEnumerator TypeSentence (string name, Sprite icon, string sentence)
    {
        // Debug.Log("TER");
        nameText.text = name;
        iconImage.sprite = icon;

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

        // Debug.Log(bm);

        // THIS IS A TERRIBLE WAY TO START COMBAT
        bm = GameObject.FindObjectOfType<BattleManager>();

        bm.StartCombat();
    }

}