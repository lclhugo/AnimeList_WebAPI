﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace AnimeListApi.Models.Data;

public partial class Animelist
{
    public int Listid { get; set; }

    public Guid? Userid { get; set; }

    public int? Animeid { get; set; }

    public int? Statusid { get; set; }

    public int? Watchedepisodes { get; set; }

    public int? Rating { get; set; }

    public DateTime? Created { get; set; }

    public DateTime? Lastupdated { get; set; }

    public virtual Anime Anime { get; set; }

    public virtual Status Status { get; set; }

    public virtual Profiles User { get; set; }
}