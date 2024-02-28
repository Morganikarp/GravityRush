using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Imported for changing scenes
using System.IO; //Imported for file handling

public class LevelSelection : MonoBehaviour
{
    public GameObject Map;
    public GameObject[] LevelIcons;

    Vector3 Y_Adjustment = new Vector3(0f, 0.6f, 0f); //Positions the player slightly above the level icon

    public int LevelSelected; //Selected level index
    public float Direction; //input variable determining if the level index is ascending or descending

    bool InputBuffer; //When a button is pressed, another action cannot be taken until button is unpressed

    public int MaxOpenLevelIndex; //The index of the highest accessible level
    public int FinalLevelIndex; //The index of the final level

    void Start()
    {
        FileStream fileStream = new FileStream(ActiveFileData.filePath, FileMode.Open, FileAccess.Read);
        StreamReader streamReader = new StreamReader(fileStream);
        streamReader.ReadLine();

        MaxOpenLevelIndex = int.Parse(streamReader.ReadLine()); //Reads from the save file the max level index that have been reached

        streamReader.Close();
        fileStream.Close();


        if (MaxOpenLevelIndex > 0)
        {
            LevelSelected = MaxOpenLevelIndex - 1; //Set the selected index to the level the player just finished
        }

        else
        {
            LevelSelected = 0;
        }

        Map = GameObject.Find("Map");
        LevelIcons = new GameObject[Map.transform.childCount];

        for (int i = 0; i < Map.transform.childCount; i++)
        {
            LevelIcons[i] = Map.transform.GetChild(i).gameObject;
        }

        InputBuffer = false;

        FinalLevelIndex = Map.transform.childCount - 1; //Set the index for the final level to be 1 less than the number of icons, to account for indexing
    }

    void Update()
    {
        Direction = Input.GetAxisRaw("Horizontal"); // "a" or left arrow = -1, "d" or right arrow = 1, neither = 0

        switch (Direction) //Outcome depends on if Direction is 1, -1 or 0
        {
            case 1:
                if (!InputBuffer && LevelSelected != FinalLevelIndex && LevelSelected != MaxOpenLevelIndex) //If the buffer isnt active and the currently selected level isn't the final or newest levels...
                {
                    LevelSelected++;
                    InputBuffer = true; //Disable inputs
                }
                break;
            case -1:
                if (!InputBuffer && LevelSelected != 0) //If the buffer isnt active and the currently selected level the first...
                {
                    LevelSelected--;
                    InputBuffer = true;
                }
                break;
            default:
                InputBuffer = false; //Allow inputs again
                break;
        }

        if (Input.GetMouseButtonDown(0)) //On left click...
        {
            RaycastHit2D RayHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); //Sends a ray perpendicular from the camera to the point in the world where the player clicked

            if (RayHit.collider != null && (RayHit.collider.gameObject == this.gameObject || RayHit.collider.gameObject.name == "GoIcon")) //If the ray hit something, and that something is either the player or the Go Icon...
            {
                SceneManager.LoadScene(LevelSelected + 2, LoadSceneMode.Single); //Load the scene with the same build index as the level selected variable
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(LevelSelected + 2, LoadSceneMode.Single);
        }

        transform.position = LevelIcons[LevelSelected].transform.position + Y_Adjustment;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
