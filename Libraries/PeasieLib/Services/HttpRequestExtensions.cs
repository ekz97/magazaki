﻿using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace PeasieLib.Services
{
    public static class HttpRequestExtensions
    {
        public static async Task<T> ReadAsJsonAsync<T>(this HttpRequest request, JsonSerializerOptions? options = null)
        {
            request.Body.Position = 0;
            var result = await request.ReadFromJsonAsync<T>(options);
            // reset the position again to let endpoint middleware read it
            request.Body.Position = 0;
            return result;
        }
    }
}
