using Pathogen;
using Pathogen.Models;
using Pathogen.Views.Cells;
using Xamarin.Forms;

namespace ChatUIXForms.Utilities
{
    class ChatTemplateSelector : DataTemplateSelector
    {
        DataTemplate inboundDataTemplate;
        DataTemplate outboundDataTemplate;

        public ChatTemplateSelector()
        {
            this.inboundDataTemplate = new DataTemplate(typeof(InboundViewCell));
            this.outboundDataTemplate = new DataTemplate(typeof(OutboundViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var messageVm = item as Message;
            if (messageVm == null)
                return null;

            return (messageVm.User == App.User) ? outboundDataTemplate : inboundDataTemplate;
        }
    }
}
