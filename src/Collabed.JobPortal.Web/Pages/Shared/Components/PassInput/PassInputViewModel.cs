namespace Collabed.JobPortal.Web.Pages.Shared.Components.PassInput
{
    public class PassInputViewModel
    {
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
        public string InputType { get; set; }
        public string Placeholder { get; set; }
        public string HtmlTitle { get; set; }
        public string PrependIcon { get; set; }
        public string AppendIcon { get; set; }
        public string ErrorAppendIcon { get; set; }
        public string OnKeyUp { get; set; }
        public string Label { get; set; }
        public string Hint { get; set; }
        public bool IsPassword { get; set; }
        public bool InlineWithPassword { get; set; }
    }
}
