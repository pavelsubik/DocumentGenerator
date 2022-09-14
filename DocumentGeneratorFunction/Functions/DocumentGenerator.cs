using System;
using System.Net;
using System.Threading.Tasks;
using DocumentGeneratorFunction.Domain;
using DocumentGeneratorFunction.Processors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace DocumentGeneratorFunction.Functions
{
    public class DocumentGenerator
    {
        //[FunctionName("ProcessDocxTemplate")]
        //[OpenApiOperation(operationId: "ProcessDocxTemplate", tags: new[] { "name" })]
        //[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DocumentResponse), Description = "The OK response")]
        //[OpenApiRequestBody(contentType: "application/json", bodyType: typeof(DocumentRequest), Description = "Request object")]
        //public async Task<IActionResult> ProcessDocxTemplate(
        //    [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)][FromBody] DocumentRequest documentRequest,
        //    ILogger log)
        //{

        //    var template = Convert.FromBase64String(documentRequest.Template);
        //    var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(documentRequest.Data);
        //    var documentProcessor = new DocumentProcessor();
        //    var documentBase64 = documentProcessor.GenerateDocument(template, data, documentRequest.Type);

        //    return new OkObjectResult(new DocumentResponse
        //    {
        //        Document = documentBase64
        //    });
        //}

        [FunctionName("ProcessHtmlTemplate")]
        [OpenApiOperation(operationId: "ProcessHtmlTemplate", tags: new[] { "name" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(DocumentResponse), Description = "The OK response")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(DocumentRequest), Description = "Request object")]

        //public async Task<HttpResponseMessage> ProcessHtmlTemplate(
        public async Task<IActionResult> ProcessHtmlTemplate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)][FromBody] DocumentRequest documentRequest,
            ILogger log)
        {
            try
            {
                if (documentRequest?.Template == null)
                {
                    return new BadRequestResult();
                }

                var data = documentRequest.Data;
                var template = documentRequest.Template;
                var options = documentRequest.Options;
                var download = documentRequest.Download;
                log.LogInformation($"Process started. Template: {template}, Data: {data}, Options {options}");
                documentRequest.HeaderTemplate = string.IsNullOrEmpty(documentRequest.HeaderTemplate)
                    ? " "
                    : documentRequest.HeaderTemplate;
                documentRequest.FooterTemplate = string.IsNullOrEmpty(documentRequest.FooterTemplate)
                    ? " "
                    : documentRequest.FooterTemplate;
                var documentProcessor = new DocumentProcessor();
                var pdfDocumentBytes = await documentProcessor.GenerateDocument(template, documentRequest.HeaderTemplate, documentRequest.FooterTemplate, data, options);
                //var response = new HttpResponseMessage
                //{
                //    Content = new ByteArrayContent(pdfDocumentBytes),
                //    StatusCode = HttpStatusCode.OK,
                //};

                //application/pdf 
                //return response;
                log.LogInformation($"Process finished. Document length: {pdfDocumentBytes.Length}");

                if (download)
                {
                    return new FileContentResult(pdfDocumentBytes, "application/pdf");
                }
                else
                {
                    return new OkObjectResult(new DocumentResponse
                    {
                        Document = Convert.ToBase64String(pdfDocumentBytes)
                    });
                }
                

            }
            catch (Exception e)
            {
                log.LogError(e, e.Message);
                throw;
            }
           
        }
    }
}

