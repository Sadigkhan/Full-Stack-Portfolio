#pragma checksum "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "687d1eb4f530980e2fbf0d3c12c105506fa6c7c1"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__MyFavoritePartial), @"mvc.1.0.view", @"/Views/Shared/_MyFavoritePartial.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"687d1eb4f530980e2fbf0d3c12c105506fa6c7c1", @"/Views/Shared/_MyFavoritePartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8e9e96fd415c714e2615418161759ee43489adb5", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__MyFavoritePartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<WishVM>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("styles_removeButton DeleteFromWishtBtn"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", new global::Microsoft.AspNetCore.Html.HtmlString("button"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "DeleteWish", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Wish", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("styles_productImage-span-img"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("alt", new global::Microsoft.AspNetCore.Html.HtmlString("Alternate Text"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("styles_imageWrapper"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "product", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_8 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "ProductDetail", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_9 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("styles_productTitleWrap"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_10 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("styles_reviewsNumber"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_11 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("styles_submitOrderButton AddToCartBtn"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_12 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "AddBasket", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_13 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Product", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormActionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml"
  
    var Title = "";


#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"styles_cartPopupItems addtocart\" style=\"position: relative\">\r\n");
#nullable restore
#line 9 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml"
     if (Model.Count > 0)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div class=\"styles_cartPopupItems-products\" style=\"overflow-y: auto; max-height: 50rem; padding: 1rem\">\r\n");
