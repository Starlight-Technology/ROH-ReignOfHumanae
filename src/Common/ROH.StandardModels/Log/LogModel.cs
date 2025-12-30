//-----------------------------------------------------------------------
// <copyright file="LogModel.cs" company="Starlight-Technology">
//     Author:  
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace ROH.StandardModels.Log
{
    public class LogModel
    {
        public DateTime Date { get; }

        public string? Message { get; set; }

        public Severity Severity { get; set; }
    }
}