using UnityEngine;
using UnityEngine.UI;

public class AnimatedBackground : MonoBehaviour
{
    //Set these in the Inspector
    public Sprite[] bgSprites;
    public float changeTime;

    private BattleManager bm;
    private Image bgImage;
    private int currentSlide = 0;
    private float timeSinceLast = 1.0f;

    void Start()
    {
        //Fetch the Image from the GameObject
        bgImage = GetComponent<Image>();
        bm = GameObject.FindObjectOfType<BattleManager>();
    }

    void Update()
    {
        if (bm.GetDialoguePhase()) {
            if (bgSprites.Length - 1 < currentSlide) {
                currentSlide = 0;
            }
            if (timeSinceLast > changeTime && currentSlide < bgSprites.Length) {
                bgImage.sprite = bgSprites[currentSlide];
                timeSinceLast = 0.0f;
                currentSlide++;
            }
            timeSinceLast += Time.deltaTime;
        }
    }
}