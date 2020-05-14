using System;

namespace GeoLearn.Pages
{

    public class MainMasterDetailPageMasterMenuItem
    {
        public MainMasterDetailPageMasterMenuItem()
        {
            TargetType = typeof(MainMasterDetailPageMasterMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}