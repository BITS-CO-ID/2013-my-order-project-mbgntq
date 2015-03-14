<%@ WebHandler Language="C#" Class="Upload" %>

using System;
using System.Web;

public class Upload : IHttpHandler {
   

    public void ProcessRequest(HttpContext context)
    {
        HttpPostedFile uploads = context.Request.Files["upload"];
        string CKEditorFuncNum = context.Request["CKEditorFuncNum"];
        string file = System.IO.Path.GetFileName(uploads.FileName);
        uploads.SaveAs(context.Server.MapPath("~/Upload/ComplaintsImage/" + file));
        string url = "/Upload/ComplaintsImage/" + file;
        context.Response.Write("<script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + url + "\");</script>");
        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}