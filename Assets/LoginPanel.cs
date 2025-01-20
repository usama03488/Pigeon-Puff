using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LoginPanel : MonoBehaviour
{
    public TMP_InputField email;
    public TMP_InputField password;
    public TMP_Text Error;
    public GameObject errorObj;
    // Start is called before the first frame update
    private void OnEnable()
    {
        email.text = "";
        password.text = "";
        Error.text = " ";
        errorObj.SetActive(false);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
