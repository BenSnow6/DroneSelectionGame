using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Minimap : MonoBehaviour
{
    // Start is called before the first frame update
    // Grab the screenshot from the main manager
    Texture2D screenshot = new Texture2D(2,2); //MainManager.Instance.screenshot;
    public Image minimapDisplay;
    void Start()
    {
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
        float a = 420/2560f;
        float b = 85/1600f;
        Rect rect = new Rect(a*width, b*height, width*(1-2*a), height*(1-2*b));
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
