using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject Player;
	public Transform target;
	public Transform oldTarget;
	public Vector3 offset;
	public bool useOffsetValues;
	private Vector3 moveDirection;
	private float circleHori;
	private float movespeed;
	
	public Vector3 oldPosition;
	public Vector3 oldRotation;
	public bool isGoingToFirstPerson;
	public bool isGoingBackFromFirstPerson;
	public bool isFirstPerson;
	
	public bool isRightRayActive;
	public bool isLeftRayActive;
	public bool isBackRayActive;
	public bool isForwardRayActive;
	
	public bool canMoveRight;
	public bool canMoveLeft;
	public bool canMoveBack;
	
	public bool needToGoForward;
	
	public float raySize;
	
	public float player_dist;
	
	public bool foundFwdTarget;
	public float fwdTarget;
	
	public float last_wall_dist;
	
	private GameObject fwdHit;
	
	private RaycastHit rightHit;
	private RaycastHit leftHit;
	private RaycastHit backHit;
	private RaycastHit forwardHit;
	private RaycastHit playerHit;
	
	public float lastBackDist;
	
	public Transform FPTarget;
	
	
	
	void Start () {
		//circleHori = target.transform.parent.GetComponent<PlayerController>().circleHori;
		//movespeed = target.transform.parent.GetComponent<PlayerController>().movespeed;
		transform.position = target.position - offset;
		if(!useOffsetValues) {
			offset = target.position - transform.position;
		}
		
		isRightRayActive = false;
		isLeftRayActive = false;
		
		player_dist = Player.GetComponent<PlayerController>().cameraDistance;
		
		last_wall_dist = 10f;
		canMoveRight = true;
		canMoveLeft = true;
		canMoveBack = true;
		
		lastBackDist = 100f;
		
		foundFwdTarget = false;
		fwdTarget = 10f;
		
		needToGoForward = false;
		
		fwdHit = null;
		
		
		
		oldPosition = transform.position;
		oldRotation = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,transform.eulerAngles.y);
		isGoingToFirstPerson = false;
		isGoingBackFromFirstPerson = false;
		
	}
	
	
	void Update () {
		//if(target.transform.position.y < target.transform.GetComponent<PlayerController>().start_y-100){
			//transform.position = new Vector3(1f,1f,1f) - offset;
		//}
		//Debug.Log(Input.GetAxis("CameraRotationHoriz"));
		//Debug.Log(transform.eulerAngles.y);
		if (Input.GetAxis("CameraRotationHoriz") > 0.0001f){
			isLeftRayActive = true;
			isRightRayActive = false;			
		}
		else if (Input.GetAxis("CameraRotationHoriz") < -0.0001f) {	
			isLeftRayActive = false;		
			isRightRayActive = true;			
		}
		else {
			isLeftRayActive = false;		
			isRightRayActive = false;				
		}
		//Debug.Log(canMoveRight);
		Vector3 rightRay = new Vector3(1f,0f,0f);
		rightRay = Quaternion.Euler(0, transform.eulerAngles.y, 0) * rightRay;
		//Debug.Log(rightRay);
		Ray right_ray = new Ray(transform.position, rightRay);
		//Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, right_ray.direction*10f, Color.red);
		if (Physics.Raycast(right_ray, out rightHit, raySize)){
			
			if(rightHit.transform.gameObject.tag == "Wall"){
				canMoveRight = false;
			}
			else {
				canMoveRight = true;
			}
			
	
		
		}
		else {
			canMoveRight = true;			
		}
		
		
		
		Vector3 leftRay = new Vector3(-1f,0f,0f);
		leftRay = Quaternion.Euler(0, transform.eulerAngles.y, 0) * leftRay;
		//Debug.Log(rightRay);
		Ray left_ray = new Ray(transform.position, leftRay);
		//Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, left_ray.direction*10f, Color.red);
		if (Physics.Raycast(left_ray, out leftHit, raySize)){
			
			if(leftHit.transform.gameObject.tag == "Wall" || leftHit.transform.gameObject.tag == "LadderWall"){
				canMoveLeft = false;
			}
			else {
				canMoveLeft = true;
			}
			
	
		
		}
		else {
			canMoveLeft = true;			
		}
		
		Vector3 backRay = new Vector3(0f,0f,-1f);
		backRay = Quaternion.Euler(0, transform.eulerAngles.y, 0) * backRay;
		//Debug.Log(rightRay);
		Ray back_ray = new Ray(transform.position, backRay);
		//Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, back_ray.direction*10f, Color.red);
		if (Physics.Raycast(back_ray, out backHit, 100f)){ //3f //change to 3f and remove backHitDistance but it seems to be working(just with a trebble)
			
		
			lastBackDist = backHit.distance;
			if((backHit.transform.gameObject.tag == "Wall" || backHit.transform.gameObject.tag == "LadderWall")  && backHit.distance < 0.5f){//
				canMoveBack = false;
			}
			else {
				canMoveBack = true;
			}
			
	
		
		}
		else {
			canMoveBack = true;			
		}
		
		
		player_dist = Mathf.Sqrt((transform.position.x-Player.transform.position.x)*(transform.position.x-Player.transform.position.x)+(transform.position.z-Player.transform.position.z)*(transform.position.z-Player.transform.position.z));
		
		Vector3 forwardRay = new Vector3(0f,0f,1f);
		forwardRay = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0) * forwardRay;
		//Debug.Log(rightRay);
		Ray forward_ray = new Ray(transform.position, forwardRay);
		//Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, forward_ray.direction*10f, Color.red);
		//Debug.Log(foundFwdTarget);
		if (Physics.Raycast(forward_ray, out forwardHit, 15f) && foundFwdTarget == false ){
			
			float wall_dist = forwardHit.distance;
			last_wall_dist = wall_dist;
			//player_dist = Mathf.Sqrt((transform.position.x-Player.transform.position.x)*(transform.position.x-Player.transform.position.x)+(transform.position.z-Player.transform.position.z)*(transform.position.z-Player.transform.position.z));
			if ((forwardHit.transform.gameObject.tag == "Wall" || forwardHit.transform.gameObject.tag == "LadderWall") && wall_dist < player_dist-1f) {
				
				needToGoForward = true;
				fwdHit = forwardHit.transform.gameObject;
				foundFwdTarget = true;
				
				fwdTarget = (player_dist-wall_dist)/3f;
				
				//fwdHit.GetComponent<Renderer>().enabled = false;
				
				
			}
			else if ((forwardHit.transform.gameObject.tag == "Wall" || forwardHit.transform.gameObject.tag == "LadderWall") && wall_dist >= player_dist-1f) {
				needToGoForward = false;
				//foundFwdTarget = false;
				//fwdHit = null;
			}
			else {
				if (fwdHit!=null)
					fwdHit.GetComponent<Renderer>().enabled = true;
				//needToGoForward = false;
				//fwdHit = null;
				
			}
			
		
			
	
		
		}
		else {
			needToGoForward = false;
			if (fwdHit != null)
				fwdHit.GetComponent<Renderer>().enabled = true;

			//fwdHit = null;
		}
		
		
		
		
		float head = 1.2f;
		float mult = 1f;
		if (Player.GetComponent<PlayerController>().isRotating)
			mult = 7f;
		
		if (isGoingToFirstPerson){
			float diff_x = Player.transform.position.x - transform.position.x;
			float diff_y = Player.transform.position.y+head - transform.position.y;
			float diff_z = Player.transform.position.z - transform.position.z;		
			float sensitivity = Player.GetComponent<PlayerController>().cameraSensitivity;

			transform.position += new Vector3(diff_x*sensitivity*Time.deltaTime*3f*mult,
											  diff_y*sensitivity*Time.deltaTime*3f*mult, 
											  diff_z*sensitivity*Time.deltaTime*3f*mult);
			
			
			Vector3 diff_vec = new Vector3(diff_x, diff_y, diff_z);
			//Debug.Log(diff_vec.magnitude);
			
			if (diff_vec.magnitude < 2f){
			
				if (Player.transform.GetChild(0).gameObject.activeSelf == true){
					for (int count = 0; count < 27; count++){
						 Player.transform.GetChild(count).gameObject.SetActive(false);
					 }
				}
				//transform.eulerAngles = new Vector3(transform.eulerAngles.x, Player.transform.eulerAngles.y, transform.eulerAngles.z);
				if (diff_vec.magnitude < 0.1f){
					transform.position = Player.transform.position + new Vector3(0f,head,0f);
					
					isFirstPerson = true;
					target = null;
				}
				
			}
											  
			
		}
		else if(isGoingBackFromFirstPerson) {
			float diff_x = oldPosition.x - transform.position.x;
			float diff_y = oldPosition.y - transform.position.y;
			float diff_z = oldPosition.z - transform.position.z;		
			float sensitivity = Player.GetComponent<PlayerController>().cameraSensitivity;

			transform.position += new Vector3(diff_x*sensitivity*Time.deltaTime*3f,
											  diff_y*sensitivity*Time.deltaTime*3f, 
											  diff_z*sensitivity*Time.deltaTime*3f);
			
			
			Vector3 diff_vec = new Vector3(diff_x, diff_y, diff_z);
			//Debug.Log(diff_vec.magnitude);
			
			if (diff_vec.magnitude < 6f){
			
				if (Player.transform.GetChild(0).gameObject.activeSelf == false){
					for (int count = 0; count < 27; count++){
						 Player.transform.GetChild(count).gameObject.SetActive(true);
					 }
				}
				//target = oldTarget;
				
			}
			if (diff_vec.magnitude < 0.1f) {
			
				isGoingBackFromFirstPerson = false;
				//transform.position = oldPosition + new Vector3(0f,-0.1f,0f);
				
				
			}
			
			
		}
		
		
		
		
		//moveDirection = new Vector3(Mathf.Cos(circleHori)*Input.GetAxis("Horizontal")*movespeed + Mathf.Cos(Mathf.PI/2)*Input.GetAxis("Vertical"),
								//	0,
								//	-Mathf.Sin(circleHori)* Input.GetAxis("Horizontal")*movespeed* + Mathf.Sin(Mathf.PI/2)*Input.GetAxis("Vertical")*movespeed);
		//if (Player.GetComponent<PlayerController>().isCamLockedTrack == false && isFirstPerson == false){
		if (isFirstPerson == false){
			transform.LookAt(target);
		}
		else {
			if (isFirstPerson) {
				float sensitivity = Player.GetComponent<PlayerController>().cameraSpeed;
				
				//float speed_horizontal = Input.GetAxis("CameraRotationHoriz");
				//float speed_vertical = Input.GetAxis("CameraRotationVertical");
				float speed_horizontal = Input.GetAxis("Horizontal");
				float speed_vertical = Input.GetAxis("Vertical");
				if ( Mathf.Abs(speed_horizontal) > 0.3f || Mathf.Abs(speed_vertical) > 0.3f ) {
					
					float mod = Mathf.Sqrt(speed_horizontal*speed_horizontal+speed_vertical*speed_vertical);
					
					Vector3 rotation_vec = new Vector3(sensitivity*Time.deltaTime*10f*speed_vertical/mod/1.5f,
																				sensitivity*Time.deltaTime*10f*speed_horizontal/mod/1.5f,
																				0f);
					transform.eulerAngles = transform.eulerAngles + rotation_vec;
					//Debug.Log(transform.eulerAngles.x);
					if (transform.eulerAngles.x < 365f && transform.eulerAngles.x > 265f){
						
						if (transform.eulerAngles.x < 270f+15f)
							transform.eulerAngles = new Vector3(270f+15f, transform.eulerAngles.y, transform.eulerAngles.z );

					}
					else if (transform.eulerAngles.x < 95f && transform.eulerAngles.x > -5f) {
						
						if (transform.eulerAngles.x > 90f-15f)
							transform.eulerAngles = new Vector3(90f-15f, transform.eulerAngles.y, transform.eulerAngles.z );
						
					}
					//transform.eulerAngles = new Vector3(Mathf.Clamp(transform.eulerAngles.x,-75f,75f), transform.eulerAngles.y, transform.eulerAngles.z );
																				
					
					
					
				}
				
				
				
				
				
				
			}
			
		}
		
		if (isGoingBackFromFirstPerson || isGoingToFirstPerson) {
		
			//transform.eulerAngles = new Vector3(transform.eulerAngles.x, oldRotation.y, transform.eulerAngles.z);
			
		}
		//}
		//else {
		//	transform.LookAt(Player.transform);
		//}
		
		if(Mathf.Abs(Input.GetAxis("Horizontal"))>0 || Mathf.Abs(Input.GetAxis("Vertical"))>0){
			//aproximate camera to the point in player so that the camera moves in a circular movement
		}
		//transform.position += moveDirection;
		//this.transform.Translate(moveDirection*Time.deltaTime);
	}
}
