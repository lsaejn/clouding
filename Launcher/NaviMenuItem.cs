using Launcher.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher
{
    class NaviMenuItem
    {
        public string Name { get; set; }
        public int Index { get; set; }

        /// <summary>
        /// 树控件菜单名
        /// </summary>
        public string _menuFile;
        public object MenuFile
        {
            get { return _menuFile; }
            set
            {
                _menuFile = (string)value;
                InitElements();
            }
        }
        public ObservableCollection<ProfessionalCategory> _cates;
        public void InitElements()
        {
            //ProfessionalCategory _cates=
            _cates = new ObservableCollection<ProfessionalCategory>
                {
                    new ProfessionalCategory(Name,
                        new SubMenu("Predator", "D://hello"),
                        new SubMenu("Alien", "D://hello"),
                        new SubMenu("Prometheus", "D://hello")),
                    new ProfessionalCategory(Name,
                        new SubMenu("Predator", "C://hello"),
                        new SubMenu("Alien", "C://hello"),
                        new SubMenu("Prometheus", "C://hello")),
                   new ProfessionalCategory(Name,
                        new SubMenu("Predator", "John McTiernan"),
                        new SubMenu("Alien", "Ridley Scott"),
                        new SubMenu("Prometheus", "Ridley Scott")),
                    new ProfessionalCategory("Comedy",
                        new SubMenu("EuroTrip", "Jeff Schaffer"),
                        new SubMenu("EuroTrip", "Jeff Schaffer")
                    )
                };
        }
    }
}
