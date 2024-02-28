using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //Imported for file handling
using UnityEngine.SceneManagement; // Imported for changing scenes

public class FileMenuItem : MenuItemBase //Inherits from MenuItemBase
{

    public string fileName;
    public string filePath;

    public bool Override; //If true, the selected save file will be overwritten to reset the players progress

    SpriteRenderer spriteRenderer;
    public Sprite[] states = new Sprite[4]; //Array of the possible sprites the file menu item could be, depending on the players progress

    string[] newFileContent = new string[2] { "true ", "0" }; //An array of what an empty save file should contain

    private void Start()
    {
        filePath = Application.dataPath + "/SaveFiles/" + fileName; //the file path to this menu items associated save file
        Override = false;

        spriteRenderer = GetComponent<SpriteRenderer>();

        FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        StreamReader streamReader = new StreamReader(fileStream);

        if (streamReader.ReadLine() == "true ") //If the first line of the associated save file is "true "..
        {
            spriteRenderer.sprite = states[int.Parse(streamReader.ReadLine()) + 1]; //The file's sprite changes to show what level the player has reached in this save file
        }

        else
        {
            spriteRenderer.sprite = states[0]; //If the file is new, the "empty" sprite will show
        }

        streamReader.Close();
        fileStream.Close();
    }

    void Update()
    {
        if (MouseController()) //If clicked on...
        {

            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            streamWriter.WriteLine(newFileContent[0]);

            if (Override) //If there are no empty save files, the player will have the choice to rest one save file
            {
                streamWriter.WriteLine(newFileContent[1]); //Overwrites the associated save file with contents that make it appear empty
            }

            streamWriter.Close();
            fileStream.Close();

            ActiveFileData.filePath = filePath;
            SceneManager.LoadScene(1, LoadSceneMode.Single); //Load the level select scene

        }
    }

}