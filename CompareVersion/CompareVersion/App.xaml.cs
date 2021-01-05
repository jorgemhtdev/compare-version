namespace CompareVersion
{
    using Xamarin.Forms;

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected async override void OnStart()
        {
            // Comprobar si tienes conexión a internet
            if (Device.RuntimePlatform == Device.iOS)
            {
                var result = await CheckVersion.CheckVerisonIos();

                if(result)
                    await Application.Current.MainPage.DisplayAlert("Nueva versión",
                        "Hay una nueva versión de la app, ¿quieres descargarla?",
                        "No", "Si");

                // Bloquear la app obligando a que el usuario actualice la app
                // Si indica que si abrir la app store
                    // var url = "https://apps.apple.com/es/app/spotify-música-en-streaming/id324684580";
                    // await Browser.OpenAsync(url, BrowserLaunchMode.External);
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                var result = await CheckVersion.CheckVerisonAndroid();

                if (result)
                    await Application.Current.MainPage.DisplayAlert("Nueva versión",
                        "Hay una nueva versión de la app, ¿quieres descargarla?",
                        "No", "Si");

                // Bloquear la app obligando a que el usuario actualice la app
                // Si indica que si abrir la app store
                    // var url = "https://play.google.com/store/apps/details?id=com.amazon.mShop.android.shopping&hl=es_419";
                    // await Browser.OpenAsync(url, BrowserLaunchMode.External);
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
