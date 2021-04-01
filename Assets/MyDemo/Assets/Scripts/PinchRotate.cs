// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

namespace Microsoft.MixedReality.Toolkit.UI
{

    public class PinchRotate : TransformConstraint
    {
        //objectif: rotate the screw aroun one end, the default rotation is around the center
        //the idea is to proprocessing the instruction from the hand gester and move the screw in a fake way
        //Constraint Manager provide such proprocessing structre, it is possible to define new constraints
        //https://docs.microsoft.com/en-us/windows/mixed-reality/mrtk-unity/features/ux-building-blocks/constraint-manager


        public override TransformFlags ConstraintType => TransformFlags.Move & TransformFlags.Rotate;

        public override void Initialize(MixedRealityTransform worldPose)
        {
            base.Initialize(worldPose);

        }

        public override void ApplyConstraint(ref MixedRealityTransform transform)
        {
            //rotate with half the real angle
            Quaternion rotation = transform.Rotation * Quaternion.Inverse(worldPoseOnManipulationStart.Rotation);
            Vector3 eulers = rotation.eulerAngles;
            transform.Rotation = Quaternion.Euler(eulers*.5f) * worldPoseOnManipulationStart.Rotation;

            //objectif: translate the screw so that one end looks like still
            //the trick thing is to find the direction of the translation and the magnitude
            //get the screw axis using TransformDirection method
            Vector3 unitVector = new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 objectAxis = gameObject.transform.TransformDirection(unitVector);
            //get the rotation axis
            float angle = 0;
            Vector3 axis = Vector3.zero;
            rotation.ToAngleAxis(out angle, out axis);
            //the translation direction is orthogonal to the rotation axis and the screw axis
            Vector3 translationDirection = Vector3.Cross(objectAxis, axis).normalized;
            //get the size of the screw through its collider
            float size = gameObject.GetComponent<CapsuleCollider>().height;
            float magnitude = 0.5f * size * angle;
            //translate the screw
            transform.Position += magnitude * translationDirection;
        }


    }
}
