using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseResponses
{
    public class AcknowledgementResponse : UseCaseResponseMessage
    {
        public IEnumerable<Error> Errors { get; }
        public string Id { get; set; }
        public dynamic CreatedDetails { get; set; }
        public dynamic ErroredDetails { get; set; }

        public AcknowledgementResponse(IEnumerable<Error> errors, bool success = false, string message = null) : base(success, message)
        {
            Errors = errors;
            Success = success;
            Message = message;
        }

        public AcknowledgementResponse(bool success, string message = null) : base(success, message)
        {
            Success = success;
            Message = message;
        }
        public AcknowledgementResponse(string id, bool success, string message = null) : base(success, message)
        {
            Id = id;
            Success = success;
            Message = message;
        }
        public AcknowledgementResponse(dynamic createdDetailsList, dynamic erroredDetailsList, bool success, string message = null) : base(success, message)
        {
            CreatedDetails = createdDetailsList;
            ErroredDetails = erroredDetailsList;
            Success = success;
            Message = message;
        }
    }
}
