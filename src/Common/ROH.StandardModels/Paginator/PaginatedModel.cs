//-----------------------------------------------------------------------
// <copyright file="PaginatedModel.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
// Ignore Spelling: Paginator

using System.Collections.Generic;

namespace ROH.StandardModels.Paginator
{
    public class PaginatedModel
    {
        public ICollection<dynamic>? ObjectResponse { get; set; }

        public int? TotalPages { get; set; } = 0;
    }
}