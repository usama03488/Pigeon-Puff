using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class CharacterSelection : MonoBehaviour
{
   public List<GameObject> imageHighlighter;
    public int characterIndex = 0;
    public DatabaseReference reference;
    public Transform parentObj;
    public List<localId> idList;
    List<string> nftIds;
    private void OnEnable()
    {
        for(int i = 0; i < parentObj.childCount; i++)
        {
            idList.Add(parentObj.GetChild(i).GetComponent<localId>());
        }
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        string userid=FirebaseManager.instance.User.UserId;
        //var IdList = reference.Child("users").Child(userid).Child("Nfts").GetValueAsync();
        //reference.Child("users").Child(userid).Child("Nfts").GetValueAsync().ContinueWithOnMainThread(task =>
        //{
        //    if (task.IsCompleted)
        //    {
        //        DataSnapshot snapshot = task.Result;
        //         nftIds = new List<string>();

        //        if (snapshot != null && snapshot.Exists)
        //        {
        //            foreach (var child in snapshot.Children)
        //            {
        //                // Add each NFT ID (child key) to the list
        //                nftIds.Add(child.Value.ToString());
        //            }

        //            Debug.Log("NFT IDs: " + string.Join(", ", nftIds));
        //            VerifyNfts();
        //        }
        //        else
        //        {
        //            Debug.Log("No NFTs found for user.");
        //        }
        //    }
        //    else
        //    {
        //        Debug.LogError("Failed to retrieve NFTs: " + task.Exception);
        //    }
        //});
        
    }
   void VerifyNfts()
    {
        for(int j = 0; j < idList.Count; j++)
        {
            for(int x = 0; x < nftIds.Count; x++)
            {
                if (idList[j].GetID() == nftIds[x])
                {
                    Debug.Log("interactable true");
                    idList[j].isInteractable(true);
                    idList[j].Set_Status(true);
                }
               
            }
          
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        characterIndex = PlayerPrefs.GetInt("CharacterIndex",0);
        imageHighlighter[characterIndex].SetActive(true);
    }
    public void OnClick(int index) {
        if (index != characterIndex )
        {
            imageHighlighter[characterIndex].SetActive(false);
            characterIndex = index;
            imageHighlighter[characterIndex].SetActive(true);
        }
    }
    public void OnCharacterSelect()
    {
        PlayerPrefs.SetInt("CharacterIndex", characterIndex);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
