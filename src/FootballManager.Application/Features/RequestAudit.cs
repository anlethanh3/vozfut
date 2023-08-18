using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FootballManager.Application.Features
{
    public abstract record RequestAudit
    {
        [BindNever]
        [JsonIgnore]
        public string RequestedBy { get; set; }

        [BindNever]
        [JsonIgnore]
        public DateTime RequestedAt { get; set; }

        [BindNever]
        [JsonIgnore]
        public string RequestUserId { get; set; }
    }
}
