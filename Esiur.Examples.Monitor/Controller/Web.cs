using Esiur.Core;
using Esiur.Net.HTTP;
using System;
using System.Text.RegularExpressions;
using Esiur.Resource;
using System.IO;
using Microsoft.AspNetCore.StaticFiles;

namespace Esiur.Examples.Monitor.Controller
{
    public class Web : HTTPFilter
    {
        [Attribute]
        public string RootPath
        {
            get;
            set;
        }

        FileExtensionContentTypeProvider extProvider = new();

        public async override AsyncReply<bool> Execute(HTTPConnection sender)
        {
            // skip websockets
            if (sender.WSMode)
                return false;

            string fn = null;

            // Needed for IUI router
            if (Regex.IsMatch(sender.Request.Filename, @"^/?css|img|js|font|iui-js-2|node_modules|snd|view|wgt|iui|lib|route"))
                fn = RootPath + "/" + sender.Request.Filename.TrimStart('/');
            else
            {
                fn = RootPath + "/index.html";
            }
            

            if (Path.GetExtension(fn) == null)    
                fn = RootPath + "/index.html";
        
            if (File.Exists(fn))
            {
                string contentType;

                if (!extProvider.TryGetContentType(fn, out contentType))
                    contentType = "application/octet-stream";

                sender.Response.Headers["Content-Type"] = contentType;
                sender.SendFile(fn).Wait(20000);
            }
            else
            {
                sender.Response.Number = Esiur.Net.Packets.HTTPResponsePacket.ResponseCode.NotFound;
                sender.Send("`" + Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "` Not Found");
            }

            return true;
        }


        public override AsyncReply<bool> Trigger(ResourceTrigger trigger)
        {
            RootPath = RootPath ?? "View";
            return new AsyncReply<bool>(true);
        }
    }
}
