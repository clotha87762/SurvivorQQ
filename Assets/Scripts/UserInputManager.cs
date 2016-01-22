using UnityEngine;
using System.Collections;

public class UserInputManager : MonoBehaviour {
	public static UserInputManager userInputManager;
    public static UserInputManager instance
    {
        get
        {
            if (!userInputManager)
            {
                userInputManager = FindObjectOfType(typeof(UserInputManager)) as UserInputManager;

                if (!userInputManager)
                {
                    Debug.LogError("userInputManager           ::There needs to be one active GameManager script on a GameObject in your scene.");
                }
            }
            return userInputManager;
        }
    }
	
	public string KeyHorizontal1  = "Horizontal1";
	public string KeyVertical1    = "Vertical1";
	public string KeyResetCamera1 = "ResetCamera1";
	public string KeyRotateCamera1= "RotateCamera1";
	public string KeyJump1		  = "Jump1";
	public string KeyAttack1      = "Attack1";	  	
	
	
	public string KeyHorizontal2  = "Horizontal2";
	public string KeyVertical2    = "Vertical2";
	public string KeyResetCamera2 = "ResetCamera2";
	public string KeyRotateCamera2= "RotateCamera2";
	public string KeyJump2		  = "Jump2";
	public string KeyAttack2      = "Attack2";
	void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
		
	}
	public static string getKeyHorizontal(int playerIndex)
	{
		return (playerIndex==1)? instance.KeyHorizontal1 : instance.KeyHorizontal2;
	}	
	public static string getKeyVertical(int playerIndex)
	{
		return (playerIndex==1)? instance.KeyVertical1 : instance.KeyVertical2;
	}	
	public static string getKeyResetCamera(int playerIndex)
	{
		return (playerIndex==1)? instance.KeyResetCamera1 : instance.KeyResetCamera2;
	}	
	public static string getKeyRotateCamera(int playerIndex)
	{
		return (playerIndex==1)? instance.KeyRotateCamera1 : instance.KeyRotateCamera2;
	}		
	public static string getKeyJump(int playerIndex)
	{
		return (playerIndex==1)? instance.KeyJump1 : instance.KeyJump2;
	}
	public static string getKeyAttack(int playerIndex)
	{
		return (playerIndex==1)? instance.KeyAttack1 : instance.KeyAttack2;
	}
}
