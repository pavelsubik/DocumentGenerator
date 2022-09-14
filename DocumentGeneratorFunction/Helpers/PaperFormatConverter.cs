using System;
using System.Collections.Generic;
using System.Text;
using PuppeteerSharp.Media;

namespace DocumentGeneratorFunction.Helpers
{
    public static class PaperFormatConverter
    {
        public static PaperFormat GetFromString(string format)
        {
            switch (format)
            {
                case "Letter":
                    return PaperFormat.Letter;
                case "Legal":
                    return PaperFormat.Legal;
                case "Tabloid":
                    return PaperFormat.Tabloid;
                case "Ledger":
                    return PaperFormat.Ledger;
                case "A0":
                    return PaperFormat.A0;
                case "A1":
                    return PaperFormat.A1;
                case "A2":
                    return PaperFormat.A2;
                case "A3":
                    return PaperFormat.A3;
                case "A4":
                    return PaperFormat.A4;
                case "A5":
                    return PaperFormat.A5;
                case "A6":
                    return PaperFormat.A6;
                default:
                    return PaperFormat.A4;
            }
        }
    }
}
