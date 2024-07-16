using System;
using System.Collections.Generic;
using MiniSpacePwa.DTO.Wrappers;

namespace MiniSpacePwa.Models.Reports
{
    public class GetStudentReportsModel
    {
        public int Page { get; set; }
        public int Results { get; set; }

        public GetStudentReportsModel()
        {
            SetDefaultValues();
        }
        
        public void SetDefaultValues()
        {
            Page = 1;
            Results = 5;
        }
    }
}
