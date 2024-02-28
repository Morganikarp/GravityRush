using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItemBase : MonoBehaviour
{
    protected int OffScreenXPos = 15; //The x position to which the menu items go to when off screen
    protected float TransitionSpeed = 15f; //The speed at which the menu items move on/off screen

    public bool MouseController() //public function determining mouse controls for the menu items, returning a bool
    {
        if (Input.GetMouseButtonDown(0)) //On left click...
        {
            RaycastHit2D RayHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); //Sends a ray perpendicular from the camera to the point in the world where the player clicked

            if (RayHit.collider != null && (RayHit.collider.gameObject == this.gameObject)) //If the ray hits something, and that something is the menu item calling this function
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        else
        {
            return false;
        }
    }
}