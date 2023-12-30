using UnityEngine;
using UnityEngine.UI;

public class AnimatedBackground : MonoBehaviour
{
    Image bgImage;
    //Set this in the Inspector
    public Sprite[] bgSprites;
    // public Sprite bg1;
    // public Sprite bg2;


    public float changeTime = 2.0f;
    private int currentSlide = 0;
    private float timeSinceLast = 1.0f;

    void Start()
    {
        //Fetch the Image from the GameObject
        bgImage = GetComponent<Image>();
    }

    void Update()
    {
        // Debug.Log(bgSprites.Length);
        //Press space to change the Sprite of the Image
        // if (Input.GetKey(KeyCode.Space))
        // {
        //     bgImage.sprite = bgSprites[0];
        // }

        if (bgSprites.Length - 1 < currentSlide)
        {
            currentSlide = 0;
        }

        if (timeSinceLast > changeTime && currentSlide < bgSprites.Length)
        {
            bgImage.sprite = bgSprites[currentSlide];
            // guiTexture.texture = slides[currentSlide];
            // guiTexture.pixelInset = new Rect(-slides[currentSlide].width/20, -slides[currentSlide].height/20, slides[currentSlide].width, slides[currentSlide].height);
            timeSinceLast = 0.0f;
            currentSlide++;
        }
        timeSinceLast += Time.deltaTime;
    }
}