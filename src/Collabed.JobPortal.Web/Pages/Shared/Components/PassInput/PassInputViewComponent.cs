using Collabed.JobPortal.Web.Pages.Shared.Components.PassInput;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Collabed.JobPortal.Web.Pages.Shared.Components.Input
{
    public class PassInputViewComponent : ViewComponent
    {
        /// <summary>
        /// Renders HTML input field 
        /// </summary>
        /// <param name="propertyName">Assigns the input field's value to Model's property by name</param>
        /// <param name="inputType">Acceptable values = text, password</param>
        /// <param name="htmlTitle">HTML title attribute</param>
        /// <param name="placeholder">Field's placeholder</param>
        /// <param name="propertyValue">Initial value of input field</param>
        /// <param name="prependIcon">Prepend icon name - if empty string, will not be visible</param>
        /// <param name="appendIcon">Append icon name - if empty string, will not be visible</param>
        /// <param name="onKeyUp">JS Function to perform when onKeyUp event will be fired. Requires the JS function's name visible in the Razor Page's scope.</param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(string propertyName, string inputType, string htmlTitle, string placeholder, string propertyValue = "", string prependIcon = "",
            string appendIcon = "", string onKeyUp = "", string label = "", string hint = "", bool isPassword = false, bool inlineWithPassword = false)
        {
            return View(new PassInputViewModel()
            {
                PropertyName = propertyName,
                PropertyValue = propertyValue,
                InputType = inputType,
                HtmlTitle = htmlTitle,
                Placeholder = placeholder,
                PrependIcon = prependIcon,
                AppendIcon = appendIcon,
                OnKeyUp = onKeyUp,
                Label = label,
                Hint = hint,
                IsPassword = isPassword,
                InlineWithPassword = inlineWithPassword
            });
        }
    }
}
