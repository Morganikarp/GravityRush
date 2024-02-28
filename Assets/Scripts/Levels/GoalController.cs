using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalController : MonoBehaviour
{

    int currentLevelIndex;
    int fileLevelIndex;
    int fileLevelIndexMax;

    private void Start()
    {
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex - 2;
        fileLevelIndex = 0;
        fileLevelIndexMax = 2;
    }
    private void OnTriggerEnter2D(Collider2D subject)
    {
        if (subject.gameObject.tag == "Player")
        {
            FileStream fileStream = new FileStream(ActiveFileData.filePath, FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new StreamReader(fileStream);

            streamReader.ReadLine();
            fileLevelIndex = int.Parse(streamReader.ReadLine());

            streamReader.Close();
            fileStream.Close();

            if (fileLevelIndex == currentLevelIndex && fileLevelIndex != fileLevelIndexMax)
            {
                int nextLevelIndex = currentLevelIndex + 1;
                FileStream newFileStream = new FileStream(ActiveFileData.filePath, FileMode.Open, FileAccess.ReadWrite);
                StreamWriter streamWriter = new StreamWriter(newFileStream);

                newFileStream.SetLength(5);
                streamWriter.WriteLine("true ");
                streamWriter.Write(nextLevelIndex);

                streamWriter.Close();
                newFileStream.Close();
            }

            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
    }
}
