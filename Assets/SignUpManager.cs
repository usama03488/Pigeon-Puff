using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SignUpManager : MonoBehaviour
{
    public SaveFireBaseData firebaseSignUp;
    public TMP_InputField usernameInputField;
    public TMP_InputField email;
    public TMP_InputField username;
    public TMP_InputField password;
    public TMP_InputField password2;
    public TMP_Text Error;
    public GameObject errorObj;
    private void OnEnable()
    {
        username.text = "";
        email.text = "";
        password.text = "";
        password2.text = "";
        Error.text = " ";
        errorObj.SetActive(false);
    }
    public void OnSignUpButtonClicked()
    {
        string username = usernameInputField.text;
        firebaseSignUp.SignUpUser(username);
    }
}
