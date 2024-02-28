using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //Imported for file handling
using UnityEngine.SceneManagement; // Imported for changing scenes

public class RootMenuItem : MenuItemBase //Inherits from MenuItemBase
{

    public bool isNewGame; //If this script is on the new game button, set to true, if not, set to false

    public GameObject FileMenu;
    FileMenuItem[] FileItems = new FileMenuItem[3];

    MenuReturnItem ReturnController;


    void Start()
    {
        for (int i = 0; i < FileMenu.transform.childCount; i++)
        {
            Transform subject = FileMenu.transform.GetChild(i);
            if (subject.tag == "SaveFile")
            {
                FileItems[i] = subject.GetComponent<FileMenuItem>();
            }
            else //If the child lacks the correct tag, then it must be the return button
            {
                ReturnController = subject.GetComponent<MenuReturnItem>();
            }
        }
    }

    void Update()
    {
        if (MouseController()) //If this item is clicked on...
        {
            switch (isNewGame)
            {
                case true: //If this is the new game button...
                    bool fileFree = false; //Creates a new bool to represent if there are any empty save files, and sets it as false by default

                    for (int i = 0; i <= FileItems.Length - 1; i++)
                    {
                        string currentPath = FileItems[i].filePath;

                        FileStream fileStream = new FileStream(currentPath, FileMode.Open, FileAccess.Read);
                        StreamReader streamReader = new StreamReader(fileStream);

                        if (streamReader.ReadLine() == "false") //If the first line of the subject save file is "false", it is empty
                        {
                            streamReader.Close();
                            fileStream.Close();

                            FileStream newFileStream = new FileStream(currentPath, FileMode.Open, FileAccess.Write); //Opens a file stream to wrrite to the active save file
                            StreamWriter streamWriter = new StreamWriter(newFileStream);

                            streamWriter.Write("true "); //Writes "true " to the active save file, as it is about to be used

                            streamWriter.Close();
                            newFileStream.Close();

                            fileFree = true; //A free file was found, so the variable changes to reflect that

                            ActiveFileData.filePath = currentPath;
                            SceneManager.LoadScene(1, LoadSceneMode.Single); //Loads the level select scene
                            break;
                        }

                        else
                        {
                            streamReader.Close();
                            fileStream.Close();
                        }
                    }

                    if (!fileFree) //If no free files were found...
                    {
                        for (int i = 0; i <= FileItems.Length - 1; i++)
                        {
                            FileItems[i].Override = true; //Every FileMenuItem component has their Override bool set to true, and they can now overwrite the data of their associate save files
                        }
                        ReturnController.TransMenu = true;
                        ReturnController.rootOnScreen = false;
                    }

                    break;
                case false: //If this is the load game button...
                    for (int i = 0; i <= FileItems.Length - 1; i++)
                    {
                        FileItems[i].Override = false;
                    }
                    ReturnController.TransMenu = true;
                    ReturnController.rootOnScreen = false;
                    break;
            }
        }

    }
}
