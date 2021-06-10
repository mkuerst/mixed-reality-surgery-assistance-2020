using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//screw should only be scaled in one direction
public class ScrewScaleConstraint : TransformConstraint
{
    public override TransformFlags ConstraintType => TransformFlags.Scale;

    public override void ApplyConstraint(ref MixedRealityTransform transform)
    {
        Vector3 scale = transform.Scale;
       // Vector3 pos= transform.Position;

        scale.x = worldPoseOnManipulationStart.Scale.x;
        scale.z = worldPoseOnManipulationStart.Scale.z;
        //pos.x= worldPoseOnManipulationStart.Position.x;
        //pos.y = worldPoseOnManipulationStart.Position.y;
        //pos.z = worldPoseOnManipulationStart.Position.z;
        transform.Scale = scale;
        //transform.Position= pos;
    }
}
