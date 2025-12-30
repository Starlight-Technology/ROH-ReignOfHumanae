//-----------------------------------------------------------------------
// <copyright file="PlayerPositionModel.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ROH.StandardModels.Character.Position
{
    public class PlayerPositionModel
    {
        public virtual PositionModel? Position { get; set; }

        public virtual RotationModel? Rotation { get; set; }
    }
}