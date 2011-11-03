using System.Collections.Generic;

namespace KTouch.Units {

    /// <summary>
    /// Defines a static dictionnary of pages used in the application.
    /// </summary>
    public static class PageControl {

        public static Dictionary<string, string> PageDictionnary = new Dictionary<string, string>() {
           {"Solutions", "Views/SolutionsView.xaml"},
           {"Réalisations", "Views/RealisationsView.xaml"},
           {"Stages", "Views/StagesView.xaml"},
           {"Accueil", "Views/FrontView.xaml"},
           {"PresentationPage", "Views/PresentationPage.xaml"},
           {"MainPage", "Views/MainPage.xaml"},
        };
    }
}