#nullable restore
#line 12 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml"
             foreach (WishVM wishVM in Model)
            {
                Title = wishVM.Title + " " + string.Join(" ", wishVM.Product.ProductDetails.OrderByDescending(x => x.DetailKey.UpdatedAt).Where(x => x.DetailKey.ForTitle).Select(p => p.DetailValue.Name));


#line default
#line hidden
#nullable disable
            WriteLiteral("                <div class=\"styles_cartProductItem\">\r\n                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("button", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "687d1eb4f530980e2fbf0d3c12c105506fa6c7c19571", async() => {
                WriteLiteral(@"
                        <svg class=""styles_removeIcon"" xmlns=""http://www.w3.org/2000/svg"" width=""12"" height=""13"" viewBox=""0 0 12 13""><path fill=""#bebebe"" d=""M.826 12.474a.83.83 0 0 1-.59-.25.83.83 0 0 1 0-1.17L10.461.838a.83.83 0 0 1 1.162 0 .83.83 0 0 1 0 1.18L1.416 12.224a.83.83 0 0 1-.59.249z""></path><path fill=""#bebebe"" d=""M11.042 12.474a.83.83 0 0 1-.581-.24L.237 2.016A.834.834 0 1 1 1.416.837l10.207 10.225a.83.83 0 0 1-.58 1.412z""></path></svg>
                    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormActionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.Action = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.Controller = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.FormActionTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 17 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml"
                                                                                                                                         WriteLiteral(wishVM.ProductId);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                    <div class=\"styles_cartProductItemWrapper\">\r\n                        <button class=\"styles_navigateButton\">\r\n                            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "687d1eb4f530980e2fbf0d3c12c105506fa6c7c112906", async() => {
                WriteLiteral("\r\n                                <div class=\"styles_productImage\">\r\n                                    <span class=\"styles_productImage-span\">\r\n                                        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "687d1eb4f530980e2fbf0d3c12c105506fa6c7c113354", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
                BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                AddHtmlAttributeValue("", 1763, "~/User/assets/img/ProductImg/", 1763, 29, true);
#nullable restore
#line 25 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml"
AddHtmlAttributeValue("", 1792, wishVM.Image, 1792, 13, false);

#line default
#line hidden
#nullable disable
                EndAddHtmlAttributeValues(__tagHelperExecutionContext);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n                                    </span>\r\n                                </div>\r\n                            ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_7.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_7);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_8.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_8);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 22 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml"
                                                                                                                 WriteLiteral(wishVM.ProductId);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                        </button>\r\n\r\n                        <div class=\"styles_productInfo\">\r\n                            <button class=\"styles_navigateButton\">\r\n                                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "687d1eb4f530980e2fbf0d3c12c105506fa6c7c117703", async() => {
                WriteLiteral("\r\n                                    <div class=\"styles_textProductItem\">\r\n                                        <h6 class=\"styles_productTitle\">");
#nullable restore
#line 35 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml"
                                                                   Write(Title);

#line default
#line hidden
#nullable disable
                WriteLiteral("</h6>\r\n                                    </div>\r\n\r\n                                ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_9);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_7.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_7);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_8.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_8);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 33 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml"
                                                                                                                         WriteLiteral(wishVM.ProductId);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                            </button>\r\n                            <div class=\"styles_reviews\">\r\n                                <div class=\"styles_stars\">\r\n\r\n");
#nullable restore
#line 43 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml"
                                     for (int i = 1; i <= 5; i++)
                                    {
                                        if ((wishVM.Reviews != null && wishVM.Reviews.Count() > 0) && i <= (int)Math.Ceiling(wishVM.Reviews.Average(s => s.Star)))
                                        {


#line default
#line hidden
#nullable disable
            WriteLiteral(@"                                            <svg class=""rew-stared"" width=""16.125"" height=""16.52"" viewBox=""0 0 16.125 16.52""><g id=""star_empty"" data-name=""star empty"" transform=""matrix(0.309, 0.951, -0.951, 0.309, 12.457, -0.139)""><path id=""Path_935"" data-name=""Path 935"" d=""M6.944.66,8.631,4.653l4.318.371a.34.34,0,0,1,.194.6L9.867,8.458l.982,4.222a.34.34,0,0,1-.507.368L6.631,10.81,2.919,13.048a.34.34,0,0,1-.507-.368l.982-4.222L.118,5.619a.34.34,0,0,1,.194-.6L4.63,4.652,6.317.66a.34.34,0,0,1,.627,0Z"" transform=""translate(0)"" fill=""#ddd""></path></g></svg>
");
#nullable restore
#line 49 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml"

                                        }
                                        else
                                        {


#line default
#line hidden
#nullable disable
            WriteLiteral(@"                                            <svg width=""16.125"" height=""16.52"" viewBox=""0 0 16.125 16.52""><g id=""star_empty"" data-name=""star empty"" transform=""matrix(0.309, 0.951, -0.951, 0.309, 12.457, -0.139)""><path id=""Path_935"" data-name=""Path 935"" d=""M6.944.66,8.631,4.653l4.318.371a.34.34,0,0,1,.194.6L9.867,8.458l.982,4.222a.34.34,0,0,1-.507.368L6.631,10.81,2.919,13.048a.34.34,0,0,1-.507-.368l.982-4.222L.118,5.619a.34.34,0,0,1,.194-.6L4.63,4.652,6.317.66a.34.34,0,0,1,.627,0Z"" transform=""translate(0)"" fill=""#ddd""></path></g></svg>
");
#nullable restore
#line 55 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml"

                                        }
                                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("                                </div>\r\n                                <button class=\"styles_navigateButton\">\r\n                                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "687d1eb4f530980e2fbf0d3c12c105506fa6c7c123389", async() => {
                WriteLiteral("\r\n                                        <span class=\"styles_tips-text\">");
#nullable restore
#line 61 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml"
                                                                  Write(wishVM.Reviews.Count());

#line default
#line hidden
#nullable disable
                WriteLiteral(" rəy</span>\r\n                                    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_10);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_7.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_7);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_8.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_8);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 60 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml"
                                                                                                                          WriteLiteral(wishVM.ProductId);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
                                </button>


                            </div>
                            <div class=""styles_productTagWrapper addcart"">

                            </div>
                            <div class=""styles_productPriceControls addcart"">
                                <div class=""styles_productAmount"">
                                    <div class=""styles_buttons_addCart"">
");
#nullable restore
#line 73 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml"
                                         if (wishVM.Product.Count > 0)
                                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                            <a class=\"styles_cartButton-checkout \">\r\n                                                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("button", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "687d1eb4f530980e2fbf0d3c12c105506fa6c7c127236", async() => {
                WriteLiteral(@"
                                                    <span class=""styles_buttonText"">
                                                        Səbətə əlavə et
                                                    </span>
                                                    <svg class=""styles_buttonArrow"" xmlns=""http://www.w3.org/2000/svg"" width=""22"" height=""10"" viewBox=""0 0 22 10""><g><g><g><g><path fill=""#191919"" d=""M16.71.294a1.004 1.004 0 0 0-1.42 1.42l2.3 2.29H1a1 1 0 0 0 0 2h16.59l-2.3 2.29a1 1 0 0 0 0 1.42 1 1 0 0 0 1.42 0l4.7-4.71z""></path></g></g></g></g></svg>
                                                ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormActionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_11);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.Action = (string)__tagHelperAttribute_12.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_12);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.Controller = (string)__tagHelperAttribute_13.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_13);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.FormActionTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 76 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml"
                                                                                                                                                        WriteLiteral(wishVM.ProductId);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_FormActionTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            BeginWriteTagHelperAttribute();
#nullable restore
#line 76 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml"
                                                                                                                                                                                                         Write(wishVM.ProductId);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __tagHelperExecutionContext.AddHtmlAttribute("data-id", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                                            </a>\r\n");
#nullable restore
#line 83 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml"
                                        }
                                        else
                                        {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                                            <a class=""styles_cartButton-checkout "">
                                                <button disabled class=""styles_submitOrderButton AddToCartBtn disabled "" style=""background-color: #808080 !important"">
                                                    <span class=""styles_buttonText"">
                                                        Mövcud deyil
                                                    </span>
                                                    <svg class=""styles_buttonArrow"" xmlns=""http://www.w3.org/2000/svg"" width=""22"" height=""10"" viewBox=""0 0 22 10""><g><g><g><g><path fill=""#191919"" d=""M16.71.294a1.004 1.004 0 0 0-1.42 1.42l2.3 2.29H1a1 1 0 0 0 0 2h16.59l-2.3 2.29a1 1 0 0 0 0 1.42 1 1 0 0 0 1.42 0l4.7-4.71z""></path></g></g></g></g></svg>
                                                </button>
                                            </a>
");
#nullable restore
#line 94 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml"

                                        }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                                    </div>
                                </div>
                                <div class=""styles_priceWrapper"">
                                    <div>

                                        <span class=""styles_priceWrapper-text"">");
#nullable restore
#line 102 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml"
                                                                           Write((Math.Truncate(wishVM.Price)));

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n");
#nullable restore
#line 103 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml"
                                         if ((int)(((wishVM.Price) - (int)(wishVM.Price)) * 100) > 0)
                                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                            <span class=\"styles_priceWrapper-sub\">.");
#nullable restore
#line 105 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml"
                                                                               Write((int)(((wishVM.Price) - (int)(wishVM.Price)) * 100));

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n");
#nullable restore
#line 106 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml"
                                        }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                                        <svg class=""styles_priceWrapper-svg"" xmlns=""http://www.w3.org/2000/svg"" width=""15"" height=""13"" viewBox=""0 0 15 13""><path fill=""#171717"" d=""M8.11 1.78L8.092.388 6.983.86l-.029.938C.214 2.851 0 12.56 0 12.56l1.516.077c.096-8.41 4.847-9.656 5.407-9.774L6.65 11.91l1.57-.879-.098-8.168c5.33.717 5.33 9.772 5.33 9.772l1.545.085c.042-.489-.252-10.15-6.888-10.94z""></path></svg>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
");
#nullable restore
#line 115 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
        </div>
        <div class=""styles_cartPopupSummary"" style=""position: sticky; top: 100%; left: 100%; width: 100%; transform: translateX(0%);"">
            <div class=""styles_productInfo-bottom"">
                <span class=""styles_products-count"">
                    <span class=""styles_tipsLarge"">");
#nullable restore
#line 121 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml"
                                              Write(Model.Count);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n                    <span class=\"styles_tipsLarge\">Məhsul</span>\r\n                </span>\r\n\r\n            </div>\r\n\r\n        </div>\r\n");
#nullable restore
#line 128 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml"
    }
    else
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <h1 class=\"emptyCart\" style=\"width:70%;top:20%;\">Sizin Favorit siyahısı boşdur məhsul əlavə edərək alış-verişə davam edin</h1>\r\n");
#nullable restore
#line 132 "C:\Users\Admin\Desktop\Final Proje\Kontakt\Kontakt\Views\Shared\_MyFavoritePartial.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<WishVM>> Html { get; private set; }
    }
}
#pragma warning restore 1591
