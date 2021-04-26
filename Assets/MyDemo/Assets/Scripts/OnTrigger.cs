using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTrigger : MonoBehaviour
{
    private Color default_color;
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
        default_color = rend.material.GetColor("_Color");
        
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
        //ignore collisions with screwChild
        if (other.gameObject.transform == screwChild.transform) 
        {
            return;
        }
        else
        {
            Debug.Log(gameObject.name + " was triggered by " + other.gameObject.name);

            // change default color to pink when screw is selected         
            if (selectedFlag)
            {
                default_color = selectedScrewMaterial.color;
            }

            //change color to red when colliding
            rend.material.SetColor("_Color", Color.red);

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
       
        else
        {
            rend.material.SetColor("_Color", Color.red);
        }
}
    

    public void OnTriggerExit(Collider other)
    {
        //Debug.Log("No longer in contact with " + other.gameObject.name);
        
            rend.material.SetColor("_Color", default_color);
        
        
        
    }

  
}
