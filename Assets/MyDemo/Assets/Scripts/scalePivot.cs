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
        pivot.transform.localPosition = originalPosition;
        pivot.transform.localRotation = originalRotation;
        pivot.transform.localScale = originalScale;
        boundsControl = pivot.AddComponent<BoundsControl>();
        pivot.gameObject.AddComponent<ScaleConstraint>();

        this.transform.localPosition = new Vector3(0, -1, 0);
        this.transform.localRotation = new Quaternion(0, 0, 0, 0);
        this.transform.localScale = new Vector3(1, 1, 1);
        boundsControl_original = this.GetComponent<BoundsControl>();
        Destroy(boundsControl_original);

        this.transform.SetParent(pivot.transform, false);

        //adjust handles size by de/reactivating pivot gameobject
        pivot.SetActive(false);
        pivot.SetActive(true);


        //transform the child, align its center to the screw end point closest to the lat/med plate
        GameObject latPlate = GameObject.Find("Plate_Lat");
        GameObject medPlate = GameObject.Find("Plate_Med");
    }
/*
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
                follow.localPosition = new Vector3(0, 1, 0);

            }
            else
            {
                follow.localPosition = new Vector3(0, -1, 0);

            }
        }
        else
        {
            double d1 = (ep1.z - medPlate.transform.position.z);
            double d2 = (ep2.z - medPlate.transform.position.z);
            if (d1 < d2)
            {
                follow.localPosition = new Vector3(0, -1, 0);

            }
            else
            {
                follow.localPosition = new Vector3(0, 1, 0);
            }
        }

        follow.localRotation = Quaternion.identity; // no relative rotation to the screw
        follow.localScale = new Vector3(1, 1, 1);
        //save the original transform
        originalLocalPosition = follow.localPosition;
        originalLocalRotation = follow.localRotation;
        originalGlobalPosition = follow.position;

        //by deactivating and activating the screwChild the handles will be shown
        screwChild.SetActive(false);

        //hide scale handles of child
        boundsControl.ScaleHandlesConfig.ShowScaleHandles = false;
        boundsControl.RotationHandlesConfig.ShowHandleForX = true;
        //boundsControl.RotationHandlesConfig.ShowHandleForY = true;
        boundsControl.RotationHandlesConfig.ShowHandleForZ = true;
        screwChild.GetComponentInChildren<BoxCollider>().enabled = false;

        //enable onScaleStopped function
        boundsControl_parent.ScaleStopped.AddListener(OnScaleStopped);
        boundsControl_parent.ScaleStarted.AddListener(OnScaleStarted);
        scaling = false;

    }
    private void Update()
    {
        selected = this.GetComponent<OnTrigger>().selectedFlag;

        //only show handles etc when screw is selected
        if (!selected)
        {
            screwChild.SetActive(false);
        }
        else
        {
            screwChild.SetActive(true);
            //anchor the screw child position if in scaling mode
            if (scaling)
            {
                follow.position = originalGlobalPosition;
                screwChild.SetActive(false);
            }


            transform.position = follow.position;


        }


    }

    public void OnScaleStopped()
    {
        scaling = false;
        screwChild.SetActive(false);
        screwChild.SetActive(true);
        //Debug.Log(" scale stopped ");
    }

    public void OnScaleStarted()
    {
        scaling = true;
    }
*/

}
