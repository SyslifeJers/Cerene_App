namespace Cerene_App
{
    public static class ApiConfig
    {
        public const string BaseUrl = "https://terapia.clinicacerene.com/api/";
        public static string InsertExamen => BaseUrl + "insertExam.php";
        public static string InsertSeccion => BaseUrl + "insertSecciones.php";
        public static string InsertPreguntas => BaseUrl + "insertPreguntas.php";
        public static string GetAreas => BaseUrl + "get_areas.php";
        public static string GetUsuarios => BaseUrl + "get_usuario.php";
    }
}
