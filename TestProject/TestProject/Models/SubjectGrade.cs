﻿using System;
using System.Collections.Generic;

namespace TestProject.Models
{
    public partial class SubjectGrade
    {
        public string ClassName { get; set; }
        public int StudentId { get; set; }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Grade { get; set; }
        public string SubjectName { get; set; }

        public Students Students { get; set; }
    }
}
