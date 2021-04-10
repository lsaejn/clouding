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
                        new SubMenu("Predator", "John McTiernan"),
                        new SubMenu("Alien", "Ridley Scott"),
                        new SubMenu("Prometheus", "Ridley Scott")),
                    new ProfessionalCategory(Name,
                        new SubMenu("Predator", "John McTiernan"),
                        new SubMenu("Alien", "Ridley Scott"),
                        new SubMenu("Prometheus", "Ridley Scott")),
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
