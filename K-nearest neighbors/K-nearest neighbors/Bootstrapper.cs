using Caliburn.Micro;
using K_nearest_neighbors.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace K_nearest_neighbors
{
    public class Bootstrapper : BootstrapperBase
    {
        #region Constructor
        public Bootstrapper()
        {
            Initialize();
        }
        #endregion

        #region Overrides
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
             DisplayRootViewFor<MainViewModel>();
        }
        #endregion
    }
}
