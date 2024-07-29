using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializationAndDeserializationXml
{
    internal class PersonalVM : BindableObject
    {
        private Person? person;

        public Person? Person
        {
            get => person;
            set
            {
                person = value;
                OnPropertyChanged(nameof(Person));
            }
        }
        public Command SaveCommand { get; set; }
        public Command LoadCommand { get; set; }

        public PersonalVM()
        {
            Person = new Person();
            SaveCommand = new Command(OnSave);
            LoadCommand = new Command(OnLoad);
        }

        private string GetFilePath(string fileName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);
        }

        private void ShowAlert(string title, string message)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert(title, message, "Ok");
                }
            });
        }

        private void OnLoad()
        {

            try
            {
                Person? loadPerson = XmlSerializerService.DeserializeFromXml<Person>(GetFilePath("person.xml"));
                if (loadPerson != null)
                {
                    Person = loadPerson;
                    ShowAlert("Loaded", "Personal Details loaded successfully");
                }
            }
            catch (Exception ex)
            {

                ShowAlert("Exception", ex.Message);
            }
        }

        private void OnSave()
        {
            try
            {
                XmlSerializerService.SerializeToXml(GetFilePath("person.xml"), Person);
                ShowAlert("Saved", "Details saved successfully");
            }
            catch (Exception ex)
            {
                ShowAlert("Exception", ex.Message);
            }
        }
    }
}
