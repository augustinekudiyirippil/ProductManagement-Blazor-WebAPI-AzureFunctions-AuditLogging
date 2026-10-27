using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ProductLogs.DTOs;

namespace ProductLogs;

using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using ProductLogs.Models;

public class ProductAuditFunction
{
    private readonly LoggingDbContext _dbContext;

    public ProductAuditFunction(
        LoggingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [Function("ProductAudit")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(
           // AuthorizationLevel.Function,
        AuthorizationLevel.Anonymous,
            "post")]
        HttpRequestData req)
    {
        var request =
            await JsonSerializer.DeserializeAsync<ProductAuditRequest>(
                req.Body);

        if (request == null)
        {
            var badResponse =
                req.CreateResponse(
                    HttpStatusCode.BadRequest);

            return badResponse;
        }

        var log = new ProductLog
        {
            ProductId = request.ProductId,
            UserId = request.UserId,
            ActionType = request.ActionType,
            ActionDateTime = DateTime.UtcNow,
            Description = request.Description
        };

        _dbContext.ProductLog.Add(log);

        await _dbContext.SaveChangesAsync();

        var response =
            req.CreateResponse(
                HttpStatusCode.OK);

        await response.WriteStringAsync(
            "Audit Log Created Successfully");

        return response;





        //[Function("ProductAudit")]
        //public HttpResponseData Run(
        //   [HttpTrigger(
        //        AuthorizationLevel.Anonymous,
        //        "get",
        //        "post")]
        //    HttpRequestData req)
        //{
        //    var response =
        //        req.CreateResponse(HttpStatusCode.OK);

        //    response.WriteString(
        //        "Function Loaded Successfully");

        //    return response;
    }

}
