using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Vmeet.Utility;

namespace Vmeet.Controllers
{
    public class VideosController : ApiController
    {
        public HttpResponseMessage Get(string filename, string ext)
        {
            var video = new VideoStream(filename, ext);

            var response = Request.CreateResponse();
            response.Content = new PushStreamContent ((Action<System.IO.Stream, HttpContent, TransportContext>) video.WriteToStream,new MediaTypeHeaderValue("audio/"+ext));

            return response;
        }
    }
}