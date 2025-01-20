using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class localId : MonoBehaviour
{
    // Start is called before the first frame update
   public Button btn;
    [SerializeField] string ID="";
    private bool IsUnlocked = false;
    void Start()
    {
        
    }
    public void Set_Status(bool status)
    {
        IsUnlocked = status;
    }
    public bool Get_Status()
    {
        return IsUnlocked;
    }
    public string GetID()
    {
        return ID;
    }
    public void isInteractable(bool status)
    {
        btn = gameObject.GetComponent<Button>();
        btn.interactable=status;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
