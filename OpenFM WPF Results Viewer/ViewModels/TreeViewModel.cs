using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SharedModels;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OpenFM_WPF.Services;

namespace OpenFM_WPF.ViewModels
{
    class TreeViewModel : INotifyPropertyChanged
    {
        #region PROPERTIES
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<SharedModels.Models.SavedObjects.Channel> _channels 
            = new ObservableCollection<SharedModels.Models.SavedObjects.Channel>();
        public ObservableCollection<SharedModels.Models.SavedObjects.Channel> Channels
        {
            get { return _channels; }
            set
            {
                _channels = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public TreeViewModel()
        {
            try
            {
                var fileReader = new FileReader();
                foreach (var channel in fileReader.GetData())
                    _channels.Add(channel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }












        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [AttributeUsage(AttributeTargets.Method)]
        public sealed class NotifyPropertyChangedInvocatorAttribute : Attribute
        {
            public NotifyPropertyChangedInvocatorAttribute() { }
            public NotifyPropertyChangedInvocatorAttribute([NotNull] string parameterName)
            {
                ParameterName = parameterName;
            }

            [CanBeNull] public string ParameterName { get; private set; }
        }

        [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property |
        AttributeTargets.Delegate | AttributeTargets.Field | AttributeTargets.Event |
        AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.GenericParameter)]
        public sealed class CanBeNullAttribute : Attribute { }

        [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property |
        AttributeTargets.Delegate | AttributeTargets.Field | AttributeTargets.Event |
        AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.GenericParameter)]
        public sealed class NotNullAttribute : Attribute { }
    }
}
