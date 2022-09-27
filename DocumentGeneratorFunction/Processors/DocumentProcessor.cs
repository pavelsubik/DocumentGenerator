using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentGeneratorFunction.Domain;
using DocumentGeneratorFunction.Helpers;
using DotLiquid;
using Newtonsoft.Json;
using PuppeteerSharp;
using MarginOptions = PuppeteerSharp.Media.MarginOptions;

namespace DocumentGeneratorFunction.Processors
{
    public class DocumentProcessor
    {

        public async Task<byte[]> GenerateDocument(string htmlTemplate, string htmlHeaderTemplate, string htmlFooterTemplate, string jsonData, Options options)
        {
            

            string render;
            if (!string.IsNullOrWhiteSpace(jsonData))
            {
                var json = JsonConvert.DeserializeObject<IDictionary<string, object>>(jsonData, new DictionaryConverter());
                var jsonHash = Hash.FromDictionary(json);


                var template = Template.Parse(htmlTemplate);
                render = template.Render(jsonHash);
            }
            else
            {
                render = htmlTemplate;
            }


            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Args = new[] { "--no-sandbox" },
                Headless = true
            });
            var page = await browser.NewPageAsync();
            // await page.SetContentAsync(render);
            //var result = await page.GetContentAsync();
            
            await page.GoToAsync("data:text/html," + render, WaitUntilNavigation.Networkidle0);
            await page.SetContentAsync(render);

            byte[] pdf;
            if (options != null)
            {

                pdf = await page.PdfDataAsync(new PdfOptions
                {
                    MarginOptions = new MarginOptions
                    {
                        Top = options.Margins.Top,
                        Bottom = options.Margins.Bottom,
                        Left = options.Margins.Left,
                        Right = options.Margins.Right
                    },
                    Format = PaperFormatConverter.GetFromString(options.Format),
                    PreferCSSPageSize = options.PreferCssPageSize,
                    Landscape = options.Landscape,
                    PrintBackground = options.PrintBackground,
                    Scale = options.Scale>=10 && options.Scale <=200 ? (decimal)(options.Scale / 100.0) : 1,
                    DisplayHeaderFooter = options.DisplayHeaderFooter,
                    HeaderTemplate = string.IsNullOrEmpty(htmlHeaderTemplate) ? " " : htmlHeaderTemplate,
                    FooterTemplate = string.IsNullOrEmpty(htmlFooterTemplate) ? " " : htmlFooterTemplate

                });
            }
            else
            {
                pdf = await page.PdfDataAsync();
            }

            await browser.CloseAsync();
            return pdf;
           
        }
    }
}