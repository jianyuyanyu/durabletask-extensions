﻿using System.ComponentModel.DataAnnotations;

namespace LLL.DurableTask.Api.Models
{
    public class TerminateRequest
    {
        public string Reason { get; set; }
    }
}
