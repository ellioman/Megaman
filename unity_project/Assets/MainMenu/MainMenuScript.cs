using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

    void Awake()
    {       
        //Make this script persistent(Not destroy when loading a new level)
        DontDestroyOnLoad(this);

        Time.timeScale = 1.0f; //In case some game does not UN-pause..
    }

	void OnGUI () {    

        //Detect if we're in the main menu scene
        if (Application.loadedLevel == 0)
        {
            MainMenuGUI();
        }
        else
        {
            //Game scene
            InGameGUI();
        }	
	}

    void StartGame(int nr)
    {
        Application.LoadLevel(nr);
    }

    void InGameGUI()
    {
        //Top-right
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, 50));
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Back to menu"))
        {
            Destroy(gameObject); //Otherwise we'd have two of these..
            Application.LoadLevel(0);
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    public GUIStyle invisibleButton;

    void MainMenuGUI()
    {
        int leftPix = (Screen.width - 600) / 2;
        int topPix = (Screen.height - 450) / 2;

        if (GUI.Button(new Rect(leftPix, topPix, 204, 158), "", invisibleButton))
        {
            StartGame(1);
        }
        if (GUI.Button(new Rect(leftPix + 204, topPix, 204, 158), "", invisibleButton))
        {
            StartGame(2);
        }
        if (GUI.Button(new Rect(leftPix + 204 * 2, topPix, 204, 158), "", invisibleButton))
        {
            StartGame(3);
        }


        if (GUI.Button(new Rect(leftPix, topPix + 290, 204, 158), "", invisibleButton))
        {
            StartGame(4);
        }
        if (GUI.Button(new Rect(leftPix + 204, topPix + 290, 204, 158), "", invisibleButton))
        {
            StartGame(5);
        }
        if (GUI.Button(new Rect(leftPix + 204 * 2, topPix + 290, 204, 158), "", invisibleButton))
        {
            Application.OpenURL("http://www.M2H.nl");
        }


        GUI.color = Color.black;

        GUILayout.BeginArea(new Rect(Screen.width/2-150, Screen.height/2-100, 300, 200));
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();

        GUILayout.Label("Select a game!");
       
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

    }


   

}
