using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Launcher.Domain
{
    public class AnotherCommandImplementation : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public AnotherCommandImplementation(Action<object> execute)
            : this(execute, null)
        {
        }

        public AnotherCommandImplementation(Action<object> execute, Func<object, bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            _execute = execute;
            _canExecute = canExecute ?? (x => true);
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Refresh()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }


    //public class TreeExampleSimpleTemplateSelector : DataTemplateSelector
    //{
    //    public DataTemplate PlanetTemplate { get; set; }

    //    public DataTemplate SolarSystemTemplate { get; set; }

    //    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    //    {
    //        if (item is Planet)
    //            return PlanetTemplate;

    //        if (item?.ToString() == "Solar System")
    //            return SolarSystemTemplate;

    //        return TreeViewAssist.SuppressAdditionalTemplate;
    //    }
    //}

    public sealed class SubMenu
    {
        public SubMenu(string name, string imageSrc)
        {
            Name = name;
            ImageSrc = imageSrc;
        }

        public string Name { get; }

        public string ImageSrc { get; }
    }

    //public class Planet
    //{
    //    public string Name { get; set; }

    //    public double DistanceFromSun { get; set; }

    //    public double DistanceFromEarth { get; set; }

    //    public double Velocity { get; set; }
    //}

    public sealed class ProfessionalCategory
    {
        public ProfessionalCategory(string name, params SubMenu[] subMenu)
        {
            Name = name;
            SubMenus = new ObservableCollection<SubMenu>(subMenu);
        }

        public string Name { get; }

        public ObservableCollection<SubMenu> SubMenus { get; }
    }

    public sealed class TreesViewModel : ViewModelBase
    {
        private object _selectedItem;

        public ObservableCollection<ProfessionalCategory> ProfessionalCategories { get; }

        public AnotherCommandImplementation AddCommand { get; }

        public AnotherCommandImplementation RemoveSelectedItemCommand { get; }

        public object SelectedItem
        {
            get { return _selectedItem; }
            set => SetProperty(ref _selectedItem, value);
        }

        static int i = 0;
        public TreesViewModel(ObservableCollection<ProfessionalCategory> cates)
        {
            if(false)
            {
                ProfessionalCategories = new ObservableCollection<ProfessionalCategory>
                {
                    new ProfessionalCategory("Actionoooooo",
                        new SubMenu("Predator", "John McTiernan"),
                        new SubMenu("Alien", "Ridley Scott"),
                        new SubMenu("Prometheus", "Ridley Scott")),
                    new ProfessionalCategory("Comedy",
                        new SubMenu("EuroTrip", "Jeff Schaffer"),
                        new SubMenu("EuroTrip", "Jeff Schaffer")
                    )
                };
                return;
            }

            ProfessionalCategories = new ObservableCollection<ProfessionalCategory>
            {
                new ProfessionalCategory("Action",
                    new SubMenu("Predator", "John McTiernan"),
                    new SubMenu("Alien", "Ridley Scott"),
                    new SubMenu("Prometheus", "Ridley Scott")),
                new ProfessionalCategory("Comedy",
                    new SubMenu("EuroTrip", "Jeff Schaffer"),
                    new SubMenu("EuroTrip", "Jeff Schaffer")
                )
            };

            AddCommand = new AnotherCommandImplementation(
                _ =>
                {
                    if (!ProfessionalCategories.Any())
                    {
                        ProfessionalCategories.Add(new ProfessionalCategory(GenerateString(15)));
                    }
                    else
                    {
                        var index = new Random().Next(0, ProfessionalCategories.Count);

                        ProfessionalCategories[index].SubMenus.Add(
                            new SubMenu(GenerateString(15), GenerateString(20)));
                    }
                });

            RemoveSelectedItemCommand = new AnotherCommandImplementation(
                _ =>
                {
                    var category = SelectedItem as ProfessionalCategory;
                    if (category != null)
                    {
                        ProfessionalCategories.Remove(category);
                    }
                    else
                    {
                        var m = SelectedItem as SubMenu;
                        if (m == null) return;
                        ProfessionalCategories.FirstOrDefault(v => v.SubMenus.Contains(m))?.SubMenus.Remove(m);
                    }
                },
                _ => SelectedItem != null);
        }

        private static string GenerateString(int length)
        {
            var random = new Random();

            return string.Join(string.Empty,
                Enumerable.Range(0, length)
                .Select(v => (char)random.Next('a', 'z' + 1)));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
