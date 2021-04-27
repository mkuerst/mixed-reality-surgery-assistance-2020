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
    public BoundsControl boundsControl;
    private bool selected;
    

    private void Start()
    {
        //create a child for the screw
        screwChild = new GameObject("screwChild");

        boundsControl = screwChild.AddComponent<BoundsControl>();

        screwChild.transform.SetParent(this.transform);
        follow = screwChild.transform;
        //transform the child, align its center to the screw end point
        follow.localPosition = new Vector3(0, 1, 0); //could be (0,-1, 0) as well
        follow.localRotation = Quaternion.identity; // no relative rotation to the screw
        follow.localScale = new Vector3(1, 1, 1);
        //save the original tranform
        originalLocalPosition = follow.localPosition;
        originalLocalRotation = follow.localRotation;

        //Marina: by deactivating and activating the screwChild the handles will be shown
        screwChild.SetActive(false);
        //hide scale handles
        boundsControl.ScaleHandlesConfig.ShowScaleHandles = false;
        screwChild.GetComponentInChildren<BoxCollider>().enabled = false;
    }
    private void Update()
    {
        selected=this.GetComponent<OnTrigger>().selectedFlag;

        //only show handles etc when screw is selected
        if (!selected)
        {
            screwChild.SetActive(false);
        }
        else
        {
            screwChild.SetActive(true);
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

            //reset local position
            follow.localPosition = originalLocalPosition;
        }
        
    }
}
