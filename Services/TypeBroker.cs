namespace Platform.Services
{
    public static class TypeBroker
    {
        static IResponseFormatter formatter = new HtmlResponseFormatter();

        public static IResponseFormatter Formatter => formatter;
    }
}
