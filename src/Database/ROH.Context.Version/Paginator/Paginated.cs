//-----------------------------------------------------------------------
// <copyright file="Paginated.cs" company="Starlight-Technology">
//     Author: https://github.com/Starlight-Technology/ROH-ReignOfHumanae
//     Copyright (c) Starlight-Technology. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
// Ignore Spelling: Paginator

namespace ROH.Context.Version.Paginator;

public record Paginated(int Total, ICollection<dynamic> ObjectResponse);