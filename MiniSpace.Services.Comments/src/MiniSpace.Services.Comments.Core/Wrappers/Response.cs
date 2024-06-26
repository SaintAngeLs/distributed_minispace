﻿using System.Diagnostics.CodeAnalysis;

namespace MiniSpace.Services.Comments.Application.Wrappers
{
    [ExcludeFromCodeCoverage]
    public class Response<T>
    {
        public T Content { get; set; }
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }
        public string Message { get; set; }
    }
}