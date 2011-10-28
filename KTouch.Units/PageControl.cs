using System.Collections.Generic;

namespace KTouch.Units {

    /// <summary>
    /// 
    /// </summary>
    public static class PageControl {

        public static Dictionary<string, string> PageDictionnary = new Dictionary<string, string> ( ) {
           {"Solutions", "Views/SolutionsView.xaml"},
           {"Réalisations", "Views/RealisationsView.xaml"},
           {"Stages", "Views/StagesView.xaml"},
           {"Accueil", "Views/FrontView.xaml"},
        };
    }
}
