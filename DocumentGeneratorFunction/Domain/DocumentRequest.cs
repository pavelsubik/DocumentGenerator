using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentGeneratorFunction.Domain
{
    public class DocumentRequest
    {
        public string Template { get; set; }
        public string HeaderTemplate { get; set; }
        public string FooterTemplate { get; set; }
        public string Data { get; set; }
        public Options Options { get; set; }
        public bool Download { get; set; }

    }
}
