using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirebaseManager : MonoBehaviour
{
    //private constants for encryption key generation
    //private const string secretKey = "1+uCIcHrnff6oF9rDjvxoUv0+gP+bJrVcs2RjZDLfFw=";
    //private const int keySize = 256;
    //private const int blockSize = 128;

    //public bool deleteUser;

    public static FirebaseManager instance;


    //public GameObject AdminPanel;
    //public GameObject Home;

    [Space(10)]
    [Header("Registered users Data")]
    public TMP_Text Users_Count;

    [Space(10)]
    [Header("Wallet Connection")]
    public GameObject ConnectPanel;

    public GameObject WalletOptionPanel;

    public static bool hasMutant;

    public static bool hasPuffs;

    bool hasSelectedConnectWalletOption = false;
    bool shouldConnectWallet = false;


    [Space(10)]
    [Header("Firebase Authentication Data")]
    public DependencyStatus _dependencyStatus;
    public FirebaseAuth Auth;
    public FirebaseUser User;
    public DatabaseReference reference;


    // public GameObject HomePanel;
    public GameObject SignupPanel;
    public GameObject LoginPanel;
    public GameObject MainMenuPanel;
    public GameObject ResetPassPanel;
    public MenuManager menu;
    [Space(10)]
    [Header("Create Account Screen Data")]
    public TMP_InputField Name;
    public TMP_InputField Email;
    public TMP_InputField Password;
    public TMP_InputField confirm_password;
    [Space(10)]
    [Header("Login Account Screen Data")]
    public TMP_InputField LoginEmail;
    public TMP_InputField LoginPassword;
    bool isLoading;

    [Space(10)]
    [Header("ResetPassword Screen Data")]
    public TMP_InputField ResetEmail;

    [Space(10)]
    [Header("Email Verification Data")]
    public GameObject EmailVerifyPanel;
    //public TMP_Text EmailMessageText;
    public TMP_Text Login_ErrorMessagePane;
    public TMP_Text ErrorMessagePane;
    [Space(10)]
    [Header("Users Data")]
    //public GameObject Data;
    //public GameObject DataSpawnPos;
    public Toggle RememberMe;
    //Initialization of the Firebase

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
      
       FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = FirebaseApp.DefaultInstance;
               Debug.Log("Initializer called 2");
               Auth = FirebaseAuth.DefaultInstance;
               reference = FirebaseDatabase.DefaultInstance.RootReference;
               Auth.StateChanged += AuthStateChanged;

               Debug.Log("Database ref: " + reference);
               if (Auth != null)
               {
                   Debug.Log("auth ref set ");
               }

               // Set a flag here to indicate whether Firebase is ready to use by your app.
           }
            else
            {
                Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
       

    }



    void InitializeFirebase()
    {
        Debug.Log("Initializer called 2");
        Auth = FirebaseAuth.DefaultInstance;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        Auth.StateChanged += AuthStateChanged;
        
        Debug.Log("Database ref: " + reference);
        if (Auth != null)
        {
            Debug.Log("auth ref set ");
        }

        // AuthStateChanged(this, null);

    }


    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        bool signedIn = Auth.CurrentUser != null;



        //if (!signedIn && User != null)

        //{
        //    User = Auth.CurrentUser;
        //    Debug.Log("Signed out:" + User.UserId);
        //}else if (!signedIn && User == null)
        //{

        //}
        User = null;
        if (Gamemanager.Instance.get_Login() == false)
        {
            LoginPanel.SetActive(true);
            MainMenuPanel.SetActive(false);

        }
        else
        {
            LoginPanel.SetActive(false);
            MainMenuPanel.SetActive(true);
            
        }
      

        /*
                if (signedIn && PlayerPrefs.GetInt("RememberMe")==1)
                {
                    User=Auth.CurrentUser;


                    Debug.Log("Signed in:" + User.UserId);
                    SceneManager.LoadScene("MainMenu");


                }
                else
                {
                    User = null;
                    LoginPanel.SetActive(true);
                }
        */
    }
    /* public void onToggleClick(Toggle rem)
     {
         if (rem.isOn)
         {
             PlayerPrefs.SetInt("RememberMe", 1);
         }
         else
         {
             PlayerPrefs.SetInt("RememberMe", 0);
         }
     }*/
    public void Register()
    {

        StartCoroutine(FirebaseRegisterAsync(Name.text, Email.text, Password.text, confirm_password.text));
    }

    IEnumerator FirebaseRegisterAsync(string name, string email, string password, string confirm_password)
    {


        if (name == null || email == null || password == null || confirm_password == null)
        {
            ErrorMessagePane.text = "Please fill all the fields";

            Debug.Log("Please fill all the fields");
           
          //  SignupPanel.SetActive(false);
            EmailVerifyPanel.SetActive(true);

        }

        if (password != confirm_password)
        {

            ErrorMessagePane.text = "Passwords do not match";

            Debug.Log("Passwords do not match");
          
           // SignupPanel.SetActive(false);
            EmailVerifyPanel.SetActive(true);
        }
        // Create user in Firebase Authentication
        
        else
        {
            var RegisterTask = Auth.CreateUserWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(() => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                FirebaseException firebaseException = RegisterTask.Exception.GetBaseException() as FirebaseException;

                AuthError authError = (AuthError)firebaseException.ErrorCode;


                string failMessage = "Registeration could not complete because ";

                switch (authError)
                {
                    case AuthError.InvalidEmail:
                        failMessage += "email is invalid";
                        ErrorMessagePane.text = failMessage;
                        break;

                    case AuthError.WrongPassword:
                        failMessage += "wrong password";
                        ErrorMessagePane.text = failMessage;
                        break;

                    case AuthError.MissingPassword:
                        failMessage += "password is missing";
                        ErrorMessagePane.text = failMessage;
                        break;

                    case AuthError.MissingEmail:
                        failMessage += "email is missing";
                        ErrorMessagePane.text = failMessage;
                        break;

                    case AuthError.EmailAlreadyInUse:
                        failMessage += "email already in use";
                       ErrorMessagePane.text = failMessage;
                        break;

                    default:
                        failMessage += "registeration failed";
                       ErrorMessagePane.text = failMessage;
                        break;
                }

                Debug.Log(failMessage + " " + authError);
                ErrorMessagePane.gameObject.SetActive(true);
              //  SignupPanel.SetActive(false);
             //   EmailVerifyPanel.SetActive(true);
            }

            else
            {
                //ErrorMessagePane.text = "";
                User = RegisterTask.Result.User;
                Debug.LogError("Username" + User);
                UserProfile userProfile = new UserProfile { DisplayName = name };

                var updateProfileTask = User.UpdateUserProfileAsync(userProfile);


                yield return new WaitUntil(() => updateProfileTask.IsCompleted);


                if (updateProfileTask.Exception != null)
                {
                    //Delete the user if an exception occurs
                    User.DeleteAsync();

                    Debug.LogError(updateProfileTask.Exception);

                    FirebaseException firebaseException = updateProfileTask.Exception.GetBaseException() as FirebaseException;

                    AuthError authError = (AuthError)firebaseException.ErrorCode;

                    string failMessage = "Account updation failed because ";

                    switch (authError)
                    {
                        case AuthError.InvalidEmail:
                            failMessage += "because email is invalid";
                            ErrorMessagePane.text = failMessage;
                            break;

                        case AuthError.WrongPassword:
                            failMessage += "because of wrong password";
                            ErrorMessagePane.text = failMessage;
                            break;

                        case AuthError.MissingEmail:
                            failMessage += "because of email is missing";
                            ErrorMessagePane.text = failMessage;
                            break;

                        case AuthError.MissingPassword:
                            failMessage += "because of password is missing";
                            ErrorMessagePane.text = failMessage;
                            break;

                        default:
                            failMessage += "server error, please try again";
                            ErrorMessagePane.text = failMessage;
                            break;

                    }
                    ErrorMessagePane.gameObject.SetActive(true);
                    Debug.Log(authError + " " + failMessage);

                  //  SignupPanel.SetActive(false);
                    //EmailVerifyPanel.SetActive(true);
                }

                else
                {
                    if (!User.IsEmailVerified)
                    {

                        //Debug.LogError("HAshiarf 1122");
                        string userId = User.UserId;

                        reference = FirebaseDatabase.DefaultInstance.RootReference;
                      //  var userID = reference.Child("users").SetValueAsync(userId);
                        // You can use a unique identifier for each user as the key
                        var usernameDBTask = reference.Child("users").Child(userId).Child("username").SetValueAsync(name);
                       //  Debug.LogError("userid "+userId+"    name" + name);
                        var emailDBTask = reference.Child("users").Child(userId).Child("email").SetValueAsync(email);
                        var NftCheck = reference.Child("users").Child(userId).Child("hasNFT").SetValueAsync(false);
                      var scoreDBTask = reference.Child("users").Child(userId).Child("Score").SetValueAsync("0");
                      var Default = reference.Child("users").Child(userId).Child("Nfts").Child("DefaultNft") .SetValueAsync(1);

                        yield return new WaitUntil(predicate: () => usernameDBTask.IsCompleted && emailDBTask.IsCompleted );
                        // Debug.LogError("Hashir after yeild");
                        if (usernameDBTask.Exception != null && emailDBTask.Exception != null )
                        {
                            Debug.LogWarning(message: $"failed to register task with {usernameDBTask.Exception} {emailDBTask.Exception}");

                        }
                        else
                        {
                            Debug.Log("Database updated!");
                        }

                       


                       SendVerificationEmail();


                    }
                    else
                    {
                        ErrorMessagePane.text = "";
                        Debug.Log("Account Registered Successfully " + User.DisplayName);
                    }



                    //Debug.Log(userId + "User ID");
                    //reference.Child(userId).Child("name").SetValueAsync(name);
                    //reference.Child(userId).Child("email").SetValueAsync(email);
                }

            }
        }
    }
    /*
    public string EncryptString(string plainText)
    {
        
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(secretKey);
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.BlockSize = blockSize;
            aesAlg.Padding = PaddingMode.PKCS7;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            byte[] encryptedBytes = null;
            using (var msEncrypt = new System.IO.MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(plainTextBytes, 0, plainTextBytes.Length);
                    csEncrypt.FlushFinalBlock();
                    encryptedBytes = msEncrypt.ToArray();
                }
            }

            byte[] combinedBytes = new byte[aesAlg.IV.Length + encryptedBytes.Length];
            Array.Copy(aesAlg.IV, 0, combinedBytes, 0, aesAlg.IV.Length);
            Array.Copy(encryptedBytes, 0, combinedBytes, aesAlg.IV.Length, encryptedBytes.Length);

           

            return Convert.ToBase64String(combinedBytes);
        }
    }

    public string DecryptString(string secretKey, string cipherText)
    {
        byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(secretKey);
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.BlockSize = blockSize;
            aesAlg.Padding = PaddingMode.PKCS7;

            byte[] iv = new byte[aesAlg.IV.Length];
            byte[] cipherBytes = new byte[cipherTextBytes.Length - aesAlg.IV.Length];

            Array.Copy(cipherTextBytes, iv, aesAlg.IV.Length);
            Array.Copy(cipherTextBytes, aesAlg.IV.Length, cipherBytes, 0, cipherBytes.Length);

            aesAlg.IV = iv;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (var msDecrypt = new System.IO.MemoryStream(cipherBytes))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }*/

    public void GetAllUsers()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("users");

        //reference.GetValueAsync().contin


        reference.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Database fetch encountered an error: " + task.Exception);
                return;
            }
            if (task.IsCompleted)
            {

                DataSnapshot snapshot = task.Result;

                Users_Count.text = snapshot.ChildrenCount.ToString();

                foreach (var child in snapshot.Children)
                {
                    Debug.Log(child.Child("email").Value.ToString());
                    string userId = child.Key;
                    Debug.Log(userId);
                    string name = child.Child("username").Value.ToString();
                    string email = child.Child("email").Value.ToString();
                    //GameObject obj = Instantiate(Data, DataSpawnPos.transform);
                    //Data data = obj.GetComponent<Data>();
                    //data.nameText.text = name;
                    //data.EmailText.text = email;
                    //data.password = password;
                    //data.key = userId;

                    // ... handle user data as needed
                }

            }

        });
    }
    public void GotoSignupForm()
    {
        LoginPanel.SetActive(false);
        SignupPanel.SetActive(true);
        ResetPassPanel.SetActive(false);
        EmailVerifyPanel.SetActive(false);
    }
    /* public void AutoLogin()
     {
         if (User != null && User.IsEmailVerified)
         {
             SceneManager.LoadScene("Menu");
         }

         else
         {
             onBackbtnClick();
             //SignupPanel.SetActive(false);
             //LoginPanel.SetActive(true);
         }
     }*/

    //Delete user from firebase and authentication

    public void onBackbtnClick()
    {
        LoginPanel.SetActive(true);
        SignupPanel.SetActive(false);
        ResetPassPanel.SetActive(false);
        EmailVerifyPanel.SetActive(false);
    }
    public void Login()
    {
        //PlayerPrefs.SetString("ShopWeapons", string.Empty);

        //if (LoginEmail.text == "admin" && LoginPassword.text == "0000")
        //{
        //    JoinPanel.SetActive(false);
        //    HomePanel.SetActive(true);
        //    AdminPanel.SetActive(true);
        //    GetAllUsers();


        //}


        //else
        //{
        StartCoroutine(FirebaseLoginAsync(LoginEmail.text, LoginPassword.text));
        //}
    }

    //public void deleteUserFromFirebase()
    //{
    //    StartCoroutine(DeleteUserFromFirebaseAsync());
    //}

    //IEnumerator DeleteUserFromFirebaseAsync()
    //{
    //    string userId = User.UserId.ToString();

    //    var usernameDBTask = reference.Child("users").Child(userId).Child("username").SetValueAsync(null);
    //    var emailDBTask = reference.Child("users").Child(userId).Child("email").SetValueAsync(null);
    //    var passwordDBTask = reference.Child("users").Child(userId).Child("password").SetValueAsync(null);

    //    yield return new WaitUntil(predicate: () => usernameDBTask.IsCompleted && emailDBTask.IsCompleted && passwordDBTask.IsCompleted);

    //    Users_Count.text = (int.Parse(Users_Count.text) - 1).ToString();

    //    var DeleteUserTask = User.DeleteAsync();

    //    yield return new WaitUntil(() => DeleteUserTask.IsCompleted);

    //    deleteUser = false;
    //}
    public IEnumerator FirebaseLoginAsync(string email, string password)
    {

        var LoginTask = Auth.SignInWithEmailAndPasswordAsync(email, password);
       
        yield return new WaitUntil(() => LoginTask.IsCompleted);
        Login_ErrorMessagePane.text = "Email is not verified";
        Login_ErrorMessagePane.gameObject.SetActive(true);
        if (LoginTask.Exception != null)
        {
          //  Debug.LogError("excepetionexcepitonasndjsadas"+LoginTask.Exception);

            FirebaseException firebaseException = LoginTask.Exception.GetBaseException() as FirebaseException;

            AuthError authError = (AuthError)firebaseException.ErrorCode;

            string failedMessage = "Your Authentication failed because ";
         
            switch (authError)
            {
                case AuthError.InvalidEmail:
                    failedMessage += "your email is invalid";
                    Login_ErrorMessagePane.text = failedMessage;
                    Login_ErrorMessagePane.gameObject.SetActive(true);
                    break;

                case AuthError.WrongPassword:
                    failedMessage += "your password is invalid";
                    Login_ErrorMessagePane.text = failedMessage;
                    Login_ErrorMessagePane.gameObject.SetActive(true);
                    break;

                case AuthError.MissingEmail:
                    failedMessage += "your email is missing";
                    Login_ErrorMessagePane.text = failedMessage;
                    Login_ErrorMessagePane.gameObject.SetActive(true);
                    break;

                case AuthError.MissingPassword:
                    failedMessage += "your password is missing";
                    Login_ErrorMessagePane.text = failedMessage;
                    Login_ErrorMessagePane.gameObject.SetActive(true);
                    break;

                default:
                    failedMessage += "we could not found user";
                    Login_ErrorMessagePane.text = failedMessage;
                    Login_ErrorMessagePane.gameObject.SetActive(true);
                    break;

            }


           // Debug.Log("herheherehrehrer"+failedMessage);
  
            SignupPanel.SetActive(false);
           // EmailVerifyPanel.SetActive(true);
        }
        else
        {
          
            if (Auth.CurrentUser.IsEmailVerified)
            {
                LoginPanel.SetActive(false);
                MainMenuPanel.SetActive(true);
                Gamemanager.Instance.Set_Login(true);
                User = LoginTask.Result.User;

                //Debug.Log("{0} You are successfully logged In: " + User.DisplayName);

                //PlayerPrefs.SetInt("RememberMe", 1);

                if (!isLoading)
                {
                    isLoading = true;

                    //LoadScene("MainMenu");
                   // ConnectPanel.SetActive(true);
                      //ConnectWallet();
                    StartCoroutine(CheckConnection());
                }
            }
            else
            {
                SendVerificationEmail();
            }
        }
    }

    public void RetrieveWeaponsArray(string userId)
    {
        //DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference("users").Child(userId).Child("weapons");
        Debug.Log("userid" + userId);

        reference.Child("users").Child(userId).Child("weapons").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error
                Debug.LogError("Error fetching data");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                List<object> weaponsList = snapshot.Value as List<object>;

                Debug.Log("Getting weapons data");
                foreach (DataSnapshot weapon in snapshot.Children)
                {
                    //Debug.Log(int.Parse(weapon.Value.ToString()));
                }

                if (weaponsList != null)
                {
                    // Convert the list of objects to a list of integers
                    List<int> weapons = new List<int>();
                    foreach (var weapon in weaponsList)
                    {
                        weapons.Add(System.Convert.ToInt32(weapon));
                    }

                    // Convert the list to a comma-separated string
                    string listString = string.Join(",", weapons);

                    // Store the string in PlayerPrefs
                    PlayerPrefs.SetString("ShopWeapons", listString);
                    PlayerPrefs.Save();

                    //Debug.Log("Weapons array retrieved: " + string.Join(", ", weapons));
                }
                else
                {
                    Debug.LogWarning("Weapons array is null or not found.");
                }
            }
            else
            {
                Debug.LogError("Failed to retrieve weapons array: " + task.Exception);
            }

            PlayerPrefs.SetInt("HasConnectedWallet", 1);
            LoadScene("MainMenu");
        });
    }

  public  void ConnectWallet()
    {
        StartCoroutine(WaitForWalletConnection());
    }

    //string checkNFTPath = "users/" + User.UserId + "/hasNFT";
    IEnumerator CheckConnection()
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        var checkNFTTask = reference.Child("users").Child(User.UserId).Child("hasNFT").GetValueAsync();
        yield return new WaitForSeconds(1f);
       
        if (checkNFTTask.IsCompleted)
        {
            DataSnapshot mutantsnapshot = checkNFTTask.Result;
            if (mutantsnapshot != null && mutantsnapshot.Exists)
            {
                hasMutant = (bool)mutantsnapshot.Value;
            
                        Debug.Log("Mutant: " + hasMutant);

                if (hasMutant)
                {
                 
                    MainMenuPanel.SetActive(true);
                    ConnectPanel.SetActive(false);

                }
                else
                {
                    ConnectPanel.SetActive(true);
                }
            }
            else
            {
                ConnectPanel.SetActive(true);
            }
        }
       
    }
    // Add or update a player's data
    public void AddOrUpdatePlayer(string userId, string username, int score)
    {
        reference.Child("users").Child(userId).Child("username").SetValueAsync(username).ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                Debug.Log("Username added/updated successfully.");
            }
            else
            {
                Debug.LogError("Failed to add/update username: " + task.Exception);
            }
        });

        reference.Child("users").Child(userId).Child("score").SetValueAsync(score).ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                Debug.Log("Score added/updated successfully.");
            }
            else
            {
                Debug.LogError("Failed to add/update score: " + task.Exception);
            }
        });
    }
    IEnumerator WaitForWalletConnection()
    {
        // check if user has connected the wallet the proceed without asking for user to connect

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        var checkNFTTask = reference.Child("users").Child(User.UserId).Child("hasNFT").GetValueAsync();

     //   var CheckMutantTask = reference.Child("users").Child(User.UserId).Child("hasMutantNft").GetValueAsync();

      //  var CheckPuffsTask = reference.Child("users").Child(User.UserId).Child("hasPuffsNft").GetValueAsync();

        yield return new WaitForSeconds(1f);

        //Check whether user has mutant nft bool check
        #region comment

        //if (CheckMutantTask.IsCompleted)
        //{
        //    if (CheckMutantTask.Exception != null)
        //    {
        //        Debug.Log("Error checking Mutant: " + CheckMutantTask.Exception);
        //    }

        //    DataSnapshot mutantsnapshot = CheckMutantTask.Result;

        //    if (mutantsnapshot != null && mutantsnapshot.Exists)
        //    {
        //        hasMutant = (bool)mutantsnapshot.Value;

        //        Debug.Log("Mutant: " + hasMutant);

        //        /*if (mutant)
        //        {
        //            bl_PlayerSelector.Data.AllPlayers[15].Unlockability.UnlockAtLevel = 0;

        //            Debug.Log("Players: " + bl_PlayerSelector.Data.AllPlayers.Count());
        //        }*/
        //    }
        //}


        /* //if (CheckPuffsTask.IsCompleted)
         //{
         //    if (CheckPuffsTask.Exception != null)
         //    {
         //        Debug.Log("Error checking Puffs: " + CheckPuffsTask.Exception);
         //    }

         //    DataSnapshot puffssnapshot = CheckPuffsTask.Result;

         //    if (puffssnapshot != null && puffssnapshot.Exists)
         //    {
         //        hasPuffs = (bool)puffssnapshot.Value;

         //        Debug.Log("Puffs: " + hasPuffs);

         //        /*if (mutant)
         //        {
         //            bl_PlayerSelector.Data.AllPlayers[15].Unlockability.UnlockAtLevel = 0;

         //            Debug.Log("Players: " + bl_PlayerSelector.Data.AllPlayers.Count());
         //        }*/
        //    }
        //}
        #endregion
        if (checkNFTTask.IsCompleted)
        {
            if (checkNFTTask.Exception != null)
            {
                Debug.LogError("Error checking NFT: " + checkNFTTask.Exception);
                yield break; // Exit coroutine on error
            }

            DataSnapshot snapshot = checkNFTTask.Result;

            if (snapshot != null && snapshot.Exists)
            {
                bool hasNFT = (bool)snapshot.Value;

                Debug.Log("Inside check Is Nft"+hasNFT);

                if (hasNFT)
                {
                    Debug.Log("NFT Found");
                    //
                    ConnectPanel.SetActive(false);
                    //  RetrieveWeaponsArray(User.UserId);
                    //LoadScene("MainMenu");
                    yield break;
                }
            }
        }



        if (false == shouldConnectWallet)
        {
            PlayerPrefs.SetInt("HasConnectedWallet", 0);
            //  ConnectPanel.SetActive(false);
            // LoadScene("MainMenu");
            yield break;
        }

        hasSelectedConnectWalletOption = false;
        shouldConnectWallet = false;

       // WalletOptionPanel.SetActive(false);

        ConnectPanel.SetActive(true);

        Application.OpenURL("https://pigeon-puffs.vercel.app/?modal=rewardPoolSelection");
    
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        while (true) // Keep checking indefinitely until hasNFT is true
        {
            checkNFTTask = reference.Child("users").Child(User.UserId).Child("hasNFT").GetValueAsync();
            Debug.Log("While Loop executing");
            yield return new WaitForSeconds(1f);
         
            if (checkNFTTask.IsCompleted)
            {
                if (checkNFTTask.Exception != null)
                {
                    Debug.LogError("Error checking NFT: " + checkNFTTask.Exception);
                    yield break; // Exit coroutine on error
                }

                DataSnapshot snapshot = checkNFTTask.Result;

                if (snapshot != null && snapshot.Exists)
                {
                    bool hasNFT = (bool)snapshot.Value;
                    Debug.Log("NFT Found  inside the while loop: "+ hasNFT);
                    if (hasNFT)
                    {
                        
                        ConnectPanel.SetActive(false);
                        break; // Exit coroutine when NFT is found
                    }
                }
            }
            if (hasSelectedConnectWalletOption)
            {
                if (false == shouldConnectWallet)
                {
                    PlayerPrefs.SetInt("HasConnectedWallet", 0);
                    ConnectPanel.SetActive(false);
                    //LoadScene("MainMenu");
                    yield break;
                }
            }
        }

      


    }


    //Send verification
    void SendVerificationEmail()
    {
        StartCoroutine(SendVerificationEmailAsync());
    }


    IEnumerator SendVerificationEmailAsync()
    {
        if (User != null)
        {
            var emailVerifyTask = User.SendEmailVerificationAsync();
         
            yield return new WaitUntil(() => emailVerifyTask.IsCompleted);

            if (emailVerifyTask.Exception != null)
            {
                FirebaseException firebaseException = emailVerifyTask.Exception.GetBaseException() as FirebaseException;

                AuthError authError = (AuthError)firebaseException.ErrorCode;

                string failMessage = "Failure! Verification could not complete, Please try again.";

                switch (authError)
                {
                    case AuthError.Cancelled:
                        failMessage = "Failure! verification cancelled";
                        Login_ErrorMessagePane.text = failMessage;
                        break;

                    case AuthError.TooManyRequests:
                        failMessage = "Failure! too many messages";
                        Login_ErrorMessagePane.text = failMessage;
                        break;

                    case AuthError.InvalidRecipientEmail:
                        failMessage = "Failure! verification mail could not be sent because email is invalid";
                        Login_ErrorMessagePane.text = failMessage;
                        break;
                }

                Debug.Log(failMessage + " " + authError);
                Login_ErrorMessagePane.gameObject.SetActive(true);
                updateUIForEmailVerification(false, null, failMessage);
            }

            else
            {
                updateUIForEmailVerification(true, User.Email, null);
            }
        }
    }
    public void LogOut_Id()
    {
        Gamemanager.Instance.Set_Login(false);
        SceneManager.LoadScene(0);
    }
    void updateUIForEmailVerification(bool isEmailSent, string emailId, string errorMessage)
    {
        EmailVerifyPanel.SetActive(true);
        LoginPanel.SetActive(false);
        SignupPanel.SetActive(false);
        ResetPassPanel.SetActive(false);
       

        //if (isEmailSent)
        //{
        //    ErrorMessagePane.text = "Email sent successfully! please log into your email account and click the verification link at" + emailId + ".Once your email is verified,press back to go back to the login screen.";
        //}

        //else
        //{
        //    ErrorMessagePane.text = "Email sending failed! please try again " + errorMessage;
        //}
    }

    public void IncrementScore(string userId, int incrementAmount)
    {
        if (reference == null)
        {
            Debug.LogError("Firebase not initialized");
            return;
        }

        DatabaseReference userScoreRef = reference.Child("users").Child(userId).Child("Score");

        // Run a transaction to increment the score
        userScoreRef.RunTransaction(transaction =>
        {
            if (transaction.Value == null)
            {
                // Node doesn't exist, create it with the incrementAmount as a string
                transaction.Value = incrementAmount.ToString();
            }
            else
            {
                // Node exists, increment the existing value (assuming it's a string representing a number)
                int currentValue = int.Parse((string)transaction.Value);
                transaction.Value = (currentValue + incrementAmount).ToString();
            }

            return TransactionResult.Success(transaction);
        }).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error
                Debug.LogError("Error incrementing data: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                // Increment successful
                Debug.Log("Data incremented successfully");
            }
        });
    }
    public IEnumerator Set_Username()
    {

        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

        var checkNFTTask = reference.Child("users").Child(User.UserId).Child("username").GetValueAsync();
        yield return new WaitForSeconds(1f);

        if (checkNFTTask.IsCompleted)
        {
            DataSnapshot mutantsnapshot = checkNFTTask.Result;
            if (mutantsnapshot != null && mutantsnapshot.Exists)
            {
                //  hasMutant = (bool)mutantsnapshot.Value;

                Debug.Log("Mutant: " + hasMutant);

                if (hasMutant)
                {
                    Debug.Log("Wallet already connected");
                    MainMenuPanel.SetActive(true);
                    ConnectPanel.SetActive(false);

                }
                else
                {
                    ConnectPanel.SetActive(true);
                }
            }
            else
            {
                ConnectPanel.SetActive(true);
            }
        }
    }
    public void upload_Score(int score)
    {
        
  

        if (User != null)
        {
            string userId = User.UserId;

            // Update the score in the database
          //  DatabaseReference userScoreRef = reference.Child("users").Child(User.UserId).Child("Score").SetValueAsync(score);
           reference.Child("users").Child(userId).Child("Score").SetValueAsync(score)
                .ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.LogError($"Failed to update score: {task.Exception}");
                    }
                    else if (task.IsCompleted)
                    {
                        Debug.Log("Score updated successfully!");
                    }
                });
        }
        else
        {
            Debug.LogWarning("No user is logged in. Cannot update score.");
        }
    }
    public void SelectConnectWalletOption(bool choice)
    {
        hasSelectedConnectWalletOption = true;
        shouldConnectWallet = choice;
    }
    private void OnDestroy()
    {
        Auth.StateChanged -= AuthStateChanged;
    }
 
    //Delete the user

    /*public void DeleteUser()
    {
        StartCoroutine(DeleteUserAsync());
    }

    IEnumerator DeleteUserAsync()
    {
       // User = User.
    }
    */


    //Reset Password

    /*  void ResetPassword()
      {
          StartCoroutine(PasswordReset(ResetEmail.text));
      }

      IEnumerator PasswordReset(string email)
      {
          var PasswordResetTask = User.UpdatePasswordAsync()
      }
    */




    public void ResetPasswordButton()
    {
        StartCoroutine(OnResetPasswordButtonClick());
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private IEnumerator OnResetPasswordButtonClick()
    {
        string email = ResetEmail.text;
  

        ErrorMessagePane.text = "";
        if (string.IsNullOrEmpty(email))
        {
            ErrorMessagePane.text = "Please enter your email address.";
            yield break;
        }

        if (!IsValidEmail(email))
        {
            ErrorMessagePane.text = "The email address is badly formatted.";
            yield break;
        }
        EmailVerifyPanel.SetActive(true);
        var resetPassTask = Auth.SendPasswordResetEmailAsync(email);

        yield return new WaitUntil(() => resetPassTask.IsCompleted);

        // Send password reset email
        if (resetPassTask.IsCanceled)
        {
            Debug.LogError("SendPasswordResetEmailAsync was canceled.");
            ErrorMessagePane.text = "Password reset request canceled.";
        }
        else if (resetPassTask.IsFaulted)
        {
            Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + resetPassTask.Exception);
            ErrorMessagePane.text = "An error occurred. Please check your email and try again.";
        }
        else
        {
            // Success
            Debug.Log("Password reset email sent successfully.");
            ErrorMessagePane.text = "Password reset email sent. Please check your inbox.";
               ResetPassPanel.SetActive(false);
            LoginPanel.SetActive(true);


        }

        ErrorMessagePane.gameObject.SetActive(true);
       
        
    }


    public void LoadScene(string name)
    {
        //SceneManager.LoadScene(name);
    }

    bool alreadyOpened = false;
    int valueMoved = 0;
    Vector3 originalPos;
    public void OnOpenedInput(int value)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if(alreadyOpened)
        {
            OnClosedInput(valueMoved);
        }
        alreadyOpened = true;
        valueMoved = value;
        originalPos = LoginPanel.transform.localPosition;
        Vector3 newPosition = LoginPanel.transform.localPosition;
        newPosition.y += value;  // Move the input field up to avoid the keyboard
        LoginPanel.transform.localPosition = newPosition;
