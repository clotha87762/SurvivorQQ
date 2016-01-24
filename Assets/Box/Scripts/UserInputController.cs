using UnityEngine;
using System.Collections;


[RequireComponent(typeof(BoxController))]
public class UserInputController : MonoBehaviour
{

	public string KeyHorizontal  = "Horizontal1";
	public string KeyVertical    = "Vertical1";
	public string KeyResetCamera = "ResetCamera1";
	public string KeyJump		 = "Jump1";
	public string KeyAttack		 = "Attack1";
	public string KeyRoll		 = "Roll1";
	public string KeySprint		 = "Sprint1";
    public string KeyItem = "Item1";
	
    private BoxController mBox;
    public Transform mCamera;
    private Vector3 moveVector;
    private bool enableControl = true;
	private bool resettingCamera = false;
	private Vector3 cameraFoward;
	private Vector3 cameraRight;
	private float tempH, tempV, h, v;
	private bool keyH, keyV;
	private bool keyHAxis, keyVAxis;
	
    void Start()
    {
        if (Camera.main != null)
        {
            mCamera = Camera.main.transform;
        }
        else Debug.Log("userinput_controller::No main camera");
        mBox = transform.GetComponent<BoxController>();
    }
	
	void Update()
	{        
		if (enableControl)
        {
			if(!resettingCamera && Input.GetButtonDown (KeyResetCamera)) 
			{
				Debug.Log("resettingCamera:: ture");
				resettingCamera = true;
				cameraFoward = Vector3.Scale(mCamera.forward, new Vector3(1, 0, 1)).normalized;
				cameraRight = Vector3.Scale(mCamera.right, new Vector3(1, 0, 1)).normalized;
				keyHAxis = (h>0.0f);
				keyVAxis = (v>0.0f);
				keyH = Input.GetButton (KeyHorizontal);
				keyV = Input.GetButton (KeyVertical);
			}
        }
	}
	
	
    void FixedUpdate()
    {
        if (enableControl)
        {
            MoveInput();
            AttackInput();
        }
    }

    void MoveInput()
    {
        //jump if character is grounded and Jump key is pressed
        if (Input.GetButton(KeyJump)) mBox.Jump(); 
        if (Input.GetButtonDown(KeyRoll)) mBox.Roll();
        if (Input.GetButtonDown(KeyItem)) mBox.UseItem();


        ///////////////////////////////////
        h = 0;
        v = 0;
        if (!mBox.acting)
        {
            h=Input.GetAxis(KeyHorizontal);
            v=Input.GetAxis(KeyVertical);
        }
		bool tempkeyHAxis = (h>0.0f);
		bool tempkeyVAxis = (v>0.0f);
		//if(resettingCamera && (Mathf.Abs(h-tempH) > 0.1f || Mathf.Abs(v-tempV) > 0.1f)) 
		if(resettingCamera && ( (keyH != Input.GetButton (KeyHorizontal) || keyV != Input.GetButton (KeyVertical)) || 
								(tempkeyHAxis != keyHAxis || tempkeyVAxis != keyVAxis)
								) )
		{
			resettingCamera = false;
			Debug.Log("resettingCamera:: false");
			//h = 0.0f;
			//v = 0.0f;
		}

        //calculate move vector and pass to boxcontroller
        if (mCamera != null)
        {
            if(!resettingCamera)
			{
				cameraFoward = Vector3.Scale(mCamera.forward, new Vector3(1, 0, 1)).normalized;
				cameraRight = Vector3.Scale(mCamera.right, new Vector3(1, 0, 1)).normalized;
			}
            moveVector = v * cameraFoward + h * cameraRight;
        }
        else moveVector = v * Vector3.forward + h * Vector3.right;
        mBox.Move(moveVector, Input.GetButton(KeySprint));
        
        //////////////////////////////////
    }

    void AttackInput()
    {
        if (Input.GetButton(KeyAttack)) mBox.Attack();
    }

    public void SetEnable(bool set)
    {
        enableControl = set;
    }
}
