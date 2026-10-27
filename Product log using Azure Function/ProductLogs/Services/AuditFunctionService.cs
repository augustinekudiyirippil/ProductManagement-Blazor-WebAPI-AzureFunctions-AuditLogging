using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using ProductLogs.DTOs;

namespace ProductLogs.Services
{
    public class AuditFunctionService
    {
        private readonly HttpClient _httpClient;

        public AuditFunctionService(
            HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task LogAsync(
            ProductAuditRequest request)
        {
            await _httpClient.PostAsJsonAsync(
                "http://localhost:7062/api/ProductAudit",
                request);
        }

    }
}
