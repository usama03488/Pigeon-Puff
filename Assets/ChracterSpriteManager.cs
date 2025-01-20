using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChracterSpriteManager : MonoBehaviour
{
    public List<GameObject> CharactersSprites;
    // Start is called before the first frame update
    void Start()
    {
        int index= PlayerPrefs.GetInt("CharacterIndex", 0);
        CharactersSprites[index].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

