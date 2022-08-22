namespace Platform
{
    public static class ResponseString
    {
        public static string DefaultResponse = @"
            <!DOCTYPE html>
            <html>
            <head>
                <link rel=""stylesheet"" href=""lib/twitter-bootstrap/css/bootstrap.min.css""/>
                <meta charset=""utf-8"" />
                <title>Error</title>
            </head>
            <body class=""text-center"">
                <h3 class=""p-2"">Что то пошло не так... Ошибка {0}</h3>
                <h6>Вы можете перейти на главную страницу <a href=""/"">Домой</a> и попробовать еще раз</h6>

            </body>
            </html>";
    }
}
