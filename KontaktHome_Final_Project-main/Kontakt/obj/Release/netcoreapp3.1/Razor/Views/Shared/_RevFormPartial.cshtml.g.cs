#pragma checksum "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_RevFormPartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e30ecd42327fd47754dcd85551bdb0b0c8672ef3"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__RevFormPartial), @"mvc.1.0.view", @"/Views/Shared/_RevFormPartial.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 2 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\_ViewImports.cshtml"
using Kontakt.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\_ViewImports.cshtml"
using Kontakt.ViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\_ViewImports.cshtml"
using Kontakt.Services;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e30ecd42327fd47754dcd85551bdb0b0c8672ef3", @"/Views/Shared/_RevFormPartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8e9e96fd415c714e2615418161759ee43489adb5", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__RevFormPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"
<div class=""rev-form-items"">
    <div class=""text-danger rew-validation-summary""></div>
    <h3 class=""rev-form-title col-lg-12 col-12"">Rəy yazmaq</h3>
    <div class=""rev-star-items col-lg-12 col-12"">
        <input id=""star-val"" type=""hidden"" name=""name"" value=""1"" />
        <div class=""rev-stars"">
            <button class=""rev-star-btn"" data-rate=""1"">
                <span class=""star-rait""><i class=""fas fa-star active-post""></i></span>
            </button>
            <button class=""rev-star-btn"" data-rate=""2"">
                <span class=""star-rait""><i class=""fas fa-star""></i></span>
            </button>
            <button class=""rev-star-btn"" data-rate=""3"">
                <span class=""star-rait""><i class=""fas fa-star""></i></span>
            </button>
            <button class=""rev-star-btn"" data-rate=""4"">
                <span class=""star-rait""><i class=""fas fa-star""></i></span>
            </button>
            <button class=""rev-star-btn"" data-rate=""5"">
                <span");
            WriteLiteral(@" class=""star-rait""><i class=""fas fa-star""></i></span>
            </button>
        </div>
        <span class=""rev-star-text"">
            <span class=""styles_inherit-text"">Qiymətləndirmə</span>
            &nbsp;
            <span class=""styles_valueNumber""><span class=""rated-star"">1</span>/5</span>
        </span>
    </div>
    <div class=""rev-message col-lg-12 col-12"">
        <h6 class=""rev-message-title"">Rəy: *</h6>
        <div class=""styles_textareaComponent"">
            <div class=""styles_textareaBlock"">
                <div class=""styles_textareaWrapper"">
                    <div class=""styles_textareaInner"">
                        <textarea class=""styles_textarea"" placeholder=""Rəy bildirin""></textarea>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