#endif
    }

    public void OnClosedInput(int value)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        alreadyOpened = false;
        LoginPanel.transform.localPosition = originalPos;
        //Vector3 newPosition = LoginPanel.transform.localPosition;
        //newPosition.y -= value;  // Move the input field up to avoid the keyboard
        //LoginPanel.transform.localPosition = newPosition;
#endif
    }

    //
    bool RegisteralreadyOpened = false;
    int registerValueMoved = 0;
    Vector3 RegisterOriginalPos;
    private FirebaseApp app;

    public void OnRegisterOpenedInput(int value)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (RegisteralreadyOpened)
        {
            OnRegisterClosedInput(registerValueMoved);
        }
        RegisteralreadyOpened = true;
        registerValueMoved = value;
        RegisterOriginalPos = LoginPanel.transform.localPosition;
        Vector3 newPosition = SignupPanel.transform.localPosition;
        newPosition.y += value;  // Move the input field up to avoid the keyboard
        SignupPanel.transform.localPosition = newPosition;
#endif
    }

    public void OnRegisterClosedInput(int value)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        RegisteralreadyOpened = false;
        SignupPanel.transform.localPosition = RegisterOriginalPos;
        //Vector3 newPosition = SignupPanel.transform.localPosition;
        //newPosition.y -= value;  // Move the input field up to avoid the keyboard
        //SignupPanel.transform.localPosition = newPosition;
#endif
    }
}
