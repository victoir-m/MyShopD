using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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
        private IPrincipal fakeUser;

        public MockHttpContext()
        {
            httpCookie = new HttpCookieCollection();
            this.request = new MockRequest(httpCookie);
            this.response = new MockResponse(httpCookie);
        }

        public override IPrincipal User 
        {
            get
            {
                return this.fakeUser;
            }
            set
            {
                this.fakeUser = value;
            }
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
