using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Minimap : MonoBehaviour
{

    // Start is called before the first frame update
    public Image minimapDisplay;
    private float widthFactor = 420/2560f;
    private float heightFactor = 85/1600f;
    void Start()
    {
        // Store screen width and height
        int width = Screen.width;
        int height = Screen.height;
        Vector2Int upperRightCorner = new Vector2Int((int) (width*(1-widthFactor)), (int) (height*(1-heightFactor)));
        Vector2Int lowerLeftCorner = new Vector2Int((int) (width*widthFactor), (int) (height*heightFactor));
        Debug.Log($"Upper right corner: {upperRightCorner}, lower left corner: {lowerLeftCorner}");
        Vector2Int size = upperRightCorner - lowerLeftCorner;
        Debug.Log($"Size: {size}");
        // Set the size of the minimap canvas
        // minimapDisplay.rectTransform.sizeDelta = size/10;
        // Load the screenshot into the minimap
        minimapDisplay.sprite = LoadNewSprite(Application.persistentDataPath + "/Screenshot.png");
        // minimapDisplay.sprite = Sprite.Create(screenshot, new Rect(0, 0, screenshot.width, screenshot.height), new Vector2(0.5f, 0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
 public static Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f, SpriteMeshType spriteType = SpriteMeshType.Tight)
    {
 
        // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference
 
        Texture2D SpriteTexture = LoadTexture(FilePath);
        int width = SpriteTexture.width;
        int height = SpriteTexture.height;
        float a = 420/2560f;//572/3840f;//420/2560f;
        float b = 85/1600f;//70/2160f;//85/1600f;
        Rect rect = new Rect(a*width, b*height, width*(1-2*a), height*(1-2*b));//3090-572,2090-70);  //width*(1-2*a), height*(1-2*b));
        Debug.Log($"Rect size is: {rect}, texture size is: width: {width}, height: {height}");
        Sprite  NewSprite = Sprite.Create(SpriteTexture, rect , new Vector2(0, 0), PixelsPerUnit, 0 , spriteType); // Default back to new Rect(0, 0, SpriteTexture.width, SpriteTexture.height)
        Debug.Log($"sprite size {NewSprite.bounds}");
        return NewSprite;
    }
 
    public static Sprite ConvertTextureToSprite(Texture2D texture, float PixelsPerUnit = 100.0f, SpriteMeshType spriteType = SpriteMeshType.Tight)
    {
        // Converts a Texture2D to a sprite, assign this texture to a new sprite and return its reference
 
        Sprite NewSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), PixelsPerUnit, 0, spriteType);
 
        return NewSprite;
    }
 
    public static Texture2D LoadTexture(string FilePath)
    {
 
        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails
 
        Texture2D Tex2D;
        byte[] FileData;
 
        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        return null;                     // Return null if load failed
    }
}
