#pragma checksum "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyOrdersPartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8f724dfe18ca8b01332ef429fa74a2320d45ff39"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__MyOrdersPartial), @"mvc.1.0.view", @"/Views/Shared/_MyOrdersPartial.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8f724dfe18ca8b01332ef429fa74a2320d45ff39", @"/Views/Shared/_MyOrdersPartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8e9e96fd415c714e2615418161759ee43489adb5", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__MyOrdersPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<UserProfileVM>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("styles_imageWrap-img"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("alt", new global::Microsoft.AspNetCore.Html.HtmlString("Alternate Text"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n\r\n\r\n");
#nullable restore
#line 5 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyOrdersPartial.cshtml"
 foreach (Order order in Model.Orders)
{

#line default
#line hidden
#nullable disable
            WriteLiteral(@"    <div class=""Order-list-body"">
        <div class=""styles_ordersGrid"">
            <div class=""styles_panelTitle"">
                <div class=""styles_orderTitle"">
                    <div class=""styles_statusInfo"">
                        <span class=""styles_status-text"">");
#nullable restore
#line 12 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyOrdersPartial.cshtml"
                                                    Write(order.Status);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</span>
                    </div>
                </div>
                <span class=""styles_panelInner styles_panelInner-addres"">Ünvan</span>
                <span class=""styles_panelInner styles_panelInner-delivery"">Sifariş tarixi:</span>
                <span class=""styles_panelInner styles_panelInner-price"">Qiymət:</span>
            </div>
            <div class=""styles_panelBody"">
                <div class=""styles_bodyInner styles_orderBody"">
");
#nullable restore
#line 21 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyOrdersPartial.cshtml"
                     foreach (OrderItem orderItem in order.OrderItems.Take(2))
                    {


#line default
#line hidden
#nullable disable
            WriteLiteral("                        <div class=\"styles_productsWrap\">\r\n                            <figure class=\"styles_imageWrap-figure\">\r\n                                <span class=\"styles_imageWrap-img-span\">\r\n                                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "8f724dfe18ca8b01332ef429fa74a2320d45ff395896", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 1211, "~/User/assets/img/ProductImg/", 1211, 29, true);
#nullable restore
#line 27 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyOrdersPartial.cshtml"
AddHtmlAttributeValue("", 1240, orderItem.Product.MainImage, 1240, 28, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n                                </span>\r\n                            </figure>\r\n                        </div>\r\n");
#nullable restore
#line 32 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyOrdersPartial.cshtml"


                    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 35 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyOrdersPartial.cshtml"
                     if ((order.OrderItems.Count() - 2) > 0)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <span class=\"styles_maxElements\">+");
#nullable restore
#line 37 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyOrdersPartial.cshtml"
                                                      Write(order.OrderItems.Count()-2);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n");
#nullable restore
#line 38 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyOrdersPartial.cshtml"
                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n                </div>\r\n                <div class=\"styles_bodyInner styles_addressBody\">\r\n                    <address class=\"styles_adress\">");
#nullable restore
#line 43 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyOrdersPartial.cshtml"
                                              Write(order.Country);

#line default
#line hidden
#nullable disable
            WriteLiteral(", ");
#nullable restore
#line 43 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyOrdersPartial.cshtml"
                                                              Write(order.City);

#line default
#line hidden
#nullable disable
            WriteLiteral(",<br>");
#nullable restore
#line 43 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyOrdersPartial.cshtml"
                                                                              Write(order.State);

#line default
#line hidden
#nullable disable
            WriteLiteral(",");
#nullable restore
#line 43 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyOrdersPartial.cshtml"
                                                                                           Write(order.AppUser.PhoneNumber);

#line default
#line hidden
#nullable disable
            WriteLiteral("</address>\r\n                </div>\r\n                <div class=\"styles_bodyInner styles_deliveryBody\">\r\n                    <span class=\"styles_delivey-date\">");
#nullable restore
#line 46 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyOrdersPartial.cshtml"
                                                 Write(order.CreatedAt);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</span>
                </div>
                <div class=""styles_bodyInner styles_priceBody"">
                    <div class=""styles_priceWrapper"">
                        <div class=""styles_priceWrapper-text"">
                            <span class=""styles_priceWrapper-text-main"">
                                <h4 class=""styles_priceWrapper-text-main-text"">");
#nullable restore
#line 52 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyOrdersPartial.cshtml"
                                                                           Write((Math.Truncate(order.TotalPrice)));

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n                            </span>\r\n                           \r\n                            <span class=\"styles_sup\">\r\n");
#nullable restore
#line 56 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyOrdersPartial.cshtml"
                                 if (((int)(((order.TotalPrice) - (int)(order.TotalPrice)) * 100)) > 0)
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <span class=\"styles_sub-text\">.");
#nullable restore
#line 58 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyOrdersPartial.cshtml"
                                                               Write((int)(((order.TotalPrice) - (int)(order.TotalPrice)) * 100));

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n");
#nullable restore
#line 59 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyOrdersPartial.cshtml"
                                }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                            </span>
                            

                        </div>
                        <svg class=""price-icon"" xmlns=""http://www.w3.org/2000/svg"" width=""15"" height=""13"" viewBox=""0 0 15 13""><path fill=""#171717"" d=""M8.11 1.78L8.092.388 6.983.86l-.029.938C.214 2.851 0 12.56 0 12.56l1.516.077c.096-8.41 4.847-9.656 5.407-9.774L6.65 11.91l1.57-.879-.098-8.168c5.33.717 5.33 9.772 5.33 9.772l1.545.085c.042-.489-.252-10.15-6.888-10.94z""></path></svg>
                    </div>
                </div>
            </div>
        </div>
        
        <button type=""button"" class=""styles-order-view-btn"" data-orderId=""");
#nullable restore
#line 71 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyOrdersPartial.cshtml"
                                                                     Write(order.Id);

#line default
#line hidden
#nullable disable
            WriteLiteral(@""">
            <svg class=""styles-order-view-btn-svg"" xmlns=""http://www.w3.org/2000/svg"" width=""12"" height=""8"" viewBox=""0 0 12 8""><path fill=""#171717"" d=""M.694 1.49A.926.926 0 0 1 2.277.831L6.25 4.813 10.221.832a.93.93 0 1 1 1.315 1.315L6.249 7.424.963 2.147a.926.926 0 0 1-.269-.658z""></path></svg>
        </button>
    </div>
");
#nullable restore
#line 75 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyOrdersPartial.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<UserProfileVM> Html { get; private set; }
    }
}
#pragma warning restore 1591