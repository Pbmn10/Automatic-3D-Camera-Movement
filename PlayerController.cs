using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour {

#region Variables

	AudioSource jumpSound;
	public GameObject LookAtCenter;
	public Animator anim;
	
	public bool isAttached;
	public bool isBossBattle;
	public Transform bossTarget;
	
	
	public Transform camera;
	public int lives;
	public float cameraSensitivity;
	public int controlMode; //0 1 2 3 4 5
	
	private float speed_horizontal;
	private float speed_vertical;
	public float acc_ground;
	public float acc_air;
	
	public float lookAtLimit;
	private float lookAtRadius;
	public float lookAtSpeed;
	private Vector3 oldLookAtPos;
	
	private bool toJump;
	
	public float movespeed;
	public float walkspeed;
	//public Rigidbody theRB;
	public float jumpforce;
	public int jumpCount;
	
	public CharacterController controller;
	public float gravityScale;
	public float fallSpeedLimit;
	
	private float consecJumps = 1;
	private float jump_timer = 0;
	public float start_y;
	public float cameraSpeed;
	
	public float customTime;
	private float camdist;
	private float camera_distance_limit;
	
	public Vector3 moveDirection;
	public Vector3 addedDirection;
	
	public Vector3 transformMove;
	
	private float dist;
	private float movePoint_x;
	private float movePoint_z;
	private float stableAngle;
	
	private float saveDirection;
	
    private float checkAngle;
	public float circleHori;
	public float circleUpi;
	private Color32 color;
	private Vector3 moveCam;
	
	private Renderer rend;
	private float colorSlide;
	
	private int lookingRotation; //-4 to 4
	public float aproxRotateHori;
	public float horiTolerance;
	public float extraCircleHori;
	public float newCircleHori;
	public float aproxNewRotateHori;
	public double rotationAnglePrecise;
	
	public bool hover;
	
	private Vector3 hitNormal;
	private bool isGrounded;
	public float slopeLimit;
	public float slideFriction;
	public float slideSpeedLimit;
	
	public float cameraDistance;
	private int cameraDistanceMode;
	private float cameraDistanceTarget;
	private bool cameraDistanceIsMoving;
	
	
	private float inputHorizontal;
	private float inputVertical;
	private float pinputSpeed;
	
	private float oldCircleHori;
	private float lockedCircleHori;
	private float lastLook;
	
	public bool isCamLocked;
	public bool isCamLockedTrack;
	private float trackDiff;
	private float newTrackDiff;
	private bool trackStopped;
	private float vector_val;
	
	private float newCamDistX;
	private float newCamDistZ;
	
	private bool wasCameraMovedHoriz;
	
	private float heading;
	private float savedCircleHori;
	
	public bool isRotating;
	public float rotationSpeed;
	
	private RaycastHit hit;
	private float heading_x;
	private float heading_z;
	private Vector3 normalZX;
	private Vector3 saveSlideVector;
	
	public bool is2D;
	public bool is3D;
	
	public bool isOnFP;
	
	public bool foundFloorZX;
#endregion

	void Start () {		
		//QualitySettings.vSyncCount = 1;
		
		
		isBossBattle = false;
		saveSlideVector = new Vector3(0f,0f,0f);
		QualitySettings.vSyncCount = 1;
		
		//Screen.SetResolution(640, 480, true);
        //Application.targetFrameRate = 30;
        Application.targetFrameRate = 60;
		
		isOnFP = false;
		
		speed_horizontal = 0f;
		speed_vertical = 0f;
		
		lookAtRadius = 0f;
		oldLookAtPos = new Vector3(0f,0f,0f);
		
		isAttached = false;
		
		trackStopped = false;
		wasCameraMovedHoriz = false;
		
		normalZX = new Vector3(0f,1f,0f);
		
		addedDirection = new Vector3(0f,0f,0f);
		isRotating = false;
		cameraDistanceIsMoving = false;
		cameraDistanceMode = 1;
		
		toJump = false;
		
		anim = this.GetComponent<Animator>();
		jumpSound = GetComponent<AudioSource>();
		
		//Screen.SetResolution(640, 480, true);
		lookingRotation = 0;
		rend = GetComponent<Renderer>();
		circleHori = 0;
		circleUpi = Mathf.PI/2;
		savedCircleHori = circleHori;
		start_y = this.transform.position.y;
		controller = GetComponent<CharacterController>();
		//theRB = GetComponent<Rigidbody>();
		//dist = camera.transform.parent.GetComponent<CameraController>().offset.z;
		camdist = 10f;
		camera_distance_limit = 10f;
		
		colorSlide = 0f;
		aproxRotateHori = 0f;
		horiTolerance = 0.00001f;
		
		foundFloorZX = false;
		cameraDistanceTarget = 10f;
		
		//m_Animator = gameObject.GetComponent<Animator>();
		//Time.timeScale = 0;
	}
	
	void updateAnimations(){
	
		bool isWalking = anim.GetBool("isWalking");
		bool isRunning = anim.GetBool("isRunning");
		bool isJumping = anim.GetBool("isJumping");
		
		if (controller.isGrounded){
			if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0f || Mathf.Abs(Input.GetAxis("Vertical")) > 0f){
				anim.SetBool("isWalking", true);
			
			}
			else {
				anim.SetBool("isWalking", false);
			}
			
			if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.75f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.75f){
				anim.SetBool("isRunning", true);
			
			}
			else {
				anim.SetBool("isRunning", false);
			}
			
			if (isJumping == true){
				
				anim.SetBool("isJumping", false);
				
			}

		}
		
		
	}
	void updateRotation(){
		
		//Vector3 postionToLookAt;
		//positionToLookAt.x = Input.GetAxis("Horizontal")
		
		
		
	}
	void updateLookAtCenter(){
		//LookAtCenter.transform.position += new Vector3(moveDirection.x,0f,moveDirection.z) * Time.deltaTime/10f;
		
		LookAtCenter.transform.position += new Vector3(Mathf.Cos(circleHori)*Input.GetAxis("Horizontal")*movespeed*inputHorizontal* Time.deltaTime*lookAtSpeed + Mathf.Sin(circleHori)*Input.GetAxis("Vertical")*movespeed* Time.deltaTime*lookAtSpeed,
											moveDirection.y,
											-Mathf.Sin(circleHori)* Input.GetAxis("Horizontal")*movespeed*inputHorizontal* Time.deltaTime*lookAtSpeed + Mathf.Cos(circleHori)*Input.GetAxis("Vertical")*movespeed* Time.deltaTime*lookAtSpeed);
					
				
												  //new Vector3(Mathf.Sin(lastLook)*1f,0f,Mathf.Cos(lastLook)*1f);
				
				//float lasin = -lookAtLimit*Mathf.Sin(lastLook);
				//float lacos = lookAtLimit*Mathf.Cos(lastLook);
		if (LookAtCenter.transform.position.x > lookAtLimit+transform.position.x){
			LookAtCenter.transform.position = new Vector3(lookAtLimit+transform.position.x,LookAtCenter.transform.position.y,LookAtCenter.transform.position.z);
		}
		else if (LookAtCenter.transform.position.x < -lookAtLimit+transform.position.x){
			LookAtCenter.transform.position = new Vector3(-lookAtLimit+transform.position.x,LookAtCenter.transform.position.y,LookAtCenter.transform.position.z);
		}
		if (LookAtCenter.transform.position.z > lookAtLimit+transform.position.z){
			LookAtCenter.transform.position = new Vector3(LookAtCenter.transform.position.x,LookAtCenter.transform.position.y,lookAtLimit+transform.position.z);
		}
		else if (LookAtCenter.transform.position.z < -lookAtLimit+transform.position.z){
			LookAtCenter.transform.position = new Vector3(LookAtCenter.transform.position.x,LookAtCenter.transform.position.y,-lookAtLimit+transform.position.z);
		}
		//if (LookAtCenter.transform.position.y > 0.1f +transform.position.y+0.86f){
		//	LookAtCenter.transform.position = new Vector3(LookAtCenter.transform.position.x,0.1f +transform.position.y+0.86f,LookAtCenter.transform.position.z);
		//}
		//else if (LookAtCenter.transform.position.y < -0.1f +transform.position.y+0.86f){
		//	LookAtCenter.transform.position = new Vector3(LookAtCenter.transform.position.x,-0.1f +transform.position.y+0.86f,LookAtCenter.transform.position.z);
		//}
				
		float diff_x = Mathf.Abs(transform.position.x-LookAtCenter.transform.position.x);
		float diff_z = Mathf.Abs(transform.position.z-LookAtCenter.transform.position.z);
		lookAtRadius = Mathf.Sqrt(diff_x*diff_x+diff_z*diff_z);
		
		
	}
	void OnControllerColliderHit (ControllerColliderHit hit) {
		hitNormal = hit.normal;
	}
	
	void updateZXRotation(){
		//Vector3 normal;
		bool isAbnormalHit = false;
		Ray ray = new Ray(transform.position, new Vector3(0f,-1f,0f));
		if (Physics.Raycast(ray, out hit, 2f)){
			foundFloorZX = true;
			GameObject block = hit.transform.gameObject;
			normalZX = hit.normal;
			if (Mathf.Abs(normalZX.x) > 0.1f || Mathf.Abs(normalZX.z) > 0.1f) {
				isAbnormalHit = true;
				controller.radius = 0.5f;
				if (block.tag == "Ladder" || block.tag == "LadderWall"){
					slopeLimit = 90f;
					controller.slopeLimit = 90f;
				}
				else {
					slopeLimit = 40f;
					controller.slopeLimit = 65f;
				}
			}
			else {
				controller.radius = 0.2f;
				
			}
			
			
			//isGrounded = true;
			if (Vector3.Angle (Vector3.up, hitNormal) <= slopeLimit) {
				isGrounded = true;
				
				//if (block.tag == "Ladder"){
					//slopeLimit = 90f;
				//}
				//else {
				//	slopeLimit = 40f;
				//}
					
				//controller.detectCollisions = false;
			}
			else {
				isGrounded = false;
				//slopeLimit = 40f;
				
				//transform.position += saveSlideVector*Time.deltaTime*200f;
				//controller.radius = 0.2f;
				//saveSlideVector = new Vector3(0f,0f,0f);
				
				//controller.detectCollisions = true;
			}
			 //= (Vector3.Angle (Vector3.up, hitNormal) <= slopeLimit);
			//Debug.Log(Vector3.Angle (Vector3.up, hitNormal));
		
		}
		else {
			normalZX = new Vector3(0f,1f,0f);
			foundFloorZX = false;
			
		}
		
		
		if (isAbnormalHit == true) {
			//float llangle = Mathf.Atan2(lastLook,normalZX.y);
			//float llangle_x = Mathf.Atan2(lastLook,normalZX.y);
			//float llangle_z = Mathf.Atan2(lastLook,normalZX.y);
			//heading_x = Mathf.Atan2(normalZX.z,normalZX.y);
			//heading_z = Mathf.Atan2(normalZX.x,normalZX.y);
			
			//heading_x = llangle_x;
			//heading_z = llangle_z;
			
			//heading_x = Mathf.Sin(llangle);
			//heading_z = Mathf.Cos(llangle);
			
			heading_x = Mathf.Atan2(normalZX.z,normalZX.y);
			heading_z = Mathf.Atan2(normalZX.x,normalZX.y);
		
		}
		else {
			heading_x = Mathf.Atan2(normalZX.z,normalZX.y);
			heading_z = Mathf.Atan2(normalZX.x,normalZX.y);
			
		}
		//transform.rotation=Quaternion.Euler(heading_x*Mathf.Rad2Deg,transform.rotation.y,heading_z*Mathf.Rad2Deg);
		
		
	}
	void Update() {
		
		if (!camera.GetComponent<CameraController>().isGoingBackFromFirstPerson && !camera.GetComponent<CameraController>().isGoingToFirstPerson &&
					  !camera.GetComponent<CameraController>().isFirstPerson) {
			isOnFP = false;						  
		}
		else {
			isOnFP = true;
		}
		
		
		
		updateZXRotation();
		
		
		
		
		
		if (controller.isGrounded ) {
			if (toJump == false){
				jumpforce = 17f;
				jumpCount = 1;
			}
		}
		else {
			jumpforce = 12f;			
		}
		
		if (controller.isGrounded && jumpCount > 0) {
				//if(isRotating == false) {
				//	newCircleHori = 0f;
				//	extraCircleHori = 0f;
				//}
				//moveDirection = new Vector3(Input.GetAxis("Horizontal")*movespeed, 0f, Input.GetAxis("Vertical")*movespeed);
				
				if (Input.GetButtonDown("Jump") ) {
					toJump = true;
					jumpCount -= 1;
					
				}
		}
		else if (jumpCount > 0) {
			if (Input.GetButtonDown("Jump") ) {
					toJump = true;
					jumpCount -= 1;
					
					
			}
			
		}
		if (Input.GetButton("CamLockTrack") ){
				
				
			float newTrackDiff;
			if (isCamLockedTrack == false) {
				trackDiff = oldCircleHori - lastLook;
					
				if (!(oldCircleHori < lastLook+0.2 && oldCircleHori >= lastLook-0.2 ))
					trackStopped = false;
					
				//vector_val = Mathf.Cos(oldCircleHori)*Mathf.Sin(lastLook) - Mathf.Cos(lastLook)*Mathf.Sin(circleHori);
					
			}
			if (Mathf.Abs(Input.GetAxis("Horizontal"))>0f ||Mathf.Abs(Input.GetAxis("Vertical"))>0f) {
					
				//newTrackDiff = oldCircleHori - lastLook;
					
				if (!(oldCircleHori <= lastLook+0.2 && oldCircleHori >= lastLook-0.2 ))
					trackStopped = false;
				
			}
			if ((Mathf.Abs(Input.GetAxis("Horizontal"))<0.1f && Mathf.Abs(Input.GetAxis("Vertical"))<0.1f) &&
			wasCameraMovedHoriz == false && trackStopped == false) {
				//trackStopped = true;
			}
			//if (Mathf.Abs(newTrackDiff - trackDiff)
			vector_val = Mathf.Cos(oldCircleHori)*Mathf.Sin(lastLook) - Mathf.Cos(lastLook)*Mathf.Sin(oldCircleHori);
			isCamLockedTrack = true;
		}
		else {
			isCamLockedTrack = false;
				
		}
		
		
		
		if (Input.GetButtonDown("FirstPersonCam") && controller.isGrounded) {
			
			if (camera.GetComponent<CameraController>().isGoingToFirstPerson == false) {
				
				camera.GetComponent<CameraController>().isGoingToFirstPerson = true;
				camera.GetComponent<CameraController>().isGoingBackFromFirstPerson = false;
				camera.GetComponent<CameraController>().oldPosition = camera.transform.position;
				//camera.GetComponent<CameraController>().target = this.transform;
				camera.GetComponent<CameraController>().target = camera.GetComponent<CameraController>().FPTarget;
				camera.GetComponent<CameraController>().oldRotation = new Vector3(camera.transform.eulerAngles.x,camera.transform.eulerAngles.y,camera.transform.eulerAngles.y);
			
			}
			else if (camera.GetComponent<CameraController>().isGoingBackFromFirstPerson == false){
				camera.GetComponent<CameraController>().isGoingToFirstPerson = false;
				camera.GetComponent<CameraController>().isGoingBackFromFirstPerson = true;
				camera.GetComponent<CameraController>().isFirstPerson = false;
				camera.GetComponent<CameraController>().target = camera.GetComponent<CameraController>().oldTarget;
				camera.GetComponent<CameraController>().oldRotation = new Vector3(camera.transform.eulerAngles.x,camera.transform.eulerAngles.y,camera.transform.eulerAngles.y);
			}
			
			
		}
		
		
		
		
	}
	// Update is called once per frame
	void FixedUpdate () {
		
		
		
			updateAnimations();
			
			//UPDATE INPUT
			
			Vector3 speedTemp = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
			float mag = speedTemp.magnitude;
			
			
			float acc;
			if (controller.isGrounded == true){
				acc = acc_ground;
			}
			else {
				acc = acc_air;
			}
			
			if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2) {
				speed_horizontal += Input.GetAxis("Horizontal")*Time.deltaTime*acc;	
				speed_horizontal = speed_horizontal;
				
				if (speed_horizontal > movespeed*Mathf.Abs(Input.GetAxis("Horizontal"))) {
					speed_horizontal = movespeed*Mathf.Abs(Input.GetAxis("Horizontal"));
				}
				else if(speed_horizontal < -movespeed*Mathf.Abs(Input.GetAxis("Horizontal"))) {
					speed_horizontal = -movespeed*Mathf.Abs(Input.GetAxis("Horizontal"));
				}
			}
			else {
				if (speed_horizontal > 0f) {
					speed_horizontal -= Time.deltaTime*acc*100f;	
					if (speed_horizontal <= 0f)
						speed_horizontal = 0f;
				}
				else if(speed_horizontal < 0f) {
					speed_horizontal += Time.deltaTime*acc*100f;	
					if (speed_horizontal > 0f)
						speed_horizontal = 0f;
				}
			}
			
			
			if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2) {
				speed_vertical += Input.GetAxis("Vertical")*Time.deltaTime*acc;
				speed_vertical = speed_vertical;
				
				if (speed_vertical > movespeed*Mathf.Abs(Input.GetAxis("Vertical"))) {
					speed_vertical = movespeed*Mathf.Abs(Input.GetAxis("Vertical"));
				}
				else if(speed_vertical < -movespeed*Mathf.Abs(Input.GetAxis("Vertical"))) {
					speed_vertical = -movespeed*Mathf.Abs(Input.GetAxis("Vertical"));
				}
			}
			else {
				if (speed_vertical > 0f) {
					speed_vertical -= Time.deltaTime*acc*100f;	
					if (speed_vertical <= 0f)
						speed_vertical = 0f;
				}
				else if(speed_vertical < 0f) {
					speed_vertical += Time.deltaTime*acc*100f;
					if (speed_vertical >= 0f)
						speed_vertical = 0f;
					
				}
				
			}
			
			Vector3 finalTemp = new Vector3(speed_horizontal, 0f, speed_vertical);
			if (finalTemp.magnitude > 0.1f) {
				anim.speed = finalTemp.magnitude/movespeed * 1.5f;//*1.5f;
			}
			else {
				anim.speed = 1;	
			}
			
			
			/*if (Input.GetButtonDown("CamLock") ){
				if (isCamLocked == false) {
					isCamLocked = true;
				}
				else {
					isCamLocked = false;
				}
				
			}*/
			if ((Input.GetButton("CamLock") || Input.GetButton("CamLockTrack")) && !isOnFP ){
				isCamLocked = true;
			}
			else {
				//if (isBossBattle == false){
					isCamLocked = false;
				//}
				
			}
			
			if (oldCircleHori < 0f)
				oldCircleHori = 2f*Mathf.PI;
			
			if (circleHori < 0f)
				circleHori = 2f*Mathf.PI;
			
			if (oldCircleHori > 2f*Mathf.PI)
				oldCircleHori = 0f;
			
			if (circleHori > 2f*Mathf.PI)
				circleHori = 0f;
			
			//Debug.Log(oldCircleHori);
		
			if (isCamLocked == false) {
				oldCircleHori = circleHori;
			}
			else{
				circleHori = oldCircleHori;
				
				
				
			}
		
			inputHorizontal = Input.GetAxis ("Horizontal");
			inputVertical = Input.GetAxis ("Vertical");
			
			pinputSpeed = new Vector2(inputHorizontal, inputVertical).sqrMagnitude;
			
			inputHorizontal = Mathf.Sin(pinputSpeed);
			inputVertical = Mathf.Cos(pinputSpeed);
			
			if(is3D){
				
			//isGrounded = (Vector3.Angle (Vector3.up, hitNormal) <= slopeLimit);
			
			
				
			if(isRotating == true && !isOnFP) {
				if(extraCircleHori!=0){
					//rotationAnglePrecise += Time.deltaTime*extraCircleHori/180*Mathf.PI;
					
					
					
					//if (Input.GetAxis("CameraLock") <= 0.1) {
						newCircleHori += Time.deltaTime*extraCircleHori;
					//}
					if(newCircleHori > 360.0f || newCircleHori < -360.0f)
						//rotationAnglePrecise = 0;
						newCircleHori = 0f;
				} else{
					//newCircleHori = 0f;
					//aproxNewRotateHori = 0f;
				}
			}
				if((Input.GetAxis("Vertical") > 0 || Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0|| Input.GetAxis("Vertical") < 0) ) {
					
					transform.Rotate(0f, -(newCircleHori-aproxNewRotateHori), 0f);
					//transform.rotation = Quaternion.RotateTowards(transform.rotation, camera.rotation, 50f);
					aproxNewRotateHori = newCircleHori;
					//lookingRotation = 0;
				}
			//
			
			if(Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") == 0) {
				if(lookingRotation!=0){				
					//transform.Rotate(0f, 45f*(-lookingRotation), 0f);
				}
				transform.Rotate(0f, (circleHori-aproxRotateHori)/Mathf.PI*180,0f);
				aproxRotateHori = circleHori;
				lookingRotation = 0;
			}
			else if(Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") > 0) {
				if(lookingRotation!=2){				
					//transform.Rotate(0f, 45f*(2-lookingRotation), 0f);
				}
				transform.Rotate(0f, (circleHori-aproxRotateHori)/Mathf.PI*180,0f);
				aproxRotateHori = circleHori;
				lookingRotation = 2;
			}
			else if(Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") < 0) {
				if(lookingRotation!=-2){				
					//transform.Rotate(0f, 45f*(-2-lookingRotation), 0f);
				}
				transform.Rotate(0f, (circleHori-aproxRotateHori)/Mathf.PI*180,0f);
				aproxRotateHori = circleHori;
				lookingRotation = -2;
			}
			else if(Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") > 0) {
				if(lookingRotation!=1){				
					//transform.Rotate(0f, 45f*(1-lookingRotation), 0f);
				}
				transform.Rotate(0f, (circleHori-aproxRotateHori)/Mathf.PI*180,0f);
				aproxRotateHori = circleHori;
				lookingRotation = 1;
			}
			else if(Input.GetAxis("Vertical") > 0 && Input.GetAxis("Horizontal") < 0) {
				if(lookingRotation!=-1){				
					//transform.Rotate(0f, 45f*(-1-lookingRotation), 0f);
				}
				transform.Rotate(0f, (circleHori-aproxRotateHori)/Mathf.PI*180,0f);
				aproxRotateHori = circleHori;
				lookingRotation = -1;
			}
			else if(Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") > 0) {
				if(lookingRotation!=3){				
					//transform.Rotate(0f, 45f*(3-lookingRotation), 0f);
				}
				transform.Rotate(0f, (circleHori-aproxRotateHori)/Mathf.PI*180,0f);
				aproxRotateHori = circleHori;
				lookingRotation = 3;
			}
			else if(Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") < 0) {
				if(lookingRotation!=-3){				
					//transform.Rotate(0f, 45f*(-3-lookingRotation), 0f);
				}
				transform.Rotate(0f, (circleHori-aproxRotateHori)/Mathf.PI*180,0f);
				aproxRotateHori = circleHori;
				lookingRotation = -3;
			}
			else if(Input.GetAxis("Vertical") < 0 && Input.GetAxis("Horizontal") == 0) {
				if(lookingRotation!=4){				
					//transform.Rotate(0f, 45f*(4-lookingRotation), 0f);
				}
				transform.Rotate(0f, (circleHori-aproxRotateHori)/Mathf.PI*180,0f);
				aproxRotateHori = circleHori;
				lookingRotation = 4;
			}
			
			
			
			
			
			//colorSlide += Time.deltaTime*20;
			//rend.material.color = new Color(Mathf.Sin(colorSlide), 0f, Mathf.Cos(colorSlide));
			if(circleHori > 2*Mathf.PI || circleHori < -2*Mathf.PI) {
				circleHori = 0;
			}
			//if(circleUpi > 2*Mathf.PI || circleUpi < -2*Mathf.PI) {
			//	circleUpi = 0;
			//}
			if(jump_timer >= 0){
				jump_timer -= 100*Time.deltaTime;
			}
			if(jump_timer < 0) {
				consecJumps = 1;
			}
			
			
			//LOSE LIFE
			if(this.transform.position.y < start_y-100){
				this.transform.position = new Vector3(1f,1f,1f);//create a vector for startPosPlayer and another one for the camera
				circleHori = 0f;				
				//SceneManager.LoadScene("World1");
				if(lives == 0)
					SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
				else{
					lives--;
					camera.position=new Vector3(1f, 10f,-9f);
				}
				
			}
			
			
			if(controlMode == 0) {
				
				
				if (Mathf.Abs(Input.GetAxis("CameraRotationVertical")) > 0  ) {
					circleUpi += Time.deltaTime * cameraSensitivity * Input.GetAxis("CameraRotationVertical");
					if (circleUpi >= Mathf.PI) {
						circleUpi = Mathf.PI;
					}
					else if (circleUpi <= 0f) {
						circleUpi = 0f;
					}
				}
				float div = 1f;
				if (isCamLockedTrack)
					div = 60f;
				
				//Debug.Log(Input.GetAxis("Horizontal"));
				if (!isOnFP) {
					if(Input.GetAxis("Horizontal") > 0 && Input.GetAxis("Vertical") > 0 ) {
						stableAngle = Mathf.PI/4;
						
						//if (camera.GetComponent<CameraController>().canMoveLeft)
							circleHori += Time.deltaTime*cameraSensitivity*3/4*Mathf.Abs(Input.GetAxis("Horizontal")/div);
						//transform.Rotate(Vector3.up * Time.deltaTime/Mathf.PI*180);
					}
					else if(Input.GetAxis("Horizontal") > 0 && Input.GetAxis("Vertical") < 0) {
						stableAngle = -Mathf.PI/4;
						//if (camera.GetComponent<CameraController>().canMoveLeft)
							circleHori += Time.deltaTime*cameraSensitivity*3/4*Mathf.Abs(Input.GetAxis("Horizontal")/div);
						//transform.Rotate(Vector3.up * Time.deltaTime/Mathf.PI*180);
						
					}
					else if(Input.GetAxis("Horizontal") < 0 && Input.GetAxis("Vertical") > 0) {
						stableAngle = 3*Mathf.PI/4;
						//if (camera.GetComponent<CameraController>().canMoveRight)
							circleHori -= Time.deltaTime*cameraSensitivity*3/4*Mathf.Abs(Input.GetAxis("Horizontal")/div);
						//transform.Rotate(Vector3.up * -Time.deltaTime/Mathf.PI*180);
						
					}
					else if(Input.GetAxis("Horizontal") < 0 && Input.GetAxis("Vertical") < 0) {
						stableAngle = 5*Mathf.PI/4;
						//if (camera.GetComponent<CameraController>().canMoveRight)
							circleHori -= Time.deltaTime*cameraSensitivity*3/4*Mathf.Abs(Input.GetAxis("Horizontal")/div);
						//transform.Rotate(Vector3.up * -Time.deltaTime/Mathf.PI*180);
						
					}
					else if(Input.GetAxis("Horizontal") > 0) {
						stableAngle = 0;
						//if (camera.GetComponent<CameraController>().canMoveLeft)
							circleHori += Time.deltaTime*cameraSensitivity*Mathf.Abs(Input.GetAxis("Horizontal")/div);
						//transform.Rotate(Vector3.up * Time.deltaTime/Mathf.PI*180);
						
					}
					else if(Input.GetAxis("Horizontal") < 0) {
						stableAngle = Mathf.PI;
						//if (camera.GetComponent<CameraController>().canMoveRight)
							circleHori -= Time.deltaTime*cameraSensitivity*Mathf.Abs(Input.GetAxis("Horizontal")/div);
						//transform.Rotate(Vector3.up * -Time.deltaTime/Mathf.PI*180);
						
					}
					else if(Input.GetAxis("Vertical") > 0 ) {
						stableAngle = Mathf.PI/2;
						
					}
					else if(Input.GetAxis("Vertical") < 0 ) {
						stableAngle = -Mathf.PI/2;
						
					}
				}
				
				
				if (isOnFP == false) {
					if(Input.GetAxis("CameraRotationHoriz") > 0.1f) {
						trackStopped = false;
						wasCameraMovedHoriz = true;
						if (camera.GetComponent<CameraController>().canMoveLeft){
							circleHori += Time.deltaTime*cameraSensitivity*Mathf.Abs(Input.GetAxis("CameraRotationHoriz"));
							if (isCamLocked){
								oldCircleHori += Time.deltaTime*cameraSensitivity*Mathf.Abs(Input.GetAxis("CameraRotationHoriz"));
								
								
								
							}
						}
						

					} else if(Input.GetAxis("CameraRotationHoriz") < -0.1f) {
						trackStopped = false;
						wasCameraMovedHoriz = true;
						if (camera.GetComponent<CameraController>().canMoveRight){
							circleHori -= Time.deltaTime*cameraSensitivity*Mathf.Abs(Input.GetAxis("CameraRotationHoriz"));
							if (isCamLocked){
								oldCircleHori -= Time.deltaTime*cameraSensitivity*Mathf.Abs(Input.GetAxis("CameraRotationHoriz"));
							}
						}
						
					}
				}
				
				
				
				if (isRotating == false){
					rotationSpeed = 0f;
				}
				
				
				if(rotationSpeed > 0f && !isOnFP) {
					
					if (isCamLocked){
						oldCircleHori += Time.deltaTime*cameraSensitivity*Mathf.Abs(Input.GetAxis("CameraRotationHoriz"));
						if (oldCircleHori > 2f*Mathf.PI)
							oldCircleHori = 0f;
						
						
						
					}
					else {
						if (Mathf.Abs(Input.GetAxis("CameraRotationHoriz")) < 0.3 )
							circleHori += Time.deltaTime*cameraSensitivity/2f;
					}

				} 
				else if(rotationSpeed < 0f) {
					circleHori -= Time.deltaTime*cameraSensitivity/2f;
					if (isCamLocked){
						oldCircleHori -= Time.deltaTime*cameraSensitivity*Mathf.Abs(Input.GetAxis("CameraRotationHoriz"));
						if (oldCircleHori < 0f)
							oldCircleHori = 2f*Mathf.PI;
					}
				}
				
				if (circleHori < 0f){
					circleHori += 2f*Mathf.PI;
				}
				if (lastLook > 2*Mathf.PI)
					lastLook -= 2*Mathf.PI;
				
				if (isCamLockedTrack && !trackStopped) {
					
					if (vector_val < 0f) {
						wasCameraMovedHoriz = true;
						
						float angle01n = Mathf.Abs(lastLook - oldCircleHori);
						float angle02n = Mathf.Abs(oldCircleHori - lastLook);
						float final_anglen;
						if (angle01n < angle02n){
							final_anglen = angle01n;
						}
						else {
							final_anglen = angle02n;
						}
						if (final_anglen > Mathf.PI/2f)
							final_anglen = Mathf.PI/2f;
						
						oldCircleHori -= Time.deltaTime*cameraSensitivity*1.3f*final_anglen;
						if (oldCircleHori < 0f){
							oldCircleHori = Mathf.PI*2f;
						}
						float target_p = lastLook+0.1f;
						float target_n = lastLook-0.1f;
						float target_temp;
						if (target_p < target_n) {
							target_temp = target_p;
							target_p = target_n;
							target_n = target_temp;
							
						}
						float prod_p = vector_val = Mathf.Cos(oldCircleHori)*Mathf.Sin(target_p) - Mathf.Cos(target_p)*Mathf.Sin(oldCircleHori);
						float prod_n = vector_val = Mathf.Cos(oldCircleHori)*Mathf.Sin(target_n) - Mathf.Cos(target_n)*Mathf.Sin(oldCircleHori);
						if (prod_p > 0f || prod_n > 0f) {
							//float headAngle = heading-savedCircleHori;
							//if (headAngle < 0f) {
							//	headAngle += 2f*Mathf.PI;
							//}
							
							//if (angle < 0f) {
							//	angle += 2*Mathf.PI;
							//}
							
							float angle01 = Mathf.Abs(lastLook - oldCircleHori);
							float angle02 = Mathf.Abs(oldCircleHori - lastLook) ;
							if (angle01 <= Mathf.PI/2f || angle02 <= Mathf.PI/2f
							){
								trackStopped = true;
								//Debug.Log("STOPPED");
								
								//Debug.Log(lastLook-Mathf.PI);
								//Debug.Log(oldCircleHori-Mathf.PI);
								
								//Debug.Log(headAngle);
							}
							
						}
						
						if (oldCircleHori <= target_p && oldCircleHori >= target_n ){
							//oldCircleHori = lastLook;
							trackStopped = true;
							//Debug.Log("STOPPED");
							wasCameraMovedHoriz = false;
						}
						
						
					}
					else if (vector_val > 0f) {
						wasCameraMovedHoriz = true;
						
						float angle01p = Mathf.Abs(lastLook - oldCircleHori);
						float angle02p = Mathf.Abs(oldCircleHori - lastLook);
						float final_anglep;
						if (angle01p < angle02p){
							final_anglep = angle01p;
						}
						else {
							final_anglep = angle02p;
						}
						if (final_anglep > Mathf.PI/2f)
							final_anglep = Mathf.PI/2f;
						
						oldCircleHori += Time.deltaTime*cameraSensitivity*1.3f*final_anglep;
						
						if (oldCircleHori > 2f*Mathf.PI){
							oldCircleHori = 0f;
						}
						float target_p = lastLook+0.1f;
						float target_n = lastLook-0.1f;
						float target_temp;
						if (target_p < target_n) {
							target_temp = target_p;
							target_p = target_n;
							target_n = target_temp;
							
						}
						float prod_p = vector_val = Mathf.Cos(oldCircleHori)*Mathf.Sin(target_p) - Mathf.Cos(target_p)*Mathf.Sin(oldCircleHori);
						float prod_n = vector_val = Mathf.Cos(oldCircleHori)*Mathf.Sin(target_n) - Mathf.Cos(target_n)*Mathf.Sin(oldCircleHori);
						if (prod_p < 0f || prod_n < 0f) {
							//float headAngle = heading-savedCircleHori;
							//if (headAngle < 0f) {
							//	headAngle += 2f*Mathf.PI;
							//}
							float angle01 = Mathf.Abs(lastLook - oldCircleHori);
							float angle02 = Mathf.Abs(oldCircleHori - lastLook);
							if (angle01 <= Mathf.PI/2f || angle02 <= Mathf.PI/2f
							){
								trackStopped = true;
								//Debug.Log("STOPPED");
								
								//Debug.Log(lastLook-Mathf.PI);
								//Debug.Log(oldCircleHori-Mathf.PI);
								
								//Debug.Log(headAngle);
							}
						}
						if (oldCircleHori <= target_p && oldCircleHori >= target_n ){
							//oldCircleHori = lastLook;
							trackStopped = true;
							//Debug.Log("STOPPED");
							wasCameraMovedHoriz = false;
						}
					}
					
							
				}
				
				
				//Debug.Log(jump_timer);
				//theRB.velocity = new Vector3(Input.GetAxis("Horizontal") * movespeed, 
											// theRB.velocity.y,
										//	 Input.GetAxis("Vertical")*movespeed
				//);
				//if(Input.GetButtonDown("Jump")){
				//	theRB.velocity = new Vector3(theRB.velocity.x, jumpforce, theRB.velocity.z);
					
				//}
				moveDirection = new Vector3(Mathf.Cos(circleHori)*speed_horizontal*inputHorizontal + Mathf.Sin(circleHori)*speed_vertical,
											moveDirection.y,
											-Mathf.Sin(circleHori)*speed_horizontal*inputHorizontal + Mathf.Cos(circleHori)*speed_vertical);
			
				//LookAtCenter.transform.position = new Vector3(0f,0f,0f);
				//float lookAtLimit = 0.3f;
				//updateLookAtCenter();
				
			
			} else if(controlMode == 1) {
				moveDirection = new Vector3(Mathf.Cos(circleHori)*Input.GetAxis("Horizontal")*movespeed + Mathf.Sin(circleHori)*Input.GetAxis("Vertical")*movespeed,
											moveDirection.y,
											-Mathf.Sin(circleHori)* Input.GetAxis("Horizontal")*movespeed + Mathf.Cos(circleHori)*Input.GetAxis("Vertical")*movespeed);
				if(Input.GetAxis("CameraRotationHoriz") > 0) {
					circleHori += Time.deltaTime*cameraSensitivity;

				} else if(Input.GetAxis("CameraRotationHoriz") < 0) {
					circleHori -= Time.deltaTime*cameraSensitivity;
					
					
				}
				
				
			}
			/*moveCam = new Vector3(Mathf.Cos(2*circleHori+Mathf.PI)*Input.GetAxis("Horizontal")*5 + Mathf.Sin(circleHori)*Input.GetAxis("Vertical")*movespeed,
										0f,
										-Mathf.Sin(2*circleHori+Mathf.PI)* Input.GetAxis("Horizontal")*5 + Mathf.Cos(circleHori)*Input.GetAxis("Vertical")*movespeed);
										*/
			camdist = Mathf.Sqrt(Mathf.Abs(camera.position.x-transform.position.x)*Mathf.Abs(camera.position.x-transform.position.x) + Mathf.Abs(camera.position.z-transform.position.z)*Mathf.Abs(camera.position.z-transform.position.z));
			
			if (isCamLocked == false){ //UNNECESSARY
				moveCam = new Vector3(-Mathf.Cos(circleHori)*Input.GetAxis("CameraRotationHoriz")*2*movespeed + Mathf.Sin(circleHori+newCircleHori)*Input.GetAxis("Vertical")*movespeed,
											0f,
											-Mathf.Sin(circleHori)*Input.GetAxis("CameraRotationHoriz")*2*movespeed + Mathf.Cos(circleHori+newCircleHori)*Input.GetAxis("Vertical")*movespeed);
			}
			else {
				moveCam = new Vector3(-Mathf.Cos(circleHori)*Input.GetAxis("CameraRotationHoriz")*2*movespeed + Mathf.Sin(circleHori+newCircleHori)*Input.GetAxis("Vertical")*movespeed,
											0f,
											-Mathf.Sin(circleHori)*Input.GetAxis("CameraRotationHoriz")*2*movespeed + Mathf.Cos(circleHori+newCircleHori)*Input.GetAxis("Vertical")*movespeed);
			}
			
			
			checkAngle = Mathf.Sin(circleHori);
			
			
			//if (camdist < camera_distance_limit - 2f) {
				
			//	moveCam += new Vector3 (-Mathf.Sin(circleHori)*movespeed, 0f,  -Mathf.Cos(circleHori)*movespeed);
				
			//}
			//else if(camdist > camera_distance_limit + 2f) {
			//	moveCam += new Vector3 (Mathf.Sin(circleHori)*movespeed, 0f,  Mathf.Cos(circleHori)*movespeed);
			//}
			
			
			/*if(moveDirection.y>fallSpeedLimit || controller.isGrounded){
				if(camera.position.y > transform.position.y + 4f) {
					if(camera.position.y+(-movespeed)*Time.deltaTime < transform.position.y + 2f)
						moveCam += new Vector3(0f, -movespeed/2, 0f);
					else
						moveCam += new Vector3(0f, -movespeed*Mathf.Abs(camera.position.y-transform.position.y+4f), 0f);
				}
				else if(camera.position.y < transform.position.y + 2f) {
					if(camera.position.y+movespeed*Time.deltaTime > transform.position.y + 2f)
						moveCam += new Vector3(0f, movespeed/2, 0f);
					else
						moveCam += new Vector3(0f, 100*movespeed*Mathf.Abs(camera.position.y-transform.position.y+2f), 0f);
				}
			}*/
			
			/*
			if (isCamLocked == false || isCamLocked == true){
				if(moveDirection.y>fallSpeedLimit || controller.isGrounded){
					if(camera.position.y > transform.position.y + 5f) {
						
						//moveCam += new Vector3(0f, -movespeed*Time.deltaTime*30, 0f);
						moveCam += new Vector3(0f, -cameraSpeed, 0f);
					}
					else if(camera.position.y < transform.position.y + 2f) {
					
						//moveCam += new Vector3(0f, movespeed*Time.deltaTime*30, 0f);
						moveCam += new Vector3(0f, cameraSpeed, 0f);
					}
				}
			}
			*/
			//Debug.Log(circleUpi);
			
			if (camera.GetComponent<CameraController>().isGoingToFirstPerson == false && camera.GetComponent<CameraController>().isGoingBackFromFirstPerson == false){
				if (isCamLocked == false || isCamLocked == true){
					if(moveDirection.y>fallSpeedLimit && !controller.isGrounded){
						if ( (camera.position.y > transform.position.y + 5f )) {
							
							//moveCam += new Vector3(0f, -movespeed*Time.deltaTime*30, 0f);
							moveCam += new Vector3(0f, -cameraSpeed/2f, 0f);
							//camera.position = new Vector3 (camera.position.x, transform.position.y + 5f, camera.position.z);
						}
						else if(camera.position.y < transform.position.y + 2f) {
						
							//moveCam += new Vector3(0f, movespeed*Time.deltaTime*30, 0f);
							
							//if (moveDirection.y >= 2f*jumpforce/3f)
							moveCam += new Vector3(0f, cameraSpeed/2f, 0f);
							camera.position = new Vector3 (camera.position.x, transform.position.y + 2f, camera.position.z);
						}
					}
					else if (moveDirection.y>fallSpeedLimit || controller.isGrounded) {
						//if(camera.position.y > transform.position.y + 5f) {
						if ( (camera.position.y > transform.position.y + 5f && Mathf.Abs(Input.GetAxis("CameraRotationVertical")) < 0.2f  ) || 
						    (camera.position.y > transform.position.y + 7f && Mathf.Abs(Input.GetAxis("CameraRotationVertical")) > 0.3f ) ) {
							
							//moveCam += new Vector3(0f, -movespeed*Time.deltaTime*30, 0f);
							moveCam += new Vector3(0f, -cameraSpeed, 0f);
						}
						else if(camera.position.y < transform.position.y + 2f) {
						
							//moveCam += new Vector3(0f, movespeed*Time.deltaTime*30, 0f);
							moveCam += new Vector3(0f, cameraSpeed, 0f);
						}
						
					}
				}
			}
			
			if (isCamLocked == false){
				
				
				//camera.transform.position = new Vector3(Mathf.Sin(circleHori+Mathf.PI)*(cameraDistance-Mathf.Cos(circleUpi)*cameraDistance/2*Input.GetAxis("CameraRotationVertical")*Time.deltaTime*10) + transform.position.x + Mathf.Sin(circleHori)*Input.GetAxis("Vertical")*movespeed*Time.deltaTime,
				//						camera.transform.position.y+Mathf.Sin(circleUpi)*Input.GetAxis("CameraRotationVertical")*Time.deltaTime*cameraSpeed,
				//						Mathf.Cos(circleHori+Mathf.PI)*(cameraDistance-Mathf.Sin(circleUpi)*cameraDistance/2*Input.GetAxis("CameraRotationVertical")*Time.deltaTime*10) + transform.position.z  + Mathf.Cos(circleHori)*Input.GetAxis("Vertical")*Time.deltaTime);
				
				
				 //- Mathf.Sin(circleUpi)*Input.GetAxis("CameraRotationVertical")*Time.deltaTime*cameraSpeed;			
				//camera.transform.position = new Vector3(Mathf.Sin(circleHori+Mathf.PI)*(newCamDist) + transform.position.x + Mathf.Sin(circleHori)*Input.GetAxis("Vertical")*movespeed*Time.deltaTime,
				//						camera.transform.position.y+Mathf.Sin(circleUpi)*Input.GetAxis("CameraRotationVertical")*Time.deltaTime*cameraSpeed,
				//						Mathf.Cos(circleHori+Mathf.PI)*(newCamDist) + transform.position.z  + Mathf.Cos(circleHori)*Input.GetAxis("Vertical")*Time.deltaTime);
				//Debug.Log(circleUpi);
				newCamDistX = cameraDistance-Mathf.Cos(circleUpi)*cameraDistance/6f;//- Mathf.Sin(circleUpi)*Input.GetAxis("CameraRotationVertical")*Time.deltaTime*cameraSpeed;	;
				newCamDistZ = cameraDistance-Mathf.Cos(circleUpi)*cameraDistance/6f;//- Mathf.Cos(circleUpi)*Input.GetAxis("CameraRotationVertical")*Time.deltaTime*cameraSpeed;	;
				
				
				//camera.transform.position = new Vector3(Mathf.Sin(circleHori+Mathf.PI)*(newCamDistX) + transform.position.x + Mathf.Sin(circleHori)*Input.GetAxis("Vertical")*movespeed*Time.deltaTime - Mathf.Cos(circleUpi)*Input.GetAxis("CameraRotationVertical")*Time.deltaTime*10f,
				//						camera.transform.position.y+Mathf.Sin(circleUpi)*Input.GetAxis("CameraRotationVertical")*Time.deltaTime*cameraSpeed,
					//					Mathf.Cos(circleHori+Mathf.PI)*(newCamDistX) + transform.position.z  + Mathf.Cos(circleHori)*Input.GetAxis("Vertical")*Time.deltaTime - Mathf.Sin(circleUpi)*Input.GetAxis("CameraRotationVertical")*Time.deltaTime*10f        );
					
				
				//ORIGINAL
				if (camera.GetComponent<CameraController>().isGoingToFirstPerson == false && camera.GetComponent<CameraController>().isFirstPerson == false && camera.GetComponent<CameraController>().isGoingBackFromFirstPerson == false){
					
					
					//if (isBossBattle == true){
						
						//camera.transform.position = new Vector3(Mathf.Sin(circleHori+Mathf.PI)*(newCamDistX) + bossTarget.transform.position.x + Mathf.Sin(circleHori)*speed_vertical*Time.deltaTime,
											//camera.transform.position.y+Input.GetAxis("CameraRotationVertical")*Time.deltaTime*cameraSpeed,
											//Mathf.Cos(circleHori+Mathf.PI)*(newCamDistX) + bossTarget.transform.position.z  + Mathf.Cos(circleHori)*speed_vertical/movespeed*Time.deltaTime);
						
					//}
					//else { 
					//camera.transform.position = new Vector3(Mathf.Sin(circleHori+Mathf.PI)*(newCamDistX) + transform.position.x + Mathf.Sin(circleHori)*speed_vertical*Time.deltaTime,
											//camera.transform.position.y+Input.GetAxis("CameraRotationVertical")*Time.deltaTime*cameraSpeed,
											//Mathf.Cos(circleHori+Mathf.PI)*(newCamDistX) + transform.position.z  + Mathf.Cos(circleHori)*speed_vertical/movespeed*Time.deltaTime);
											
						camera.transform.position = new Vector3(Mathf.Sin(circleHori+Mathf.PI)*(newCamDistX) + transform.position.x ,
											camera.transform.position.y+Input.GetAxis("CameraRotationVertical")*Time.deltaTime*cameraSpeed,
											Mathf.Cos(circleHori+Mathf.PI)*(newCamDistX) + transform.position.z  );
					//}						
											
					//with circleUpi					
					//camera.transform.position = new Vector3(Mathf.Sin(circleHori+Mathf.PI)*(newCamDistX) + transform.position.x + Mathf.Sin(circleHori)*Input.GetAxis("Vertical")*movespeed*Time.deltaTime,
						//					camera.transform.position.y+Mathf.Sin(circleUpi)*Input.GetAxis("CameraRotationVertical")*Time.deltaTime*cameraSpeed,
						//					Mathf.Cos(circleHori+Mathf.PI)*(newCamDistX) + transform.position.z  + Mathf.Cos(circleHori)*Input.GetAxis("Vertical")*Time.deltaTime);
				}		
										
				/*camera.transform.position = new Vector3(Mathf.Sin(circleHori+Mathf.PI)*(cameraDistance-Mathf.Sin(circleUpi)*cameraDistance/2) + transform.position.x + Mathf.Sin(circleHori)*Input.GetAxis("Vertical")*movespeed*Time.deltaTime,
										camera.transform.position.y+Mathf.Sin(circleUpi)*Input.GetAxis("CameraRotationVertical")*Time.deltaTime*cameraSpeed,
										Mathf.Cos(circleHori+Mathf.PI)*(cameraDistance-Mathf.Cos(circleUpi)*cameraDistance/2) + transform.position.z  + Mathf.Cos(circleHori)*Input.GetAxis("Vertical")*Time.deltaTime);
					*/					
			}
			else {	

				//Original
				//camera.transform.position = new Vector3(Mathf.Sin(oldCircleHori+Mathf.PI)*cameraDistance + transform.position.x + Mathf.Sin(circleHori)*Input.GetAxis("Vertical")*movespeed*Time.deltaTime,
									//	camera.transform.position.y+Mathf.Sin(circleUpi)*Input.GetAxis("CameraRotationVertical")*Time.deltaTime*cameraSpeed,
									//	Mathf.Cos(oldCircleHori+Mathf.PI)*cameraDistance + transform.position.z  + Mathf.Cos(circleHori)*Input.GetAxis("Vertical")*Time.deltaTime);
			
			
			
				//camera.transform.position = new Vector3(Mathf.Sin(oldCircleHori+Mathf.PI)*newCamDistX + transform.position.x + Mathf.Sin(circleHori)*speed_vertical*Time.deltaTime- Mathf.Cos(circleUpi)*Input.GetAxis("CameraRotationVertical")*Time.deltaTime*10f,
				//						camera.transform.position.y+Mathf.Sin(circleUpi)*Input.GetAxis("CameraRotationVertical")*Time.deltaTime*cameraSpeed,
				//						Mathf.Cos(oldCircleHori+Mathf.PI)*newCamDistZ + transform.position.z  + Mathf.Cos(circleHori)*speed_vertical/movespeed*Time.deltaTime- Mathf.Sin(circleUpi)*Input.GetAxis("CameraRotationVertical")*Time.deltaTime*10f);
			
			
				camera.transform.position = new Vector3(Mathf.Sin(oldCircleHori+Mathf.PI)*newCamDistX + transform.position.x - Mathf.Cos(circleUpi)*Input.GetAxis("CameraRotationVertical")*Time.deltaTime*10f,
										camera.transform.position.y+Mathf.Sin(circleUpi)*Input.GetAxis("CameraRotationVertical")*Time.deltaTime*cameraSpeed,
										Mathf.Cos(oldCircleHori+Mathf.PI)*newCamDistZ + transform.position.z - Mathf.Sin(circleUpi)*Input.GetAxis("CameraRotationVertical")*Time.deltaTime*10f);
			
			
			
			
			
			
			
			
			}
			//Debug.Log(oldCircleHori);
			//Debug.Log(camdist);
			
			
			
			if (controller.isGrounded || toJump == true) {
				//if(isRotating == false) {
				//	newCircleHori = 0f;
				//	extraCircleHori = 0f;
				//}
				//moveDirection = new Vector3(Input.GetAxis("Horizontal")*movespeed, 0f, Input.GetAxis("Vertical")*movespeed);
				
				if (toJump == true) {
					moveDirection.y = jumpforce; //* consecJumps;
					anim.SetBool("isJumping", true);
					jumpSound.Play();
					isGrounded = false;
					if(consecJumps<3){
						consecJumps++;
					}
					jump_timer = 100;
					toJump = false;
					//isJumping = true;
					
				} 
			}			
			else {
				if (moveDirection.y>fallSpeedLimit){
					
					bool isExtraHigh = false;
					if (Input.GetButton("Jump") && moveDirection.y > 0f) {
						isExtraHigh = true;
					}
				
					if (isExtraHigh) {
						moveDirection.y = moveDirection.y + Physics.gravity.y * gravityScale * 0.5f;
					}
					else {
						moveDirection.y = moveDirection.y + Physics.gravity.y * gravityScale;
					}
				}
				else {
					moveDirection.y = moveDirection.y;
				}
			}
			
			
			if (Input.GetButton("Zoom") && (cameraDistanceIsMoving == false) ) {
				if (cameraDistanceMode == 1) {
					cameraDistanceTarget = 15f;
					cameraDistanceMode = 2;
					cameraDistanceIsMoving = true;
				
				}
				else if(cameraDistanceMode == 2) {
					cameraDistanceTarget = 10f;
					cameraDistanceMode = 1;
					cameraDistanceIsMoving = true;
				}
				
			}
			//CHECK IF FOUND camera canMoveBack == false!!!!!!!!!!!!!
			bool needToGoForward = camera.GetComponent<CameraController>().needToGoForward;
			float camPlayerDist = camera.GetComponent<CameraController>().player_dist;
			float last_wall_dist = camera.GetComponent<CameraController>().last_wall_dist;
			
			if (cameraDistanceIsMoving == true && needToGoForward == false) {
			
				if (cameraDistance < cameraDistanceTarget){
					
					cameraDistance = cameraDistance + Time.deltaTime * cameraSpeed;
					if (cameraDistance >= cameraDistanceTarget){
						cameraDistance = cameraDistanceTarget;
						cameraDistanceIsMoving = false;
					}
				}
				else if (cameraDistance > cameraDistanceTarget){
					
					cameraDistance = cameraDistance - Time.deltaTime * cameraSpeed;
					
					if (cameraDistance <= cameraDistanceTarget){
						cameraDistance = cameraDistanceTarget;
						cameraDistanceIsMoving = false;
					}
				
				}
				
				
				
			}
			
			if (Mathf.Abs(camera.GetComponent<CameraController>().fwdTarget-cameraDistance) < Mathf.Abs(camPlayerDist-last_wall_dist)/3f+0.1f    )
				camera.GetComponent<CameraController>().foundFwdTarget = false;
			
			//if (camera.GetComponent<CameraController>().foundFwdTarget = false && )
			//Debug.Log(camera.GetComponent<CameraController>().canMoveBack);
			//if (needToGoForward && camera.GetComponent<CameraController>().player_dist > 1f && camera.GetComponent<CameraController>().canMoveBack) {
				
			if (!isCamLocked){
				if (camera.GetComponent<CameraController>().foundFwdTarget ) {//&& camera.GetComponent<CameraController>().canMoveBack){
					
					//if (camera.GetComponent<CameraController>().fwdTarget < cameraDistance){
					if (camera.GetComponent<CameraController>().canMoveBack){
						if (camera.GetComponent<CameraController>().fwdTarget < cameraDistance)
							cameraDistance = cameraDistance - Time.deltaTime * cameraSpeed * camPlayerDist/5f;
						
						if (camera.transform.position.y > transform.position.y+2.3f)
							moveCam += new Vector3(0f, -cameraSpeed/2f, 0f);
					}
					else {
						camera.GetComponent<CameraController>().foundFwdTarget = false;
					}
					
					
				}
				else {//checkBACK
				
				
					if ((!camera.GetComponent<CameraController>().canMoveBack ||
						(Input.GetAxis("Horizontal") > 0.1f  && !camera.GetComponent<CameraController>().canMoveLeft) ||
						(Input.GetAxis("Horizontal") < -0.1f  && !camera.GetComponent<CameraController>().canMoveRight)
						) && cameraDistance >0.5f
						
						) {
							
						cameraDistance = cameraDistance - Time.deltaTime * cameraSpeed * cameraDistance/4f;
						if (cameraDistance < 0.5f)
							cameraDistance = 0.5f;
						
					} 
					else if ((!camera.GetComponent<CameraController>().canMoveBack ||
						(Input.GetAxis("Horizontal") > 0.1f  && !camera.GetComponent<CameraController>().canMoveLeft) ||
						(Input.GetAxis("Horizontal") < -0.1f  && !camera.GetComponent<CameraController>().canMoveRight)
						) && cameraDistance <0.4f
						
						) {
						//cameraDistance = cameraDistance + Time.deltaTime * cameraSpeed * cameraDistance/4f;
						
					}
					else {
						if ((Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f||(Mathf.Abs(Input.GetAxis("CameraRotationHoriz")) > 0.1f)) && camera.GetComponent<CameraController>().lastBackDist > 3f) {
							if (cameraDistance < cameraDistanceTarget){
								cameraDistance = cameraDistance + Time.deltaTime * cameraSpeed;
							}
							else if(cameraDistance > cameraDistanceTarget+1f) {
								cameraDistance = cameraDistance - Time.deltaTime * cameraSpeed;
							}
						}
					}
				
				
					/*
					if (isCamLocked == false) {
						if (camera.GetComponent<CameraController>().canMoveBack) {
							if (cameraDistance < cameraDistanceTarget){
								cameraDistance = cameraDistance + Time.deltaTime * cameraSpeed;
							}
							else if(cameraDistance > cameraDistanceTarget+1f) {
								cameraDistance = cameraDistance - Time.deltaTime * cameraSpeed;
							}
						}
						camera.GetComponent<CameraController>().fwdTarget = cameraDistance; 
					}
					*/
					
					
					
					
				}
			}
			
			
			/*dist = Mathf.Sqrt((camera.position.z-this.transform.position.z)*(camera.position.z-this.transform.position.z)+(camera.position.x-this.transform.position.x)*(camera.position.x-this.transform.position.x));
			if(Mathf.Abs(dist) > 5+0.0001f ){
				if(camera.position.z > this.transform.position.z+0.0001f && camera.position.x > this.transform.position.x+0.0001f){
					moveCam.z = -movespeed;
					moveCam.x = -movespeed;
				}
				else if(camera.position.z < this.transform.position.z-0.0001f && camera.position.x < this.transform.position.x-0.0001f){
					moveCam.z = movespeed;
					moveCam.x = movespeed;
				}
				else if(camera.position.z > this.transform.position.z+0.0001f && camera.position.x < this.transform.position.x-0.0001f){
					if(Math.Abs(camera.position.z-this.transform.position.z) > 5.0f)
						moveCam.z = -movespeed;
					
				}
				
				
				
			}*/
			//moveCam = new Vector3 (Mathf.Sqrt(25-transform.position.x*transform.position.x), camera.position.y, Mathf.Sqrt(25-transform.position.z*transform.position.z));
			//transform.position += (moveDirection * Time.deltaTime);
		
			
			
			//moveDirection.x = 0f;
			//moveDirection.y = 0f;
			//moveDirection.z = 0f;
			transformMove = moveDirection;
			transformMove.y = 0f;
			
			//moveDirection.x = 0f;
			//moveDirection.z = 0f;
			//transform.position += transformMove*Time.deltaTime;
			saveDirection = addedDirection.y;
			addedDirection.y=0;
			
			moveDirection = moveDirection+addedDirection;
			
			
			if (!isOnFP) {
				controller.Move(moveDirection * Time.deltaTime);
			}
			
			
			
			
			//transform.position += (moveDirection * Time.deltaTime);
			//moveCam =  moveDirection;
			//moveCam.y = 0f;
			moveCam.x = 0f;
			moveCam.z = 0f;
			
			camera.transform.position += (moveCam*Time.deltaTime);
			if(moveDirection.y<=fallSpeedLimit && !controller.isGrounded && hover)
				camera.position += new Vector3(0, moveDirection.y*Time.deltaTime, 0);
			
			//addedDirection.x = 0f;
			//addedDirection.z =0f;
			addedDirection.y = saveDirection;
			
			Vector3 tempVect = new Vector3(0f, addedDirection.y,0f);
			
			transform.position += tempVect*Time.deltaTime;
			
			//SLOPE
			
			if (!isGrounded && addedDirection.y <= 0f) {
				 float speed_x = (1f - normalZX.y) * normalZX.x * (1f - slideFriction);
				 float speed_z = (1f - normalZX.y) * normalZX.z * (1f - slideFriction);
				 //inpRes.speed.x += (1f - hitNormal.y) * hitNormal.x * (1f - slideFriction);
				 //inpRes.speed.z += (1f - hitNormal.y) * hitNormal.z * (1f - slideFriction);
				 
				 Vector3 slideVector = new Vector3(speed_x,0f,speed_z);
				 float mod = Mathf.Sqrt(speed_x*speed_x + speed_z*speed_z);
				 
				 if (mod<0.5f){
					controller.radius = 0.2f;
				 }
				 
				 if (mod > slideSpeedLimit) {
					slideVector = new Vector3(speed_x/mod,0f,speed_z/mod)*slideSpeedLimit;					 
				 }
				 //if (Mathf.Abs(speed_x) <= slideSpeedLimit || Mathf.Abs(speed_z) <= slideSpeedLimit) {
				 //if (Mathf.Abs(speed_x) <= slideSpeedLimit && Mathf.Abs(speed_z) <= slideSpeedLimit) {
				 //if (Mathf.Abs(speed_x/mod) <= slideSpeedLimit && Mathf.Abs(speed_z/mod) <= slideSpeedLimit) {
					 //SLOPE
					transform.position += slideVector*Time.deltaTime*20f;
					saveSlideVector = slideVector;
					//camera.position += (slideVector*Time.deltaTime);
				 //}
				 
			 }
			
			
			
			//transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(tempVect), -0.15F);
			//to look forward
			if (( (Mathf.Abs(Input.GetAxis("Horizontal")) > 0) || (Mathf.Abs(Input.GetAxis("Vertical") ) > 0)   ) && !isOnFP) {
				
				//WHAT MATTERS
				//float heading = Mathf.Atan2(Mathf.Sin(circleHori)+Mathf.Sin(Input.GetAxis("Horizontal")),Mathf.Cos(circleHori)+Mathf.Cos(Input.GetAxis("Vertical")))  ;
				float hori_input = Input.GetAxis("Horizontal");
				float verti_input = Input.GetAxis("Vertical");
				float mod = Mathf.Sqrt(hori_input*hori_input+verti_input*verti_input);
				heading = circleHori + Mathf.Atan2(Input.GetAxis("Horizontal")/mod,Input.GetAxis("Vertical")/mod);
				savedCircleHori = circleHori;
				//transform.rotation=Quaternion.Euler(heading_x*Mathf.Cos(lastLook)*Mathf.Rad2Deg + heading_z*Mathf.Sin(lastLook)*Mathf.Rad2Deg ,heading*Mathf.Rad2Deg,0f);
				//Debug.Log(Mathf.Cos(lastLook));
				float oldLastLook = lastLook;
				lastLook = heading;//*Mathf.Rad2Deg;
				if (lastLook < 0f) {
					lastLook += 2f*Mathf.PI;
				}
				
				//LookAtCenter.transform.position = transform.position;
				updateLookAtCenter();
				//float diffAngle = Mathf.Atan2(Mathf.Sin(lastLook-oldLastlook), Mathf.Cos(lastLook-oldLastlook))
				
				
				//LookAtCenter.transform.position = new Vector3(LookAtCenter.transform.position.x-Mathf.Sin(lastLook)*lookAtRadius+Mathf.Sin(oldLastLook)*lookAtRadius  ,
				//										   	  LookAtCenter.transform.position.y,
				//											  LookAtCenter.transform.position.z+Mathf.Cos(lastLook)*lookAtRadius-Mathf.Cos(oldLastLook)*lookAtRadius  );
				
				oldLookAtPos = LookAtCenter.transform.position;
				//Debug.Log(lastLook);
				//Debug.Log(oldCircleHori);
				//Debug.Log(vector_val);
				/////////////////////////////
				
				
				
				
				
				//float heading = Mathf.Atan2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
			}
			if (isRotating) {
				//heading += rotationSpeed*Time.deltaTime/200f;
				//savedCircleHori = circleHori;
				//updateLookAtCenter();
				//lastLook = heading;
				//oldLookAtPos = LookAtCenter.transform.position;
				
			}
			float llcos = Mathf.Cos(lastLook);
			float llsin = Mathf.Sin(lastLook);
			if (jumpCount > 0 ) {
				transform.rotation=Quaternion.Euler(heading_x*llcos*Mathf.Rad2Deg + heading_z*llsin*Mathf.Rad2Deg ,
												    heading*Mathf.Rad2Deg,
													heading_x*llsin*Mathf.Rad2Deg + heading_z*(-llcos)*Mathf.Rad2Deg);
			}
			else {

				transform.rotation=Quaternion.Euler(0f,heading*Mathf.Rad2Deg,0f);
				//transform.rotation=Quaternion.Euler(0f,transform.eulerAngles.y,0f);
			}
			
			
			if ( (Mathf.Abs(Input.GetAxis("Horizontal")) > 0) || (Mathf.Abs(Input.GetAxis("Vertical") ) > 0)  ) {
				
				LookAtCenter.transform.position = new Vector3(oldLookAtPos.x, transform.position.y+0.86f, oldLookAtPos.z);
				
				
			}
			
			if ((isCamLockedTrack == true || Mathf.Abs(Input.GetAxis("CameraRotationHoriz")) > 0f || Mathf.Abs(Input.GetAxis("CameraRotationVertical")) > 0f || isRotating)) {
				
				float diff_x = LookAtCenter.transform.position.x - transform.position.x;
				float diff_z = LookAtCenter.transform.position.z - transform.position.z;
				
				if(Mathf.Abs(diff_x) > 0.15f || Mathf.Abs(diff_z) > 0.15f)
					LookAtCenter.transform.position -= new Vector3(diff_x, 0f, diff_z) * Time.deltaTime*cameraSensitivity;
				
				
			}
		
			
			
		
			
			 
			
			
			
			
			
		}
		else if(is2D){
		
		
		
		}
	}
	
}
