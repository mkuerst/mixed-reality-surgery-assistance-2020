using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTrigger : MonoBehaviour
{
    private Color default_color;
    private Color other_default_color;
    private Renderer rend;
    GameObject bone;
    GameObject bonemix;
    GameObject screwChild;
    public Material selectedScrewMaterial;
    public bool selectedFlag;
    int counter;

    // Start is called before the first frame update
    void Start()
    {
        bone = GameObject.Find("Bone");
        bonemix = GameObject.Find("BoneMix");
        screwChild= GameObject.Find("screwChild");
        rend = gameObject.GetComponent<Renderer>();
        
    }


    public void OnTriggerEnter(Collider other)
    {
        //check null references
        if (!other.gameObject.transform.parent || !gameObject.transform.parent)
        {
            return;
        }
        //ignore collisions with handles
        if (other.gameObject.transform.parent.name == "rigRoot" || gameObject.transform.parent.name == "rigRoot")
        {
            return;
        }
        //ignore collisions with bone
        if (other.gameObject.transform.parent.transform.IsChildOf(bone.transform) || other.gameObject.transform == bonemix.transform) //if (other.gameObject.transform.parent.parent.name == "Bone")
        {
            return;
        }
        else
        {
            Debug.Log(gameObject.name + " was triggered by " + other.gameObject.name);
            //note the colors before colliding
            if (gameObject.GetComponent<Renderer>() != null)
            {
                default_color = gameObject.GetComponent<Renderer>().material.color;
            }
            if (other.gameObject.GetComponent<Renderer>() != null)
            {
                other_default_color = other.gameObject.GetComponent<Renderer>().material.color;
            }

        }
    }

    public void OnTriggerStay(Collider other)
    { 
        if (!other.gameObject.transform.parent || !gameObject.transform.parent)
        {
            return;
        }
        //ignore collisions with handles
        if (other.gameObject.transform.parent.name == "rigRoot" || gameObject.transform.parent.name == "rigRoot")
        {
            return;
        }
        //ignore collisions with bone
        if (other.gameObject.transform.parent.transform.IsChildOf(bone.transform) || other.gameObject.transform == bonemix.transform) //if (other.gameObject.transform.parent.parent.name == "Bone")
        {
            return; 
        }
        // ignore collisions with any children attached to the screw -- works only on the non-selected screws
        if (gameObject.transform.IsChildOf(other.gameObject.transform)) 
        {
            return;
        }     
        else
        {
            rend.material.SetColor("_Color", Color.red);
        }
}
    

    public void OnTriggerExit(Collider other)
    {
        //Debug.Log("No longer in contact with " + other.gameObject.name);
            
        // change default color to pink when screw is selected OnTriggerExit
        if (selectedFlag) 
        {
            default_color = selectedScrewMaterial.color;
        }
        rend.material.SetColor("_Color", default_color);

        //change the other collider color back 
        if (other.gameObject.GetComponent<Renderer>() != null)
        {
            other.gameObject.GetComponent<Renderer>().material.color = other_default_color;
        }

    }


}
