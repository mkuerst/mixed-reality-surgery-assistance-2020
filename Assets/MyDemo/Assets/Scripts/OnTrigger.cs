using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTrigger : MonoBehaviour
{
    private Color default_color;
    private Renderer rend;
    GameObject bone;
    GameObject bonemix;

    // Start is called before the first frame update
    void Start()
    {
        
        bone = GameObject.Find("Bone");
        bonemix = GameObject.Find("BoneMix");
        rend = gameObject.GetComponent<Renderer>();
        default_color = rend.material.GetColor("_Color");

    }

    // // Update is called once per frame
    // void Update()
    // {

    // }

    public void OnTriggerStay(Collider other)
    {
        //ignore collisions with bone
        if (other.gameObject.transform.parent.transform.IsChildOf(bone.transform)||other.gameObject.transform==bonemix.transform) //if (other.gameObject.transform.parent.parent.name == "Bone")
        {
            return;
            
        }
        else
        {
            Debug.Log(gameObject.name + " was triggered by " + other.gameObject.name);
            
            //change color when colliding
            rend.material.SetColor("_Color", Color.red);
            
        }

    }
    
    public void OnTriggerExit(Collider other)
    {
        Debug.Log("No longer in contact with " + other.gameObject.name);
        rend.material.SetColor("_Color", default_color);
    }
}
