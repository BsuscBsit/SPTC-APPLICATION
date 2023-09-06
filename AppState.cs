using SPTC_APPLICATION.Database;
using System.Windows;
using SPTC_APPLICATION.Objects;
using SPTC_APPLICATION.View.Pages;
using SPTC_APPLICATION.View;

namespace SPTC_APPLICATION
{
    public static class AppState
    {
        
        public static bool IS_ADMIN = false;
        public static Employee USER = null;
        public static string EXPIRATION_DATE = "2023 - 2024";
        public static string CHAIRMAN = "ROLLY M. LABINDAO";
        public static string REGISTRATION_NO = "9520-03006397";

        public static void Login(string username, string password, Window window)
        {
            dynamic result = Retrieve.Login(username, password);

            if (result is View.ControlWindow controlWindow)
            {
                EventLogger.Post($"User :: Login Failed: USER({username})");
                //DEBUG THIS ON OTHER PC
                //CreateEmployee(); //result in password :: 751cb3f4aa17c36186f4856c8982bf27
            }
            else if (result is Employee employee)
            {

                USER = employee;
                //(new PrintPreview()).Show();
                //(new Test()).Show();
                (new MainBody()).Show();
                EventLogger.Post($"User :: Login Success: USER({username})");
                window.Close();
            }
        }

        public static void Logout(Window window)
        {
            IS_ADMIN = false;
            USER = null;
            EventLogger.Post($"User :: Logout Success");
            (new Login()).Show();
            window.Close();
        }
    }

}
