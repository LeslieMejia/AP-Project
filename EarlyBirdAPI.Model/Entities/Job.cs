using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EarlyBirdAPI.Model.Entities
{
    public class Job
    {
        public int Id { get; set; }

        public int EmployerId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Location { get; set; }

        public string? SalaryRange { get; set; }

        public string? Category { get; set; }

        public JobStatus Status { get; set; }
    }
}
