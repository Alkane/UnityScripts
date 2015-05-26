using UnityEngine;
using System.Collections;
//0=  Defense
//1=  Catch
//2 = 2
//3 = 3
//4 = 4
//5 = 6

[System.Serializable]
public class Ranges
{
	public float[] Range = new float[20];
	public string whichTypeOfShot;
}

public class Ball_Generate : MonoBehaviour {

	// Use this for initialization
	public float timer = 0;
	public float botTimer = 7;

	public GameObject ball;
	public GameObject BallOriginal;
	public GameObject[] ballPoints = new GameObject[2];
	static Ball_Generate myInstance;
	//public int whichTypeOfBall;
	public int whichPositionOfBall;
	public int whichSidesOfBall;
	public int currentWhichBall = 0;
	//public int[] whichSideOfBall = new int[4];

	//public float[,] Ranges = new float[4,5];
	public int[] typeOfRange = new int[10];

	public Ranges[] Straight_Ball = new Ranges[8];
	public Ranges[] InSwinger= new Ranges[8];
	public Ranges[] OutSwinger= new Ranges[8];
	public Ranges[] Leg_Spin= new Ranges[8];
	public Ranges[] OffSpin= new Ranges[8];

	public int balls = 0;
	public int whichTypeToActuallyBall = 0;

	public RuntimeAnimatorController[] Controllers = new RuntimeAnimatorController[6];

	public string currentTypeOfBall = "OutSwing-Ball";

	public	Ranges[] RangeToConsider;

	public GradientScript gradient;

	public GameObject otherBall;
	public GameObject marker;
	public GameObject animatedMarker;

	bool generate =false;
	int generated = 0;

	public bool toggleTimer = false;

	int whichRangeused = 0;

	//For Bot Balls
	public bool animation_Started = false;
	public Vector3 Start_Position;

	public BatController bControllerForThisPlayer;

	float differenceInBallPositions = 0;
	float B1Position;
	float B2Position;

	public ButtonTextureChange bRTexChange;
	public ButtonTextureChange bLTexChange;

	public Shadow_Marker shadow;

	public SkeletonAnimation bowlerSkeleton;

	public GameObject Button1, Button2, CameraTex,Wicket,Wicket2;

	public GameObject OverText, OverAlpha;

	public static Ball_Generate Instance
	{
		get
		{
			if (myInstance == null)
				myInstance = FindObjectOfType(typeof(Ball_Generate)) as Ball_Generate;
			
			return myInstance;
		}
	}

	void LateUpdate()
	{
		if( this.transform.parent.GetComponent<PlayerScript>().isBot && ball)
		{
			//animation_Started = false;
			//Debug.Log ("difference " + differenceInBallPositions);
			Vector3 position = ball.transform.position;
			position.x += differenceInBallPositions;
			ball.transform.position = position;
		}
	}
	void Start () {
//		for(int i=0;i<8;i++)
//		{
//			for(int j=0;j<20;j++)
//			{
//				Straight_Ball[i].Range[j] = Straight_Ball[5].Range[j];
//			}
//		}
		float B1Position = this.transform.parent.GetComponent<PlayerScript>().Player1.thisBall_Generate.transform.position.x;
		float B2Position = this.transform.parent.GetComponent<PlayerScript>().Player2.thisBall_Generate.transform.position.x;

		differenceInBallPositions = B2Position - B1Position;

		for(int i=0;i<8;i++)
		{

			Straight_Ball[i].Range[10] = Straight_Ball[i].Range[8] - 0.07f;   
			Straight_Ball[i].Range[11] = Straight_Ball[i].Range[9] + 0.07f;
			Straight_Ball[i].Range[12] = Straight_Ball[i].Range[11];
			Straight_Ball[i].Range[13] = Straight_Ball[i].Range[12] + 0.06f;
			Straight_Ball[i].Range[14] = Straight_Ball[i].Range[13];
			Straight_Ball[i].Range[15] = Straight_Ball[i].Range[14] + 0.05f;
			Straight_Ball[i].Range[16] = Straight_Ball[i].Range[15];
			Straight_Ball[i].Range[17] = Straight_Ball[i].Range[11]+ 0.02f;
			Straight_Ball[i].Range[18] = Straight_Ball[i].Range[17];
			Straight_Ball[i].Range[19] = 0.8f;

			Straight_Ball[i].Range[7] = Straight_Ball[i].Range[10];
			Straight_Ball[i].Range[6] = Straight_Ball[i].Range[7] - 0.06f;
			Straight_Ball[i].Range[5] = Straight_Ball[i].Range[6];
			Straight_Ball[i].Range[4] = Straight_Ball[i].Range[5] - 0.05f;
			Straight_Ball[i].Range[3] = Straight_Ball[i].Range[4];
			Straight_Ball[i].Range[2] = Straight_Ball[i].Range[3] - 0.02f;
			Straight_Ball[i].Range[1] = Straight_Ball[i].Range[2];
			Straight_Ball[i].Range[0] = Straight_Ball[i].Range[1] - 0.02f; 
		}
	}
	
