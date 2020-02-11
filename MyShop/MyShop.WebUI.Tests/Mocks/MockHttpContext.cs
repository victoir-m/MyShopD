using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.WebUI.Tests.Mocks
{
    public class MockHttpContext : HttpContextBase
    {
        private MockResponse response;
        private MockRequest request;
        private HttpCookieCollection httpCookie;

        public MockHttpContext()
        {
            httpCookie = new HttpCookieCollection();
            this.request = new MockRequest(httpCookie);
            this.response = new MockResponse(httpCookie);
        }

        public override HttpRequestBase Request
        {
            get
            {
                return request;
            }
        }

        public override HttpResponseBase Response
        {
            get
            {
                return response;
            }
        }
    }

    public class MockResponse : HttpResponseBase
    {
        private readonly HttpCookieCollection httpCookie;

        public MockResponse(HttpCookieCollection httpCookie)
        {
            this.httpCookie = httpCookie;
        }

        public override HttpCookieCollection Cookies
        {
            get
            {
                return httpCookie;
            }
        }
    }
    public class MockRequest : HttpRequestBase
    {
        private readonly HttpCookieCollection httpCookie;

        public MockRequest(HttpCookieCollection httpCookie)
        {
            this.httpCookie = httpCookie;
        }

        public override HttpCookieCollection Cookies
        {
            get
            {
                return httpCookie;
            }
        }
    }
}
