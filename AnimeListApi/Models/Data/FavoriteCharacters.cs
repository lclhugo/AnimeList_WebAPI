﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace AnimeListApi.Models.Data;

public partial class Favoritecharacters
{
    public int Favid { get; set; }

    public Guid? Userid { get; set; }

    public int? Characterid { get; set; }

    public virtual Characters Character { get; set; }

    public virtual Profiles User { get; set; }
}