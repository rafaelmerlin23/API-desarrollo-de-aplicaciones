﻿using System.Security.Claims;

namespace foroLIS_backend.Services
{
    public class CurrentService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public CurrentService(IHttpContextAccessor contextAccessor) 
        {
            _contextAccessor = contextAccessor;
        }
        public string? GetUserId()
        {
            var userId = _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId;
        }
    }
}
