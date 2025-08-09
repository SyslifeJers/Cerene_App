namespace Cerene_App
{
    public static class ApiConfig
    {
        public const string BaseUrl = "https://ejemplo.com/api/";
        public static string InsertExamen => BaseUrl + "insert_examen.php";
        public static string InsertSeccion => BaseUrl + "insert_seccion.php";
    }
}
