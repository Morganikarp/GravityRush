using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuReturnItem : MenuItemBase //Inherits from MenuItemBase
{
    public bool TransMenu; //Represents when the menu is transitioning
    public bool rootOnScreen; //Represents if the root menu is on screen of not
    float destinationDif; // The difference that a menu must be within to have made it to the final location

    Transform RootMenuTrans; //The transform component of the root menu

    GameObject FileMenu; //The gameObject representing the file menu
    Transform MenuTrans; //The transform component of the file menu

    private void Start()
    {
        RootMenuTrans = GameObject.Find("RootMenu").transform;

        FileMenu = transform.parent.gameObject;
        MenuTrans = FileMenu.transform;

        TransMenu = false;
        rootOnScreen = true;
        destinationDif = 0.01f;
    }

    private void Update()
    {
        if (MouseController()) //If the return button is clicked on...
        {
            TransMenu = true;
            rootOnScreen = true;
        }

        if (TransMenu)
        {
            Move();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    void Move() //Moves the root menu off screen and the file menu on screen, and vice versa
    {
        if (rootOnScreen)
        {
            MenuTrans.position = new Vector3(Mathf.Lerp(MenuTrans.position.x, OffScreenXPos, Time.deltaTime * TransitionSpeed), 0f, 0f); //set the file menu's position to a linearly interpolated vector at some point between its on screen and off screen locations, based on the time between frames multipled by a speed value
            RootMenuTrans.position = new Vector3(Mathf.Lerp(RootMenuTrans.position.x, 0f, Time.deltaTime * TransitionSpeed), 0f, 0f); //set the root menu's position to a linearly interpolated vector at some point between its off screen and on screen locations, based on the time between frames multipled by a speed value

            if ((OffScreenXPos - destinationDif) < MenuTrans.position.x && MenuTrans.position.x < (OffScreenXPos + destinationDif)) //if the menu's x position within a certain distance to its final location...
            {
                MenuTrans.position = new Vector3(OffScreenXPos, MenuTrans.position.y, 0f); //set the file menu's position to its off screen vector
                RootMenuTrans.transform.position = new Vector3(0f, RootMenuTrans.position.y, 0f); //set the root menu's position to its on screen vector
                TransMenu = false; //The menu's have finished transitioning
            }
        }

        else if (!rootOnScreen)
        {
            MenuTrans.position = new Vector3(Mathf.Lerp(MenuTrans.position.x, 0f, Time.deltaTime * TransitionSpeed), 0f, 0f); //set the file menu's position to a linearly interpolated vector at some point between its off screen and on screen locations, based on the time between frames multipled by a speed value
            RootMenuTrans.position = new Vector3(Mathf.Lerp(RootMenuTrans.position.x, -OffScreenXPos, Time.deltaTime * TransitionSpeed), 0f, 0f); //set the root menu's position to a linearly interpolated vector at some point between its off screen and on screen locations, based on the time between frames multipled by a speed value

            if ((OffScreenXPos - destinationDif) < MenuTrans.position.x && MenuTrans.position.x < (OffScreenXPos + destinationDif)) //if the menu's x position within a certain distance to its final location...
            {
                MenuTrans.transform.position = new Vector3(0f, MenuTrans.position.y, 0f); //set the file menu's position to its on screen vector
                RootMenuTrans.transform.position = new Vector3(-OffScreenXPos, RootMenuTrans.position.y, 0f);//set the root menu's position to its off screen vector
                TransMenu = false; //The menu's have finished transitioning
            }
        }
    }
}
