using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;

public class followChild : MonoBehaviour
{
    private GameObject screwChild;
    private Transform follow;
    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;
    private Vector3 originalGlobalPosition;
    private Vector3 originalScale;
    public BoundsControl boundsControl;
    private bool selected;
    private bool scaling;
    private BoundsControl boundsControl_parent;


    private void Start()
    {
        //create a child for the screw
        screwChild = new GameObject("screwChild");
        boundsControl = screwChild.AddComponent<BoundsControl>();

        boundsControl_parent = gameObject.GetComponent<BoundsControl>();

        screwChild.transform.SetParent(this.transform);
        follow = screwChild.transform;

        //transform the child, align its center to the screw end point closest to the lat/med plate
        GameObject latPlate = GameObject.Find("Plate_Lat");
        GameObject medPlate = GameObject.Find("Plate_Med");

        //set the child position of the screw to its corresponding plate
        //using the fact that the lat plate is closer to the origin in z direction than the med plate
        Vector3 ep1 = gameObject.transform.position + gameObject.transform.up*gameObject.transform.localScale.y/2;
        Vector3 ep2 = gameObject.transform.position - gameObject.transform.up*gameObject.transform.localScale.y/2;
        if(gameObject.tag == "Lat")
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

        //enable event listeners 
        boundsControl_parent.ScaleStopped.AddListener(OnScaleStopped);
        boundsControl_parent.ScaleStarted.AddListener(OnScaleStarted);
        scaling = false;

    }    
    private void Update()
    {
        selected=this.GetComponent<OnTrigger>().selectedFlag;

        //only show handles when screw is selected
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
            
            //HAS TO BE IN THIS ORDER
            //sort of "reverses" the quaternion so that the local rotation is 0 if it is equal to the original local rotation
            follow.RotateAround(follow.position, follow.forward, -originalLocalRotation.eulerAngles.z);
            follow.RotateAround(follow.position, follow.right, -originalLocalRotation.eulerAngles.x);
            follow.RotateAround(follow.position, follow.up, -originalLocalRotation.eulerAngles.y);

            //rotate the parent
            transform.rotation = follow.rotation;

            //moves the parent by the child's original offset from the parent
            transform.position += -transform.right * originalLocalPosition.x;
            transform.position += -transform.TransformVector(new Vector3(0, 1, 0)) * originalLocalPosition.y;
            transform.position += -transform.forward * originalLocalPosition.z;


            //resets local rotation, undoing step 2
            follow.localRotation = originalLocalRotation;

            //reset local position to make sure screw doesn't slide away
            //reset global position to enable translation of the screw when not scaling
            if (!scaling) 
            {
               follow.localPosition = originalLocalPosition;
               originalGlobalPosition = follow.position;
            }

        }


    }
    
    public void OnScaleStopped()
    {
        scaling = false;
        screwChild.SetActive(false);
        screwChild.SetActive(true);
    }
      
    public void OnScaleStarted()
    {   
        scaling = true;
    }


}
