using System;
using System.Collections.Generic;
using Astravent.Web.Wasm.DTO.Wrappers;

namespace Astravent.Web.Wasm.Models.Reports
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
