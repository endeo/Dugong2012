using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {
	
	public Texture2D TextBox;
	public Texture2D guiBars;
	public Texture2D wongaPouch;
	
	static bool isEnd;
	static string currentText;
	static int currentPage;
	static string[] PageContent;
	
	static float fadeAlpha;
	
	bool fadeOverlayActive;
	bool DebugOverlayActivated;
	bool HelmSelectActivated;
	bool LevelSelectActivated;
	
	
	static int EndPlayerState;
	
	public GUIStyle normalText;
	public GUIStyle signText;
	public GUIStyle guiTextOne;
	
	GUIStyle SelectedStyle;
	
	static GameObject PlayerObject;
	
	private int FramesPerSecond;
	private int FrameCount;
	private double LastInterval;
	
	static int currentMoney;
	
	public Texture2D NewCursor;
	
	
	void Start()
	{
		
		LastInterval = 1.0f;
		FrameCount = 0;
		FramesPerSecond = 0;
		PlayerObject = GameObject.Find("You");
		SelectedStyle = normalText;
		currentPage = 0;
		PageContent = new string[9];
		for(int tempI = 0; tempI < 9; tempI++)
		{
			ChangePage(tempI, "", "Normal");
		}
		DebugOverlayActivated = false;
		HelmSelectActivated = false;
		LevelSelectActivated = false;
		fadeOverlayActive = false;
		currentMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
	}
	
	void Update()
	{
		//"`" Opens the Debug Menu 
		if (Input.GetKeyDown(KeyCode.BackQuote))
		{
        	if(DebugOverlayActivated)
			{
				DebugOverlayActivated = false;
			}
			else
			{
				DebugOverlayActivated = true;
			}
		}
		
		//Calculate FPS
		++FramesPerSecond;
		if(Time.time > LastInterval)
		{
			FrameCount = FramesPerSecond;
			FramesPerSecond = 0;
			LastInterval = Time.time + 1.0f;
		}
		if(currentMoney < PlayerPrefs.GetInt("PlayerMoney"))
		{
			currentMoney++;
		}
		if(currentMoney > PlayerPrefs.GetInt("PlayerMoney"))
		{
			currentMoney--;
		}
		
	}
	
	
	void OnGUI()
	{
		if(transform.GetComponent<PlayerController>().GetPlayerState() == 5 || transform.GetComponent<PlayerController>().GetPlayerState() == 3)
		{
			GUI.Label(new Rect(50,(Screen.height - 150),1050, 3570), guiBars);
			GUI.Label(new Rect((Screen.width - 250),(Screen.height - 75),1050, 3570), wongaPouch);
			GUI.Label(new Rect((Screen.width - 190),(Screen.height - 56),1050, 3570), "X", guiTextOne);
			GUI.Label(new Rect((Screen.width - 160),(Screen.height - 56),1050, 3570), currentMoney.ToString(), guiTextOne);
		}
		//Displays the last signtext that has been fed into PageContent
		if(transform.GetComponent<PlayerController>().GetPlayerState() == 2)
		{
			print("Checking " + currentPage);
			if(PageContent[currentPage] != "")
			{
				//print ("Displaying page " + currentPage);
				GUI.Label(new Rect(((Screen.width / 2) - 500),(Screen.height - 320),1000, 300), TextBox);
				GUI.Label(new Rect(((Screen.width / 2) - 500),(Screen.height - 320),1000, 300), PageContent[currentPage], SelectedStyle);
			}
			
			if(PageContent[currentPage] == "")
			{
				transform.GetComponent<PlayerController>().ChangePlayerState(EndPlayerState);
				for(int tempI = 0; tempI < 9; tempI++)
				{
					ChangePage(tempI, "", "Normal");
				}
				currentPage = 0;
			}
		}
		
		if(transform.GetComponent<PlayerController>().GetPlayerState() != 5)
		{
			GUI.Label(new Rect(Input.mousePosition.x, (Screen.height - Input.mousePosition.y), 210, 200), NewCursor);
		}
		
		
		
		//Displays the Debug Overlay
		if(DebugOverlayActivated)
		{
			GUI.Box(new Rect(10,10,200,400), "Character Info");
			GUI.Label(new Rect(10, 30, 200, 20), "Position(Vector3):");
			GUI.Label(new Rect(10, 45, 200, 20), PlayerObject.transform.position.ToString());
			GUI.Label(new Rect(10, 65, 200, 20), "Rotation(Quaternion):");
			GUI.Label(new Rect(10, 80, 200, 20), PlayerObject.transform.rotation.ToString());
			GUI.Label(new Rect(10, 100, 200, 20), "Camera Rotation(Quaternion):");
			GUI.Label(new Rect(10, 120, 200, 20), Camera.main.transform.rotation.ToString());
			if (GUI.Button (new Rect (10,225,200,20), "Whop your Wad on the Counter"))
			{
				int newMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
				newMoney += 100;
				PlayerPrefs.SetInt("PlayerMoney", newMoney);
			}
			if (GUI.Button (new Rect (10,200,200,20), "Lose Wonga"))
			{
				int newMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
				newMoney -= 100;
				PlayerPrefs.SetInt("PlayerMoney", newMoney);
			}
			if (GUI.Button (new Rect (10,250,200,20), "Give All"))
			{
				PlayerPrefs.SetInt("HasHelmet2", 1);
				PlayerPrefs.SetInt("HasHelmet3", 1);
				PlayerPrefs.SetInt("HasHelmet4", 1);
				PlayerPrefs.SetInt("HasHelmet5", 1);
				PlayerPrefs.SetInt("HasHelmet6", 1);
				PlayerPrefs.SetInt("HasHelmet7", 1);
				PlayerPrefs.SetInt("HasHelmet8", 1);
				PlayerPrefs.SetInt("HasHelmet9", 1);
				PlayerPrefs.SetInt("HasHelmet10", 1);
				
				PlayerPrefs.SetInt("HasWeapon1", 1);
				PlayerPrefs.SetInt("HasWeapon2", 1);
				PlayerPrefs.SetInt("HasWeapon3", 1);
				PlayerPrefs.SetInt("HasWeapon4", 1);
				PlayerPrefs.SetInt("HasWeapon5", 1);
			}
			if (GUI.Button (new Rect (10,275,200,20), "Lose All"))
			{
				PlayerPrefs.SetInt("HasHelmet2", 0);
				PlayerPrefs.SetInt("HasHelmet3", 0);
				PlayerPrefs.SetInt("HasHelmet4", 0);
				PlayerPrefs.SetInt("HasHelmet5", 0);
				PlayerPrefs.SetInt("HasHelmet6", 0);
				PlayerPrefs.SetInt("HasHelmet7", 0);
				PlayerPrefs.SetInt("HasHelmet8", 0);
				PlayerPrefs.SetInt("HasHelmet9", 0);
				PlayerPrefs.SetInt("HasHelmet10", 0);
				
				PlayerPrefs.SetInt("HasWeapon1", 0);
				PlayerPrefs.SetInt("HasWeapon2", 0);
				PlayerPrefs.SetInt("HasWeapon3", 0);
				PlayerPrefs.SetInt("HasWeapon4", 0);
				PlayerPrefs.SetInt("HasWeapon5", 0);
			}
			if (GUI.Button (new Rect (10,300,200,20), "Equip Select")) 
			{
				if(HelmSelectActivated)
				{
					HelmSelectActivated = false;
				}
				else
				{
					HelmSelectActivated = true;
				}
			}
			
			
			GUI.Box (new Rect (Screen.width - 200, 10, 200, 90), "Application Info");
			GUI.Label(new Rect(Screen.width - 200, 30, 200, 20), "Level Index:");
			GUI.Label(new Rect(Screen.width - 200, 45, 200, 20), Application.loadedLevelName + " (" + Application.loadedLevel.ToString() + ")");
			GUI.Label(new Rect(Screen.width - 200, 65, 200, 20), "FPS:");
			GUI.Label(new Rect(Screen.width - 200, 80, 200, 20), FrameCount.ToString());
			if (GUI.Button (new Rect (Screen.width - 200, 300, 200, 20), "Level Select")) 
			{
				if(LevelSelectActivated)
				{
					LevelSelectActivated = false;
				}
				else
				{
					LevelSelectActivated = true;
				}
			}
			
			if(HelmSelectActivated)
			{
				GUI.Box(new Rect(205,10,200,200), "Helm Selector");
				if(GUI.Button (new Rect(205, 30, 200, 20), "None"))
				{
					transform.GetComponent<PlayerController>().ChangeHelmet("None");
				}
				if(GUI.Button (new Rect(205, 53, 200, 20), "Ascended Helm"))
				{
					transform.GetComponent<PlayerController>().ChangeHelmet("Ascended");
				}
				if(GUI.Button (new Rect(205, 76, 200, 20), "Hatchet"))
				{
					transform.GetComponent<PlayerController>().ChangeWeapon("Hatchet");
				}
				if(GUI.Button (new Rect(205, 99, 200, 20), "Ascended Axe"))
				{
					transform.GetComponent<PlayerController>().ChangeWeapon("SheepAxe");
				}
			}
			if(LevelSelectActivated)
			{
				GUI.Box(new Rect(Screen.width - 405,10,200,200), "Level Selector");
				if(GUI.Button (new Rect(Screen.width - 405,30,200,20), "Main Menu"))
				{
					Application.LoadLevel("MainMenu");
				}
				if(GUI.Button (new Rect(Screen.width - 405,53,200,20), "Playground"))
				{
					Application.LoadLevel("TestingArena");
				}
				if(GUI.Button (new Rect(Screen.width - 405,76,200,20), "Demo Dungeon"))
				{
					Application.LoadLevel("DemoDungeon");
				}
			}
			
			
			
		}
	}
	
	
	public void NextPage()
	{
		currentPage += 1;
	}
	
	public void PageReset()
	{
		currentPage = 0;
	}
	
	public void ChangePage(int PageSelect, string NewPageContent, string StyleSelect, int EndState)
	{
		PageContent[PageSelect] = NewPageContent;
		//print ("Setting page " + PageSelect + " as " + PageContent);
		switch (StyleSelect)
		{
			case "Sign":
				SelectedStyle = signText;
				break;
			case "Normal":
				SelectedStyle = normalText;
				break;
			default:
				SelectedStyle = normalText;
				break;
		}
		EndPlayerState = EndState;
	}
	
	
	public void ChangePage(int PageSelect, string NewPageContent, string StyleSelect)
	{
		PageContent[PageSelect] = NewPageContent;
		//print ("Setting page " + PageSelect + " as " + PageContent);
		switch (StyleSelect)
		{
			case "Sign":
				SelectedStyle = signText;
				break;
			case "Normal":
				SelectedStyle = normalText;
				break;
			default:
				SelectedStyle = normalText;
				break;
		}
	}
	
	
	

	
}
