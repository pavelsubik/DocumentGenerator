namespace DocumentGeneratorFunction.Domain
{
    public class Options
    {
        public MarginOptions Margins { get; set; }
        public string Format { get; set; }
        public bool Landscape { get; set; }
        public bool PreferCssPageSize { get; set; }
        public bool PrintBackground { get; set; }
        public int Scale { get; set; }
        public bool DisplayHeaderFooter { get; set; }

    }
}