	// Update is called once per frame
	void Update () {
		timer = timer + Time.deltaTime;
		if (timer > 6.5f && !this.transform.parent.GetComponent<PlayerScript>().isBot)
			StartCoroutine(GenerateBall ());
		else if(timer > botTimer && this.transform.parent.GetComponent<PlayerScript>().isBot)
		        {
			StartCoroutine(GenerateBall ());
			botTimer = 6.5f; 			//BOT LAG
		}

//		if(ball)
//		{
//		AnimationInfo[] infos = ball.GetComponent<Animator> ().GetCurrentAnimationClipState(0);
//		if(infos.Length > 0 && generate)
//		{
//			//Debug.Log (infos[0].clip.name);
//			//otherBall.animation["Straight-Right-50"].normalizedTime = 0.5f;
//			otherBall.GetComponent<Animator>().enabled = true;
//			int hash = ball.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).nameHash;
//			float percent  = (RangeToConsider[this.transform.parent.GetComponent<PlayerScript>().thisBatController.currentball].Range[8]+ RangeToConsider[this.transform.parent.GetComponent<PlayerScript>().thisBatController.currentball].Range[9])/2;
//			otherBall.GetComponent<Animator>().Play(hash,0,percent);
//			
//			if(generated == 0)
//			{
//				generated++;
//			}
//			else if(generated == 1)
//			{
//				generate = false;
//				otherBall.GetComponent<Animator>().enabled = false;
//				generated=0;
//				marker.transform.position = otherBall.transform.position;
//				marker.GetComponent<TweenScale>().ResetToBeginning();
//				marker.GetComponent<TweenScale>().PlayForward();
//			}
//		}
//		//Debug.Log (infos.Length);
//		}
	}
	public int whichBall = 0;
	public IEnumerator GenerateBall()
	{
		if(whichBall == 8)
		{
			whichBall = 0;
		}
		//whichBall = Random.Range (0, 7);
	//	whichBall = 4;
		timer = 0;
		whichBall = Random.Range (0, 2);
		RangeToConsider = Straight_Ball;
		//whichBall = 1;
		if (balls > 5 && this.transform.parent.GetComponent<PlayerScript> ().thisBatController.numberOfBallsByCB < 24) {
						if (currentTypeOfBall == "Straight") {
								balls = 0;
								currentTypeOfBall = "InSwing";
								whichTypeToActuallyBall = 1;
								this.transform.parent.GetComponent<PlayerScript> ().thisBatController.ShowTimingText ("InSwing Bowler");
								RangeToConsider = OutSwinger;
								timer -= 2;
								if (!this.transform.parent.GetComponent<PlayerScript> ().isBot) {
										Timer.Instance.GenerateTimer (InSwinger [0].Range);
										//OverAlpha.SetActive(true);
										OverText.GetComponent<UILabel>().text = "Over " + ((this.transform.parent.GetComponent<PlayerScript> ().thisBatController.numberOfBallsByCB/6) + 1).ToString();
										OverText.GetComponent<TweenPosition>().ResetToBeginning();
										OverAlpha.GetComponent<TweenAlpha>().ResetToBeginning();
										OverAlpha.GetComponent<TweenAlpha>().PlayForward();

										TweenAlpha[] tweens = OverText.GetComponents<TweenAlpha>();
										for(int i=0;i<tweens.Length;i++)
										{
											if(tweens[i].tweenGroup == 0)
											{
												tweens[i].ResetToBeginning();
												tweens[i].PlayForward();
											}
										}

										yield return new WaitForSeconds(2);
										
										OverText.GetComponent<TweenPosition>().PlayForward();

										Button1.SetActive (false);
										Button2.SetActive (false);
										if(GameController.Instance.isPvP)
										CameraTex.SetActive (false);
										Wicket.SetActive(false);
										Wicket2.SetActive(false);
										this.transform.parent.GetComponent<PlayerScript> ().thisBatController.OverAnimation.stop ();
										this.transform.parent.GetComponent<PlayerScript> ().thisBatController.OverAnimation.play ();

								}
								timer -= 7;	
								yield return new WaitForSeconds (7);
								if(!this.transform.parent.GetComponent<PlayerScript> ().isBot)
								{
									TweenAlpha[] tweens = OverText.GetComponents<TweenAlpha>();
									for(int i=0;i<tweens.Length;i++)
									{
										if(tweens[i].tweenGroup == 1)
										{
											tweens[i].ResetToBeginning();
											tweens[i].PlayForward();
										}
									}
									//OverText.GetComponent<TweenAlpha>().PlayReverse();
								}

						} else {
								balls = 0;
								currentTypeOfBall = "Straight";
								whichTypeToActuallyBall = 0;
								this.transform.parent.GetComponent<PlayerScript> ().thisBatController.ShowTimingText ("Straight Bowler");
								timer -= 2;
								RangeToConsider = Straight_Ball;
								if (!this.transform.parent.GetComponent<PlayerScript> ().isBot) {
										Timer.Instance.GenerateTimer (Straight_Ball [0].Range);
										OverText.GetComponent<UILabel>().text = "Over " + ((this.transform.parent.GetComponent<PlayerScript> ().thisBatController.numberOfBallsByCB/6) + 1).ToString();
										OverText.GetComponent<TweenPosition>().ResetToBeginning();
										OverAlpha.GetComponent<TweenAlpha>().ResetToBeginning();
										OverAlpha.GetComponent<TweenAlpha>().PlayForward();
										TweenAlpha[] tweens = OverText.GetComponents<TweenAlpha>();
										for(int i=0;i<tweens.Length;i++)
										{
											if(tweens[i].tweenGroup == 0)
											{
												tweens[i].ResetToBeginning();
												tweens[i].PlayForward();
											}
										}

										yield return new WaitForSeconds(2);
										
										OverText.GetComponent<TweenPosition>().PlayForward();

										Button1.SetActive (false);
										Button2.SetActive (false);
										CameraTex.SetActive (false);
										Wicket.SetActive(false);
										this.transform.parent.GetComponent<PlayerScript> ().thisBatController.OverAnimation.stop ();
										this.transform.parent.GetComponent<PlayerScript> ().thisBatController.OverAnimation.play ();
								}
								timer -= 7;	
								yield return new WaitForSeconds (7);	
								if(!this.transform.parent.GetComponent<PlayerScript> ().isBot)
								{
										TweenAlpha[] tweens = OverText.GetComponents<TweenAlpha>();
										for(int i=0;i<tweens.Length;i++)
										{
											if(tweens[i].tweenGroup == 1)
											{
												tweens[i].ResetToBeginning();
												tweens[i].PlayForward();
											}
										}

								}
								
						}
						if(!this.transform.parent.GetComponent<PlayerScript> ().isBot)
						{
							Button1.SetActive (true);
							Button2.SetActive (true);
							if(GameController.Instance.isPvP)
							CameraTex.SetActive (true);
							Wicket.SetActive(true);
							Wicket2.SetActive(false);
						}
		} 
		else if(this.transform.parent.GetComponent<PlayerScript> ().thisBatController.numberOfBallsByCB >= 24) {
					GameController.Instance.doWinAnimation();
				}

		this.transform.parent.GetComponent<PlayerScript> ().thisBatController.DoOut(false);
		//yield return new WaitForSeconds (1f);
		yield return new WaitForSeconds (0.35f);

		if(whichBall < 4)
		ball = (GameObject)GameObject.Instantiate (BallOriginal, this.transform.Find("RightPos").position, Quaternion.identity);
		else
		ball = (GameObject)GameObject.Instantiate (BallOriginal, this.transform.position, Quaternion.identity);
		balls++;
		shadow.gameObject.SetActive (true);
		Start_Position = ball.transform.localPosition;

		//if(!this.transform.parent.GetComponent<PlayerScript>().isBot)
		

		if (this.transform.parent.GetComponent<PlayerScript> ().isBot)
		{
			ball.GetComponent<Ball> ().whichPlayer = 2;

		}
		else
		{
				ball.GetComponent<Ball> ().whichPlayer = 1;
				//bLTexChange.ResetFill();
		}




		this.transform.parent.GetComponent<PlayerScript>().thisBatController.numberOfBallsByCB++;
		gradient.GetComponent<GradientScript> ().buttonPressed = false;
	
		//this.transform.parent.GetComponent<PlayerScript>().thisBatController.canHit = true;
		this.transform.parent.GetComponent<PlayerScript>().thisBatController.hitRightShot = false;
		this.transform.parent.GetComponent<PlayerScript>().thisBatController.hitLeftShot = false;

		this.transform.parent.GetComponent<PlayerScript>().thisBatController.LeftShotCount = 0;
		this.transform.parent.GetComponent<PlayerScript>().thisBatController.RightShotCount = 0;
		this.transform.parent.GetComponent<PlayerScript>().thisBatController.loftShot = false;
		this.transform.parent.GetComponent<PlayerScript>().thisBatController.Batsman.SetActive (true);
		this.transform.parent.GetComponent<PlayerScript>().thisBatController.loftBatsman.gameObject.SetActive(false);

		ball.GetComponent<Animator> ().runtimeAnimatorController = Controllers [whichTypeToActuallyBall];
		otherBall.GetComponent<Animator> ().runtimeAnimatorController = Controllers [whichTypeToActuallyBall];
		//shadow.otherBall.GetComponent<Animator> ().runtimeAnimatorController = Controllers [whichTypeToActuallyBall];

		//gradient.GetComponent<GradientScript> ().ResetTicker ();
		if(whichBall == 0)
		{
		whichBall = Random.Range (0, 4);
		}
		else
		{
		whichBall = Random.Range (4, 8);
		}
		//this.transform.parent.GetComponent<PlayerScript>().thisBatController.ShowBallText (0, false);
		int hash = Animator.StringToHash (GetWhichTypeOfShot (whichBall));
		StartCoroutine(shadow.initiateStart (whichBall, currentTypeOfBall,ball.transform.position,hash));
		float percent  = (RangeToConsider[this.transform.parent.GetComponent<PlayerScript>().thisBatController.currentball].Range[8]+ RangeToConsider[this.transform.parent.GetComponent<PlayerScript>().thisBatController.currentball].Range[9])/2;
		otherBall.GetComponent<Animator>().enabled = true;
		otherBall.GetComponent<Animator>().Play(hash,0,percent);
		yield return 0;

		otherBall.GetComponent<Animator>().Play(hash,0,percent);
		otherBall.GetComponent<Animator>().enabled = false;
//		marker.transform.position = otherBall.transform.position;
//		marker.GetComponent<TweenScale>().ResetToBeginning();
//		marker.GetComponent<TweenScale>().PlayForward();

		Vector3 temppos = otherBall.transform.position;
		temppos.z = -1.25f;
		if (!this.transform.parent.GetComponent<PlayerScript> ().isBot) {
						animatedMarker.transform.position = temppos;
						StartCoroutine (animatedMarker.GetComponent<RotateMarker> ().StartRotation (whichBall, currentTypeOfBall));
				}
		yield return new WaitForSeconds(0.7f);

		bowlerSkeleton.state.SetAnimation (0, "animation", false); //Bowler Animation

		yield return new WaitForSeconds(0.3f);

		if (!this.transform.parent.GetComponent<PlayerScript> ().isBot)
		{
			int ReadyPose = Animator.StringToHash("ReadyPose");
			this.transform.parent.GetComponent<PlayerScript> ().thisBatController.Batsman.GetComponent<Animator>().SetTrigger(ReadyPose);
		}


		
		this.transform.parent.GetComponent<PlayerScript>().thisBatController.currentball = whichBall;
		whichRangeused = whichBall;

		if(whichBall < 4)
		{
			if( whichBall == 0)
			{
				int putBallHast = Animator.StringToHash ("50");
				ball.GetComponent<Animator> ().SetTrigger (putBallHast);
				generate = true;
			}
			if( whichBall == 1)
			{
				int putBallHast = Animator.StringToHash ("70");
				ball.GetComponent<Animator> ().SetTrigger (putBallHast);
				generate = true;
			}
			if( whichBall == 2)
			{
				int putBallHast = Animator.StringToHash ("80");
				ball.GetComponent<Animator> ().SetTrigger (putBallHast);
				generate = true;
			}
			if( whichBall == 3)
			{
				int putBallHast = Animator.StringToHash ("90");
				ball.GetComponent<Animator> ().SetTrigger (putBallHast);
				generate = true;
			}
			int putBallHast2 = Animator.StringToHash ("Right");
			ball.GetComponent<Animator> ().SetTrigger (putBallHast2);
			ball.GetComponent<SpriteRenderer>().enabled = true;
		}
		else
		{
			int tempwhichBall = whichBall - 4;
			if( tempwhichBall == 0)
			{
				int putBallHast = Animator.StringToHash ("50");
				generate = true;
				ball.GetComponent<Animator> ().SetTrigger (putBallHast);
			}
			if( tempwhichBall == 1)
			{
				int putBallHast = Animator.StringToHash ("70");
				generate = true;
				ball.GetComponent<Animator> ().SetTrigger (putBallHast);
			}
			if( tempwhichBall == 2)
			{
				int putBallHast = Animator.StringToHash ("80");
				generate = true;
				ball.GetComponent<Animator> ().SetTrigger (putBallHast);

			}
			if( tempwhichBall == 3)
			{
				int putBallHast = Animator.StringToHash ("90");
				generate = true;
				ball.GetComponent<Animator> ().SetTrigger (putBallHast);
			}
			int putBallHast2 = Animator.StringToHash ("Left");
			generate = true;
			ball.GetComponent<Animator> ().SetTrigger (putBallHast2);
			ball.GetComponent<SpriteRenderer>().enabled = true;
		}

		animation_Started = true;

		whichBall++;
		if(this.transform.parent.GetComponent<PlayerScript>().isBot)
		{
			//Debug.Log ("executed");
			StartCoroutine(this.transform.parent.GetComponent<PlayerScript>().Player2.thisBatController.PlayShot());
		}
		yield return new WaitForSeconds (0.5f);

		//


//		if(whichBall == 0)
//		{
//			int putBallHast = Animator.StringToHash ("Straight");
//			ball.GetComponent<Animator> ().SetTrigger (putBallHast);
//		}
//		else if (whichBall == 1){
//			int putBallHast = Animator.StringToHash ("Straight-Left");
//			ball.GetComponent<Animator> ().SetTrigger (putBallHast);
//		}
//		else  if (whichBall == 2){
//			int putBallHast = Animator.StringToHash ("Straight-90");
//			ball.GetComponent<Animator> ().SetTrigger (putBallHast);
//		}
//		else  if (whichBall == 3){
//			int putBallHast = Animator.StringToHash ("Straight-50");
//			ball.GetComponent<Animator> ().SetTrigger (putBallHast);
//		}

//		if(whichBall == 0)
//		{
//			int putBallHast = Animator.StringToHash ("Straight-Right-70");
//			ball.GetComponent<Animator> ().SetTrigger (putBallHast);
//		}
//		else if (whichBall == 1){
//			int putBallHast = Animator.StringToHash ("Straight-Left-70");
//			ball.GetComponent<Animator> ().SetTrigger (putBallHast);
//		}
//		else  if (whichBall == 2){
//			int putBallHast = Animator.StringToHash ("Straight-Left-50");
//			ball.GetComponent<Animator> ().SetTrigger (putBallHast);
//		}
//		else  if (whichBall == 3){
//			int putBallHast = Animator.StringToHash ("Straight-50");
//			ball.GetComponent<Animator> ().SetTrigger (putBallHast);
//		}



	}

	string GetWhichTypeOfShot(int currentball)
	{
		string typeOfBall = currentTypeOfBall;
		if(whichBall < 4)
		{
			if( whichBall == 0)
			{
				typeOfBall = typeOfBall + "-Right-50";
			}
			if( whichBall == 1)
			{
				typeOfBall = typeOfBall + "-Right-70";
			}
			if( whichBall == 2)
			{
				typeOfBall = typeOfBall + "-Right-80";
			}
			if( whichBall == 3)
			{
				typeOfBall = typeOfBall + "-Right-90";
			}

		}
		else
		{
			int tempwhichBall = whichBall - 4;
			if( tempwhichBall == 0)
			{
				typeOfBall = typeOfBall + "-Left-50";
			}
			if( tempwhichBall == 1)
			{
				typeOfBall = typeOfBall + "-Left-70";
			}
			if( tempwhichBall == 2)
			{
				typeOfBall = typeOfBall + "-Left-80";
			}
			if( tempwhichBall == 3)
			{
				typeOfBall = typeOfBall + "-Left-90";
			}
		}
		return typeOfBall;
	}




}
