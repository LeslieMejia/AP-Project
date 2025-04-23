using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EarlyBirdAPI.Model.Entities;
public class Resume
{
    public int Id { get; set; }

    public int JobseekerId { get; set; } //Foreign Key to UserId
    public string Content { get; set; }
    public User User { get; set; }    // Optional: Easier Navigation to User
}

