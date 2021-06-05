using System;
using System.Collections.Generic;

namespace Web.Api.Models.Request
{
    public class SearchRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ExtractData { get; set; }//all, no, yes
        public string SendClaim { get; set; }//all, no, yes
        public DateTime SendClaimOn { get; set; }
        public string CallStatus { get; set; }//all, pending, called
        public string FieldAllocationStatus { get; set; }//all, allowed, notallowed
        public string RequestOrCRMTypeIndex { get; set; }//all, 1, 2, 3
        public string UserType { get; set; }
        public string TeamName { get; set; }//
    }
}