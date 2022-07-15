using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Platform.Services
{
    public class TextResponseFormatter : IResponseFormatter
    {
        private int responceCounter = 0;

        static TextResponseFormatter shared;

        public async Task Format(HttpContext context, string content)
        {
            await context.Response.WriteAsync($"Response {++responceCounter}: \n{context}");
        }

        public static TextResponseFormatter Singleton
        {
            get
            {
                if (shared == null)
                {
                    shared = new TextResponseFormatter();
                }
                return shared;
            }
        }
    }
}
