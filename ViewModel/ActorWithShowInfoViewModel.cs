using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel
{
    public class ActorWithShowInfoViewModel : ActorBaseViewModel
    {
        public ActorWithShowInfoViewModel()
        {
            Shows = new List<ShowBaseViewModel>();
        }

        public IEnumerable<ShowBaseViewModel> Shows { get; set; }
    }
}