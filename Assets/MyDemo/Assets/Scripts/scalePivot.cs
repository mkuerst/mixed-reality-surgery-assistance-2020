using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;

public class scalePivot : MonoBehaviour
{
    private GameObject pivot;
    private GameObject screwparent;
   
    private Quaternion originalRotation;
    private Vector3 originalPosition;
    private Vector3 originalScale;

    public BoundsControl boundsControl;
    private bool selected;
    private bool scaling;
    private BoundsControl boundsControl_original;


    private void Start()
    {
        //get original param of screw with resp to parent
        originalPosition = this.transform.localPosition;
        originalRotation = this.transform.localRotation;
        originalScale = this.transform.localScale;

        screwparent = this.transform.parent.gameObject;
        //create a parent for the screw
        pivot = new GameObject("Pivot");
        pivot.transform.SetParent(screwparent.transform);
        pivot.transform.localPosition = originalPosition - originalScale / 2; //needs to be adapted!!! now screws aren't in same position as they should
        pivot.transform.localRotation = originalRotation;
        pivot.transform.localScale = originalScale;
       



        //transform the child, align its center to the screw end point closest to the lat/med plate
        GameObject latPlate = GameObject.Find("Plate_Lat");
        GameObject medPlate = GameObject.Find("Plate_Med");

        //set the child position of the screw to its corresponding plate
        //using the fact that the lat plate is closer to the origin in z direction than the med plate
        Vector3 ep1 = gameObject.transform.position + gameObject.transform.up * gameObject.transform.localScale.y / 2;
        Vector3 ep2 = gameObject.transform.position - gameObject.transform.up * gameObject.transform.localScale.y / 2;
        if (gameObject.tag == "Lat")
        {
            double d1 = (ep1.z - latPlate.transform.position.z);
            double d2 = (ep2.z - latPlate.transform.position.z);
            if (d1 < d2)
            {
                this.transform.localPosition = new Vector3(0, -1, 0);

            }
            else
            {
                this.transform.localPosition = new Vector3(0, 1, 0);

            }
        }
        else
        {
            double d1 = (ep1.z - medPlate.transform.position.z);
            double d2 = (ep2.z - medPlate.transform.position.z);
            if (d1 < d2)
            {
                this.transform.localPosition = new Vector3(0, 1, 0);

            }
            else
            {
                this.transform.localPosition = new Vector3(0, -1, 0);
            }
        }

        this.transform.localRotation = Quaternion.identity;
        this.transform.localScale = new Vector3(1, 1, 1);
        /*
        //remove bounds control of screw
        boundsControl_original = this.GetComponent<BoundsControl>();
        Destroy(boundsControl_original);*/

        //set pivot as parent
        this.transform.SetParent(pivot.transform, false); //maybe set to true

        //adjust handles size by de/reactivating pivot gameobject
        pivot.SetActive(false);
        pivot.SetActive(true);

        // such that box is ignored by collision detection?
        pivot.gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
    }

    private void Update()
    {
        selected = this.GetComponent<OnTrigger>().selectedFlag;

        if (selected)
        {
            //enable boundscontrol of pivot
            boundsControl = pivot.AddComponent<BoundsControl>();
            pivot.gameObject.AddComponent<ScaleConstraint>();

            //remove bounds control of screw
            boundsControl_original = this.GetComponent<BoundsControl>();
            Destroy(boundsControl_original);

            pivot.SetActive(false);
            pivot.SetActive(true);

        }
        else 
        {
            this.gameObject.AddComponent<BoundsControl>();
        }
        
    }
 

}
